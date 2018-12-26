using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FreeNet;
using SP_Server;

public partial class RobotNetwork : MonoBehaviour
{
	void send(CPacket msg)
	{
        #if UNITY_EDITOR
        Debug.Log(((PROTOCOL)msg.protocol_id).ToString());
        #endif

		this.sending_queue.Enqueue(msg);
	}

	public void Login_REQ()
	{
		CPacket msg = CPacket.create ((short)PROTOCOL.LOGIN_REQ);
		msg.push (idRobot.ToString ());

		send (msg);
	}

	public void Logout_REQ()
	{
		CPacket msg = CPacket.create ((short)PROTOCOL.LOGOUT_REQ);
		msg.push ((byte)idRobot);

		send (msg);
	}

	public void EnterCostomer_REQ(byte tableNo)
	{
        CPacket msg = CPacket.create((short)PROTOCOL.ENTER_CUSTOMER_REQ);
		msg.push (tableNo);
		msg.push((byte)Random.Range(1,8));
		msg.push((byte)Random.Range(0, 3));

        send(msg);
	}

	public void WaiterCall_REQ ()
	{
		CPacket msg = CPacket.create((short)PROTOCOL.WAITER_CALL_REQ);
		msg.push (Info.TableNum);

		send (msg);
	}

    public void Order_REQ()
	{
        List<SendMenu> list = new List<SendMenu>();
        for (int i = 0; i < 10; i++)
        {
            int menu = Mathf.Max(1, i+1);
            int cnt = 1;
            SendMenu sendMenu = new SendMenu(menu, cnt);
            list.Add(sendMenu);
        }

        LitJson.JsonData json = LitJson.JsonMapper.ToJson(list);

        CPacket msg = CPacket.create((short)PROTOCOL.ORDER_REQ);
        msg.push ((byte)idRobot);
        msg.push(json.ToString());
        msg.push(10);

		send (msg);
	}

    public void Chat_REQ()
    {
        CPacket msg = CPacket.create((short)PROTOCOL.CHAT_REQ);
		msg.push ((byte)idRobot);
		msg.push((byte)Random.Range(0, 21));
        msg.push("Hello!");

        send(msg);
    }

    public void Order_Detail_REQ()
    {
        CPacket msg = CPacket.create((short)PROTOCOL.ORDER_DETAIL_REQ);
		msg.push ((byte)idRobot);

        send(msg);
    }

	public void Game_Discount_REQ()
	{
		CPacket msg = CPacket.create((short)PROTOCOL.GAME_DISCOUNT_REQ);
		msg.push ((byte)idRobot);
		msg.push ((short)Random.Range(0, 2));
		send (msg);
	}

	public void Request_Music_List_REQ()
	{
		CPacket msg = CPacket.create ((short)PROTOCOL.REQUEST_MUSIC_LIST_REQ);
		send (msg);
	}

    public void Request_Music_REQ()
    {
        CPacket msg = CPacket.create((short)PROTOCOL.REQUEST_MUSIC_REQ);
        msg.push((byte)idRobot);
        msg.push("눈의꽃_" + idRobot.ToString());
        msg.push("박효신_" + idRobot.ToString());

        send(msg);
    }
		   
	public void SlotStart_REQ()
	{
		CPacket msg = CPacket.create((short)PROTOCOL.SLOT_START_REQ);
		msg.push ((byte)idRobot);
		send (msg);
	}		

    public void TableDiscountInput_REQ()
    {
        CPacket msg = CPacket.create((short)PROTOCOL.TABLE_DISCOUNT_INPUT_REQ);
        msg.push ((byte)idRobot);
        msg.push (1000);

        send(msg);
    }

    public void GetDiscountProb_REQ()
    {
        CPacket msg = CPacket.create((short)PROTOCOL.GET_RANDOM_DISCOUNT_PROB_REQ);
        send(msg);
    }

    public void SetDiscountProb_REQ()
    {
        CPacket msg = CPacket.create((short)PROTOCOL.SET_RANDOM_DISCOUNT_PROB_REQ);
        List<float> list = new List<float>();
        list.Add(.25f);
        list.Add(.25f);
        list.Add(.25f);
        list.Add(.25f);

        msg.push(list[0]);
        msg.push(list[1]);
        msg.push(list[2]);
        msg.push(list[3]);
        send(msg);
    }
}
