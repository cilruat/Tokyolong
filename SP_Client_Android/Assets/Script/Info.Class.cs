using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class UserInfo
{
    public byte tableNo = 0;
    public byte peopleCnt = 0;
    public byte customerType = 0;    

    public List<UserMsgInfo> listMsgInfo = new List<UserMsgInfo>();
    public List<UserLikeInfo> listLikeInfo = new List<UserLikeInfo>();
    public List<UserPresentInfo> listPresentInfo = new List<UserPresentInfo>();
    public List<UserPlzInfo> listPlzInfo = new List<UserPlzInfo>();
    public List<UserCashInfo> listCashInfo = new List<UserCashInfo>();
    public List<UserGameInfo> listGameInfo = new List<UserGameInfo>();
    public List<UserRefuseInfo> listRefuseInfo = new List<UserRefuseInfo>();
    public List<UserGameAcceptInfo> listGameAcceptInfo = new List<UserGameAcceptInfo>();
    public List<UserCancelInfo> listCanCelInfo = new List<UserCancelInfo>();
    public List<UserOnReadyInfo> listReadyAddInfo = new List<UserOnReadyInfo>();

    public List<UserBullDogFirstInfo> listBullDogFirstInfo = new List<UserBullDogFirstInfo>();
    public List<UserBullDogValueInfo> listBullDogValueInfo = new List<UserBullDogValueInfo>();



    public ECustomerType eCustomerType { get { return (ECustomerType)customerType; } }

	public UserInfo (byte tableNo, byte peopleCnt, byte customerType)
	{
		this.tableNo = tableNo;
		this.peopleCnt = peopleCnt;
		this.customerType = customerType;
	}
}

public class UserChatInfo
{
    public List<UserChat> listChat = new List<UserChat>();
    public bool isNew = false;

    public void AddChat(UserChat chat) { listChat.Add(chat); }
}

public class UserChat
{
    public UserInfo info;
    public byte person;
    public string time;
    public string chat;

    public UserChat(UserInfo info, byte person, string time, string chat)
    {
        this.info = info;
        this.person = person;
        this.time = time;
        this.chat = chat;
    }
}

public class UserMsgInfo
{
    public byte tableNo = 0;
    public string strMsg = "";
}

public class UserLikeInfo
{
    public byte tableNo = 0;
}

public class UserPresentInfo
{
    public byte tableNo = 0;
    public int presentCount = 0;
}

public class UserPlzInfo
{
    public byte tableNo = 0;
    public int plzCount = 0;
}

public class UserCashInfo
{
    //필요한 정보는 몇번테이블인지, 무슨아이템을 삿는지
    public byte tableNo = 0;
    public string reqCashItem = "";
}

public class UserGameInfo
{
    public byte tableNo = 0;
    public int reqGameCnt = 0;
    public string gameName = "";

}

public class UserGameAcceptInfo
{
    public byte tableNo = 0;
    public int reqGameCnt = 0;
    public string gameName = "";

}


public class UserRefuseInfo
{
    public byte tableNo = 0;
}

public class UserCancelInfo
{
    public byte tableNo = 0;
}


public class UserOnReadyInfo
{
    public byte tableNo = 0;
}

public class UserBullDogFirstInfo
{
    public int firstValue = 0;
}


public class UserBullDogValueInfo
{
    public int Randomvalue = 0;
}




public partial class Info : MonoBehaviour 
{
    public static UserInfo myInfo = null;
	public static Dictionary<byte, UserInfo> dictUserInfo = new Dictionary<byte, UserInfo>(); // key : TableNo, value : UserInfo
    public static Dictionary<byte, UserChatInfo> dictUserChatInfo = new Dictionary<byte, UserChatInfo>(); // key : TableNo, value : UserChatInfo

    public static string adminTablePacking = "";
    public static string adminOrderPacking = "";
    public static string adminMusicPacking = "";

    public static List<int> listLoginTable = new List<int>();

    public static void Init()
    {
        TableNum = 0;
        PersonCnt = 0;
        ECustomer = ECustomerType.MAN;
        GamePlayCnt = 0;
        GameDiscountWon = -1;
        GameCoinNum = -1;

		firstOrder = false;
        isQuiz = false;
        isSlot = false;
        isYabawi = false;

        SlotWin = false;
        YabawiWin = false;
        QuizWin = false;

        SlotLose = false;
        YabawiLose = false;
        QuizLose = false;


        myInfo = null;
        dictUserInfo.Clear();
        dictUserChatInfo.Clear();
        listLoginTable.Clear();
    }

    // About User
    public static UserInfo GetUser(byte tableNo)
    {
        if (dictUserInfo.ContainsKey (tableNo))
            return dictUserInfo [tableNo];
        else
            return null;
    }

    public static void AddOtherLoginUser(string packing)
    {
        JsonData json = JsonMapper.ToObject (packing);
        byte tableNo = byte.Parse(json["tableNum"].ToString ());
        byte peopleCnt = byte.Parse(json["peopleCnt"].ToString ());
        byte customerType = byte.Parse(json["customerType"].ToString ());

        if (dictUserInfo.ContainsKey (tableNo) == false)
            dictUserInfo.Add (tableNo, new UserInfo (tableNo, peopleCnt, customerType));
        else
            dictUserInfo [tableNo] = new UserInfo (tableNo, peopleCnt, customerType);
    }

	public static void SetLoginedOtherUser(string packing)
	{
		JsonData json = JsonMapper.ToObject (packing);
		for (int i = 0; i < json.Count; i++)    
		{
            string json1 = json [i] ["tableNum"].ToString ();
            string json2 = json [i] ["peopleCnt"].ToString ();
            string json3 = json [i] ["customerType"].ToString ();	

            byte tableNo = byte.Parse(json1);
            byte peopleCnt = byte.Parse(json2);
            byte customerType = byte.Parse(json3);

            if (dictUserInfo.ContainsKey (tableNo) == false)
                dictUserInfo.Add (tableNo, new UserInfo (tableNo, peopleCnt, customerType));
			else
                dictUserInfo [tableNo] = new UserInfo (tableNo, peopleCnt, customerType);

            if (Info.TableNum == tableNo)
                Info.myInfo = dictUserInfo[tableNo];
		}
	}

    public static void SetLogoutOtherUser(byte tableNo)
    {
        if (dictUserInfo.ContainsKey(tableNo) == false)
            return;

        dictUserInfo.Remove(tableNo);

        RemoveUserChatInfo(tableNo);

        GameObject objUIChat = UIManager.Instance.GetUI(eUI.eChat);
        UIChat uiChat = objUIChat.GetComponent<UIChat>();
        if (uiChat == null)
        {
            Debug.Log("UIChat Null..");
            return;
        }

        uiChat.RemoveChatTableElt(tableNo);
    }

    public static void RemoveUserChatInfo(byte tableNo)
    {
        if (dictUserChatInfo.ContainsKey(tableNo) == false)
            return;

        dictUserChatInfo.Remove(tableNo);
    }

    // About Chat..
    public static UserChatInfo GetUserChat(byte tableNo)
    {
        if (dictUserChatInfo.ContainsKey(tableNo))
            return dictUserChatInfo[tableNo];
        else
            return null;
    }

    public static void AddUserChatInfo(byte tableNo, UserChat chat, bool isNew)
    {
        if (dictUserChatInfo.ContainsKey(tableNo) == false)
            dictUserChatInfo.Add(tableNo, new UserChatInfo());

        if (chat == null)
            return;

        UserChatInfo chatInfo = dictUserChatInfo[tableNo];
        chatInfo.isNew = isNew;
        chatInfo.AddChat(chat);
    }
}
