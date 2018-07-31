using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FreeNet;
using FreeNetUnity;

/*public interface IMessageReceiver
{
    void on_recv(CPacket msg);
}*/

public partial class RobotNetwork : MonoBehaviour
{
    Queue<CPacket> sending_queue;
    CFreeNetUnityService freenet;

	static public string IP = "";
	static public string PORT = "";

	public void Init(int id)
	{
		idRobot = id;
		Info.listGamePlayCnt_Robot.Add (0);	
	}

	void Awake()
	{
		Application.runInBackground = true;

		this.freenet = gameObject.AddComponent<CFreeNetUnityService>();
		this.freenet.appcallback_on_message += this.on_message;
		this.freenet.appcallback_on_status_changed += this.on_status_changed;
		this.sending_queue = new Queue<CPacket>();

		connect (IP, PORT);
	}

    public void connect(string ip, string port)
    {
        // 이전에 보내지 못한 패킷은 모두 버린다.
        this.sending_queue.Clear();

        if (!this.freenet.is_connected())
        {
            this.freenet.connect(ip, int.Parse(port));
        }
    }

    public void disconnect()
    {
        if (is_connected())
        {
            this.freenet.disconnect();
            return;
        }			      
    }		   

    void on_status_changed(NETWORK_EVENT status)
    {
        switch (status)
        {
		case NETWORK_EVENT.connected:			
			Login_REQ ();
			break;
		case NETWORK_EVENT.disconnected:
			Debug.Log ("disconnected network robot");
            break;
        }
    }		   

    void Update()
    {
        if (!this.freenet.is_connected())
            return;

        while (this.sending_queue.Count > 0)
        {
            CPacket msg = this.sending_queue.Dequeue();
            this.freenet.send(msg);
        }

		_UpdateAI ();
    }

    public bool is_connected()
    {
        if (this.freenet == null)
            return false;

        return this.freenet.is_connected();
    }
}
