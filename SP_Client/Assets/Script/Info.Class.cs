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

	public static void SetOtherUser(string packing)
	{
		JsonData json = JsonMapper.ToObject (packing);
		for (int i = 0; i < json.Count; i++) 
		{
			byte tableNo  = byte.Parse(json [i] ["tableNum"].ToString ());
			byte peopleCnt = byte.Parse(json [i] ["peopleCnt"].ToString ());
			byte customerType = byte.Parse (json [i] ["customerType"].ToString ());	

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
