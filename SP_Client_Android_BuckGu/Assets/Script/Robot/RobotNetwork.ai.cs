using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SP_Server;

public partial class RobotNetwork : MonoBehaviour {

	public static int numLogined = 0;
	public static float timeToAllLogin = 600f;
	public static float accRecvTime = 0;

	public struct JBehaviorProperty
	{
		float minWait;
		float maxWait;

		public JBehaviorProperty( float min, float max )
		{
			minWait = min;
			maxWait = max;
		}			

		public float GetTimeToWait()
		{
			return Time.time + Random.Range ( minWait, maxWait );
		}

		public BH GetNextBehavior()
		{
			return listEnable [Random.Range (0, listEnable.Count)];
		}

		public static BH GlobalNextBehavior()
		{
			return listEnable [Random.Range (0, listEnable.Count)];
		}
	}

	static Dictionary<BH, JBehaviorProperty> restTime = new Dictionary<BH, JBehaviorProperty>();
	static List<BH> listEnable = new List<BH>();

	static void build_ai()
	{		
		restTime.Add ( BH.Login,			new JBehaviorProperty( 4f, 8f ) );

		for( BH bh = BH.Order; bh < BH.SetRandomDiscountProb; bh++ )
		{			
			restTime.Add( bh, new JBehaviorProperty( 4f, 8f ) );
			listEnable.Add( bh );
		}
	}

	int idRobot = -1;
	bool waiting = false;
	float timeToReq = 0;
	float timeToWait = -1f;

	BH nextBehavior = BH.Login;

	public static void InitGlobal(string ip, string port, float timeToAll)
	{
		IP = ip;
		PORT = port;
		timeToAllLogin = timeToAll;
		build_ai ();
	}

	void _UpdateAI()
	{
		if (idRobot < 0)
			return;

		if (timeToWait < 0)
			timeToWait = Time.time + Random.Range (1f, timeToAllLogin);

		if (waiting)
			return;

		if (Time.time < timeToWait)
			return;

		BH nowBehavior = nextBehavior;
		nextBehavior = restTime [nowBehavior].GetNextBehavior ();

		JBehaviorProperty property;
		if (restTime.TryGetValue (nowBehavior, out property) == false) {
			Debug.LogError ("Can not found " + nextBehavior.ToString () + " in dictionary");
			idRobot = -1;
			return;
		}

		timeToWait = property.GetTimeToWait ();
		waiting = true;
		timeToReq = Time.time;

		if (waiting == false) {
			timeToWait = Time.time;
			nextBehavior = JBehaviorProperty.GlobalNextBehavior ();
			Debug.Log( idRobot.ToString () + " : Skip " + nowBehavior.ToString () + " to " + nextBehavior.ToString () );
		}

		Info.idRobot = idRobot - RobotMgr.idStart;
		switch (nowBehavior) {
		case BH.Login:				Login_REQ ();				break;
		case BH.Order:				Order_REQ ();				break;
		case BH.OrderDetail:		Order_Detail_REQ ();		break;
		case BH.Chat:				Chat_REQ ();				break;
		case BH.GameDiscount:		Game_Discount_REQ ();		break;
		case BH.RequestMusicList:	Request_Music_List_REQ ();	break;
		case BH.RequestMusic:		Request_Music_REQ ();		break;
		case BH.SlotStart:			SlotStart_REQ ();			break;
        case BH.TableDiscountInput:     TableDiscountInput_REQ ();  break;
		case BH.GetRandomDiscountProb:  GetDiscountProb_REQ ();     break;
		case BH.SetRandomDiscountProb:	SetDiscountProb_REQ ();		break;
		default:
			break;
		}
	}

	public enum BH
	{
		Login = 0,
		EnterCustomer,

		Order,
		OrderDetail,
		Chat,
		GameDiscount,
		RequestMusicList,
		RequestMusic,
		SlotStart,
        TableDiscountInput,
        GetRandomDiscountProb,
        SetRandomDiscountProb,
		Max,
	}
}
