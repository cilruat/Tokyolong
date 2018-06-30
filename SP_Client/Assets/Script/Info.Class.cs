using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo
{
    public byte tableNo = 0;
    public byte peopleCnt = 0;
    public byte customerType = 0;
}
    
public partial class Info : MonoBehaviour 
{
    public Dictionary<byte, UserInfo> dictUsers = new Dictionary<byte, UserInfo>();
}
