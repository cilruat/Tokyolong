using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo
{
    public int tableNo = -1;
    public byte peopleCnt = 0;
    public byte customerType = 0;
}


public partial class Info : MonoBehaviour 
{
    List<UserInfo> listUser = new List<UserInfo>();
}
