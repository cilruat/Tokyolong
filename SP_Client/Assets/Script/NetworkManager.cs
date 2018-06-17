using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FreeNet;
using FreeNetUnity;

public interface IMessageReceiver
{
    void on_recv(CPacket msg);
}

public partial class NetworkManager : SingletonMonobehaviour<NetworkManager>
{
    [SerializeField]
    string server_ip = "";

    [SerializeField]
    string server_port = "";

    Queue<CPacket> sending_queue;
    CFreeNetUnityService freenet;
	public IMessageReceiver message_receiver;

	void Awake()
	{
        this.freenet = gameObject.AddComponent<CFreeNetUnityService>();
        this.freenet.appcallback_on_message += this.on_message;
        this.freenet.appcallback_on_status_changed += this.on_status_changed;

        this.sending_queue = new Queue<CPacket>();

		Application.runInBackground = true;
        DontDestroyOnLoad(this);
	}

    public void connect()
    {
        // 이전에 보내지 못한 패킷은 모두 버린다.
        this.sending_queue.Clear();

        if (!this.freenet.is_connected())
        {
            this.freenet.connect(this.server_ip, int.Parse(this.server_port));
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
			((PageLogin)PageBase.Instance).SuccessConnect ();
			break;
        case NETWORK_EVENT.disconnected:
            break;
        }
    }		   

    void Update()
    {
        if (!this.freenet.is_connected())
        {
            return;
        }

        while (this.sending_queue.Count > 0)
        {
            CPacket msg = this.sending_queue.Dequeue();
            this.freenet.send(msg);
        }
    }

    public bool is_connected()
    {
        if (this.freenet == null)
        {
            return false;
        }

        return this.freenet.is_connected();
    }
}
