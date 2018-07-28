using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;
using FreeNet;
using LitJson;

namespace SP_Server
{
    public delegate void DelegateWriteLog(string str, string strFunc, string strFile, string strLine);

    public partial class Frm : Form
    {
        public DelegateWriteLog WriteLogInstance = null;

        protected bool m_bAutoScroll = true;        

        protected FileStream m_sFileStream;
        protected StreamWriter m_sStreamWrite;        

        protected bool m_bStartClose = false;        

        CNetworkService service = null;
        private static User adminUser;

        // 유저 리스트
        public static User GetAdminUser() { return adminUser; }        
        public static void SetAdminUser(User value) { adminUser = value; }
        private List<User> listUser;

        public List<User> ListUser { get => listUser; set => listUser = value; }

        /// <summary>
        ///  저장될 정보
        /// </summary>        

        public Dictionary<int, UserInfo> dictUserInfo = new Dictionary<int, UserInfo>();

        public int orderID = -1;
        public List<RequestOrder> listRequestOrder = new List<RequestOrder>();        

        public int musicID = -1;
        public List<RequestMusicInfo> listReqMusicInfo = new List<RequestMusicInfo>();        

        public Frm()
        {
            WriteLogInstance = new DelegateWriteLog(this.WriteLog);

            CreateLogFile();
            InitializeComponent();

            int div = this.listviewLog.Width / 7;

            this.colDesc.Width = div * 3;
            this.colFunc.Width = div * 2;
            this.colFile.Width = div;
            this.colDate.Width = div;

            this.FormClosing += new FormClosingEventHandler(this.frmClosing);
        }

        protected override void OnLoad(EventArgs e)
        {
            WriteLog("!!!!------ Server Start ------!!!");

            ListUser = new List<User>();            

            service = new CNetworkService(true);
            service.session_created_callback += on_session_created;
            service.initialize(1000, 1024);
            service.listen("0.0.0.0", 7979, 100);            
        }        

        void on_session_created(CUserToken token)
        {            
            User user = new User(token, this);
            lock(ListUser)
            {
                ListUser.Add(user);
            }

            //WriteLog("[connect cnt: {0}] any client Connected!!", get_concurrent_user_count().ToString());
        }

        public void remove_user(User user)
        {
            lock(ListUser)
            {
                ListUser.Remove(user);
            }
        }

        public void remove_user(int index)
        {
            lock(listUser)
            {
                listUser.RemoveAt(index);
            }
        }

        public int get_concurrent_user_count()
        {
            return ListUser.Count;
        }        

        private void OnBtnSend(object sender, EventArgs e)
        {
            String str = textBox1.Text;

            // 서버 기능적인 부분 추후 구현
            switch (str)
            {
                case "print_user_list":
                    if(adminUser != null)
                        WriteLog("[TableNo: " + adminUser.tableNum + "]");

                    for (int i = 0; i < ListUser.Count; i++)
                        WriteLog("[TableNo: " + ListUser[i].tableNum + "]");
                    break;
                case "save":
                    AllDataSave();
                    break;
                default:
                    MessageBox.Show("아직 기능없어잉~ ㅋㅋ");
                    break;
            }
        }        

        private void CB_AutoScroll_CheckedChanged(object sender, EventArgs e)
        {
            m_bAutoScroll = CB_AutoScroll.Checked ? true : false;
        }

        public void WriteLog(string str, string param)
        {
            str = str.Replace("{0}", param);            
            WriteLog(str);
        }

        public void WriteLog(string str)
        {            
            System.Diagnostics.StackFrame stackFrame = new System.Diagnostics.StackFrame(1, true);
            WriteLog(str, stackFrame.GetMethod().Name,
                stackFrame.GetFileName(), stackFrame.GetFileLineNumber().ToString());
        }

