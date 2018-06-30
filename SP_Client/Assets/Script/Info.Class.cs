using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class UserInfo
{
    public byte tableNo = 0;
    public byte peopleCnt = 0;
    public byte customerType = 0;

	public ECustomerType eCustomerType { get { return (ECustomerType)customerType; } }

	public UserInfo (byte tableNo, byte peopleCnt, byte customerType)
	{
		this.tableNo = tableNo;
		this.peopleCnt = peopleCnt;
		this.customerType = customerType;
	}
}
    
public partial class Info : MonoBehaviour 
{
	static Dictionary<byte, UserInfo> dictUsers = new Dictionary<byte, UserInfo>(); // key : TableNo, value : userInfo

    public static void AddOtherLoginUser(string packing)
    {
        JsonData json = JsonMapper.ToObject (packing);
        string json1 = json["tableNum"].ToString ();
        string json2 = json["peopleCnt"].ToString ();
        string json3 = json["customerType"].ToString ();   

        byte tableNo = byte.Parse(json1);
        byte peopleCnt = byte.Parse(json2);
        byte customerType = byte.Parse(json3);

        if (dictUsers.ContainsKey (tableNo) == false)
            dictUsers.Add (tableNo, new UserInfo (tableNo, peopleCnt, customerType));
        else
            dictUsers [tableNo] = new UserInfo (tableNo, peopleCnt, customerType);
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

			if (dictUsers.ContainsKey (tableNo) == false)
				dictUsers.Add (tableNo, new UserInfo (tableNo, peopleCnt, customerType));
			else
				dictUsers [tableNo] = new UserInfo (tableNo, peopleCnt, customerType);
		}
	}

	public static UserInfo GetUser(byte tableNo)
	{
		if (dictUsers.ContainsKey (tableNo))
			return dictUsers [tableNo];
		else
			return null;
	}
}
