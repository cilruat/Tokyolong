using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotMgr : MonoBehaviour {

	static public int idStart = 0;
	static public int numRobots = 50;

	public Text infomation;
	public Text injected;
	public Text average;

	List<RobotNetwork> robot = new List<RobotNetwork>();

	string ip = "";
	string port = "";

	void Awake()
	{
		string path = Application.dataPath;
		int lastIdx = path.LastIndexOf(@"/");
		path = path.Substring(0, lastIdx) + @"\Info\ServerInfo.txt";
		string server = System.IO.File.ReadAllText(path);

		if (string.IsNullOrEmpty (server) == false) {
			string[] LINE_SPLIT_RE = { "\r\n", "\n\r", "\n", "\r" };
			string[] serverInfo = server.Split(LINE_SPLIT_RE, System.StringSplitOptions.RemoveEmptyEntries);
			ip = serverInfo [0];
			port = serverInfo [1];
		}
	}

	void Start()
	{
		Application.targetFrameRate = 120;
		Application.runInBackground = true;

		RobotNetwork.InitGlobal (ip, port, (float)numRobots * .1f);

		for (int i = 0; i < numRobots; i++) {
			RobotNetwork network = gameObject.AddComponent<RobotNetwork> ();
			network.Init (idStart + i);
			robot.Add (network);
		}

		infomation.text = "ROBOT #" + idStart.ToString () + " ~ #" + (idStart + numRobots - 1).ToString ();
	}		

	void Update()
	{
		string str = "INJECTED " + RobotNetwork.numLogined.ToString () + "/" + numRobots.ToString ();
		if (injected.text != str)
			injected.text = str; 

		int ms = (int)(RobotNetwork.accRecvTime * 1000f);
		str = "AVERAGE TIME " + ms.ToString () + "ms";
		if (average.text != str)
			average.text = str;

		float rate = Mathf.Clamp (RobotNetwork.accRecvTime * 2f, 0, 1f);
		average.color = Color.Lerp (Color.green, Color.red, rate);
	}

	void OnDestroy()
	{
		foreach (RobotNetwork r in robot)
			r.disconnect ();
	}
}