        public void WriteLog(string str, string strFunc, string strFile, string strLine)
        {
            strFile = strFile + " (" + strLine + ")";

            try
            {
                AddListItem(str, strFunc, strFile);
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void AddListItem(string strDesc, string strFunc, string strFile)
        {                        
            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Text = strDesc;
            listViewItem.SubItems.Add(strFunc);
            listViewItem.SubItems.Add(strFile);
            listViewItem.SubItems.Add(Convert.ToString(DateTime.Now));

            try
            {
                this.listviewLog.Items.Add(listViewItem);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            
            {
                strDesc = strDesc.Replace(",", ".");  // , 는 csv에서 구분문자이므로 못 사용하게 막음

                strDesc = strDesc + ",\t" + strFunc + ",\t" + strFile + ",\t" + DateTime.Now.ToString() + "\r\n";

                m_sStreamWrite.Write(strDesc);
                m_sStreamWrite.Flush();
                m_sFileStream.Flush();

                Int64 nSize = m_sFileStream.Length;

                if (nSize >= 2097152)  // 2메가가 넘었다. 파일 교체
                {
                    m_sStreamWrite.Close();
                    m_sFileStream.Close();
                    CreateLogFile();
                }
            }

            // 로그가 1000개 넘으면, 오래된것부터 삭제
            if (this.listviewLog.Items.Count > 1000)
                this.listviewLog.Items.RemoveAt(0);

            if (m_bAutoScroll)
            {
                this.listviewLog.EnsureVisible(listviewLog.Items.Count - 1);
            }
        }

        protected void CreateLogFile()
        {
            string strNowTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

            if (Directory.Exists("log") == false)
                Directory.CreateDirectory("log");

            m_sFileStream = new FileStream("log\\PosServer." + strNowTime + ".txt", FileMode.CreateNew);
            m_sStreamWrite = new StreamWriter(m_sFileStream, Encoding.UTF8);
        }

        private void frmClosing(object sender, FormClosingEventArgs e)
        {
            if (m_bStartClose == false)
            {
                DialogResult result = MessageBox.Show("정말로 종료하시겠습니까?", "EXIT", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {                    
                    WriteLog("!!!!------ Server Closing ------!!!");

                    m_bStartClose = true;

                    m_sFileStream.Close();
                    this.Close();
                }                    
            }

            if (m_bStartClose == false)
                e.Cancel = true;
        }        

        public void ReqClose()
        {            
            this.Close();
        }

        public bool AddUserInfo(int tableNo, ref UserInfo info)
        {
            bool isAdd = false;
            if (dictUserInfo.ContainsKey(tableNo) == false)
            {
                dictUserInfo.Add(tableNo, new UserInfo(tableNo));
                isAdd = true;
            }
            else
                info = dictUserInfo[tableNo];

            return isAdd;
        }

        public void SetUserInfo(int tableNo, UserInfo info)
        {
            if (dictUserInfo.ContainsKey(tableNo) == false)
                return;

            dictUserInfo[tableNo] = info;
        }

        public RequestMusicInfo AddRequestMusic(int tableNo, string title, string singer)
        {
            ++musicID;
            RequestMusicInfo reqMusic = new RequestMusicInfo(musicID, tableNo, title, singer);
            listReqMusicInfo.Add(reqMusic);
            DataRequestSave(false);

            return reqMusic;
        }

        public void RemoveRequestMusicInfo(int id)
        {
            if (listReqMusicInfo.Count <= 0)
            {
                WriteLog("No listings stored on the Server.");
                return;
            }

            int deleteIdx = -1;
            for (int i = 0; i <listReqMusicInfo.Count; i++)
            {
                if (id != listReqMusicInfo[i].id)
                    continue;

                deleteIdx = i;
            }

            if (deleteIdx == -1)
            {
                WriteLog("RequestMusicInfo Not Remove Because Not Find ID");
                return;
            }

            listReqMusicInfo.RemoveAt(deleteIdx);
            DataRequestSave(false);

            // 리스트가 없음으로 아이디 부여 초기화
            if (listReqMusicInfo.Count <= 0)
                musicID = -1;
        }

        public void SetRequestOrder(RequestOrder reqOrder)
        {
            listRequestOrder.Add(reqOrder);
            DataRequestSave(true);
        }

        public RequestOrder GetRequestOrder(int id)
        {
            RequestOrder reqOrder = null;
            for(int i = 0; i < listRequestOrder.Count; i++)
            {
                if (listRequestOrder[i].id != id)
                    continue;

                reqOrder = listRequestOrder[i];
                break;
            }

            return reqOrder;
        }

        public void RemoveRequestOrder(int id)
        {
            int findIdx = -1;
            for (int i = 0; i < listRequestOrder.Count; i++)
            {
                if (listRequestOrder[i].id != id)
                    continue;

                findIdx = i;
                break;
            }

            if (findIdx == -1)
                return;

            listRequestOrder.RemoveAt(findIdx);
            DataRequestSave(true);
        }

        public void SetOrder(int tableNo, string packing)
        {
            JsonData reqOrderJson = JsonMapper.ToObject(packing);
            for (int i = 0; i < reqOrderJson.Count; i++)
            {
                int reqSendMenu = int.Parse(reqOrderJson[i]["menu"].ToString());
                int reqSendCnt = int.Parse(reqOrderJson[i]["cnt"].ToString());

                SetOrder(tableNo, new SendMenu(reqSendMenu, reqSendCnt));
            }
        }

        public void SetOrder(int tableNo, SendMenu sendMenu)
        {
            if (dictUserInfo.ContainsKey(tableNo) == false)
                return;            
            
            List<SendMenu> listSendMenu = dictUserInfo[tableNo].menus;

            int containIdx = -1;
            for (int i = 0; i < listSendMenu.Count; i++)
            {
                if (listSendMenu[i].menu == sendMenu.menu)
                {
                    containIdx = i;
                    break;
                }
            }

            if (containIdx == -1)
                listSendMenu.Add(sendMenu);
            else
            {

                listSendMenu[containIdx].cnt += sendMenu.cnt;
                if (listSendMenu[containIdx].cnt <= 0)
                    listSendMenu.RemoveAt(containIdx);
            }

            DataUserInfoSave();
        }

        public List<SendMenu> GetOrder(int tableNo)
        {
            List<SendMenu> listSendMenu = new List<SendMenu>();

            if (dictUserInfo.ContainsKey(tableNo))
                listSendMenu = dictUserInfo[tableNo].menus;

            return listSendMenu;
        }

        public void SetDiscount(int tableNo, string packing)
        {
            SetDiscount(tableNo, short.Parse(packing));
        }

        public void SetDiscount(int tableNo, short discount)
        {
            if (dictUserInfo.ContainsKey(tableNo) == false)
                return;            

            dictUserInfo[tableNo].discounts.Add(discount);
            DataUserInfoSave();
        }

        public List<short> GetDiscount(int tableNo)
        {
            List<short> list = new List<short>();

            if (dictUserInfo.ContainsKey(tableNo))
                list = dictUserInfo[tableNo].discounts;

            return list;
        }

        public void SetUnfinishGame(int tableNo, Unfinish info)
        {
            if (dictUserInfo.ContainsKey(tableNo) == false)
                return;

            bool exist = dictUserInfo[tableNo].gameInfo.listUnfinish.Exists(x => x.id == info.id);
            if (exist)
                return;

            dictUserInfo[tableNo].gameInfo.listUnfinish.Add(info);
            DataUserInfoSave();
        }

        public List<Unfinish> GetUnfinishList(int tableNo)
        {
            if (dictUserInfo.ContainsKey(tableNo) == false)
                return new List<Unfinish>();

            return dictUserInfo[tableNo].gameInfo.listUnfinish;
        }

        public void RemoveUnfinishGame(int tableNo, int id)
        {
            if (dictUserInfo.ContainsKey(tableNo) == false)
                return;

            for (int i = 0; i < dictUserInfo[tableNo].gameInfo.listUnfinish.Count; i++)
            {
                Unfinish info = dictUserInfo[tableNo].gameInfo.listUnfinish[i];
                if (info.id != id)
                    continue;

                dictUserInfo[tableNo].gameInfo.listUnfinish.RemoveAt(i);
                break;
            }

            if (dictUserInfo[tableNo].gameInfo.listUnfinish.Count <= 0)
                dictUserInfo[tableNo].gameInfo.gameID = -1;

            DataUserInfoSave();
        }

        public void RefreshGameCount(int tableNo, int cnt)
        {
            if (dictUserInfo.ContainsKey(tableNo) == false)
                return;

            dictUserInfo[tableNo].gameInfo.gameCnt = cnt;
            DataUserInfoSave();
        }

        public int GetGameCount(int tableNo)
        {
            if (dictUserInfo.ContainsKey(tableNo) == false)
                return 0;

            return dictUserInfo[tableNo].gameInfo.gameCnt;
        }

        public void RemoveUserData(int tableNo)
        {
            if (dictUserInfo.ContainsKey(tableNo))
                dictUserInfo.Remove(tableNo);

            for (int i = listRequestOrder.Count - 1; i >= 0; i--)
            {
                if (listRequestOrder[i].tableNo != tableNo)
                    continue;

                listRequestOrder.RemoveAt(i);
            }

            for (int i = listReqMusicInfo.Count -1; i >= 0; i--)
            {
                if (listReqMusicInfo[i].tableNo != tableNo)
                    continue;

                listReqMusicInfo.RemoveAt(i);
            }

            int findIdx = -1;
            for (int i = 0; i < listUser.Count; i++)
            {
                if (listUser[i].tableNum != tableNo)
                    continue;

                findIdx = i;
                break;
            }

            if (findIdx == -1)
                return;

            remove_user(findIdx);   
            AllDataSave();            
        }

        private void OnBtnDataLoad(object sender, EventArgs e)
        {
            dictUserInfo.Clear();
            listRequestOrder.Clear();
            listReqMusicInfo.Clear();

            Dictionary<int, UserInfo> users = BinarySave.Deserialize<Dictionary<int, UserInfo>>("DataSave\\UserInfo.bin");
            List<RequestOrder> orders = BinarySave.Deserialize<List<RequestOrder>>("DataSave\\RequestOrder.bin");
            List<RequestMusicInfo> musics = BinarySave.Deserialize<List<RequestMusicInfo>>("DataSave\\RequestMusic.bin");

            foreach(KeyValuePair<int, UserInfo> pair in users)            
                dictUserInfo.Add(pair.Key, pair.Value);            

            for (int i = 0; i < orders.Count; i++)
            {
                RequestOrder loadOrder = orders[i];
                RequestOrder setOrder = new RequestOrder(loadOrder.type, loadOrder.id, loadOrder.tableNo, loadOrder.packing);
                
                listRequestOrder.Add(setOrder);
                orderID = loadOrder.id;
            }

            for (int i = 0; i < musics.Count; i++)
            {
                RequestMusicInfo loadMusic = musics[i];
                RequestMusicInfo setMusic = new RequestMusicInfo(loadMusic.id, loadMusic.tableNo, loadMusic.title, loadMusic.singer);

                listReqMusicInfo.Add(setMusic);
                musicID = loadMusic.id;
            }
        }

        public void DataUserInfoSave()
        {
            if (Directory.Exists("DataSave") == false)
                Directory.CreateDirectory("DataSave");

            BinarySave.Serialize(dictUserInfo, "DataSave\\UserInfo.bin");
        }        

        public void DataRequestSave(bool isOrder)
        {
            if (Directory.Exists("DataSave") == false)
                Directory.CreateDirectory("DataSave");

            if (isOrder)    BinarySave.Serialize(listRequestOrder, "DataSave\\RequestOrder.bin");
            else            BinarySave.Serialize(listReqMusicInfo, "DataSave\\RequestMusic.bin");
        }        

        public void AllDataSave()
        {
            DataUserInfoSave();
            DataRequestSave(true);
            DataRequestSave(false);
        }
    }
}
