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
        public Dictionary<int, List<SendMenu>> dictUserMenu = new Dictionary<int, List<SendMenu>>();
        public List<RequestMusic> listRequestMusic = new List<RequestMusic>();

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

            WriteLog("[connect cnt: {0}] any client Connected!!", get_concurrent_user_count().ToString());
        }

        public void remove_user(User user)
        {
            lock(ListUser)
            {
                ListUser.Remove(user);
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
                    WriteLog("[" + GetAdminUser().tableNum + "]" + GetAdminUser().tableNum.ToString());
                    for (int i = 0; i < ListUser.Count; i++)
                        WriteLog("[" + ListUser[i].tableNum + "]" + ListUser[i].tableNum.ToString());
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

            this.listviewLog.Items.Add(listViewItem);
            
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
    }
}
