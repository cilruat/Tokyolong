using System;

namespace SP_Server
{
    /// <summary>
    /// 프로토콜 정의.
    /// 서버에서 클라이언트로 가는 패킷 : S -> C
    /// 클라이언트에서 서버로 가는 패킷 : C -> S
    /// </summary>
    public enum PROTOCOL : short
    {
        //-------------------------------------
        // 0 이하는 종료코드로 사용되므로 게임에서 쓰지 말것!!
        //-------------------------------------
        BEGIN = 0,

		LOGIN_REQ,
		LOGIN_ACK,
		LOGIN_NOT,
		ENTER_CUSTOMER_REQ,
		ENTER_CUSTOMER_ACK,
		ENTER_CUSTOMER_NOT,

		//-------------------------------------
		// 실패 프로토콜.
		//-------------------------------------
		FAILED_NOT_NUMBER,

        //-------------------------------------
        // 로비 프로토콜.
        //-------------------------------------        
		LOGOUT_REQ,
		LOGOUT_ACK,
		LOGOUT_NOT,
		WAITER_CALL_REQ,
		WAITER_CALL_ACK,
		WAITER_CALL_NOT,
		ORDER_REQ,
		ORDER_ACK,
		ORDER_NOT,
		REQUEST_MUSIC_REQ,
		REQUEST_MUSIC_ACK,
		REQUEST_MUSIC_NOT,
        CHAT_REQ,
        CHAT_ACK,
        CHAT_NOT,
        ORDER_DETAIL_REQ,
        ORDER_DETAIL_ACK,
		GAME_DISCOUNT_REQ,
		GAME_DISCOUNT_ACK,
		GAME_DISCOUNT_NOT,
		REQUEST_MUSIC_LIST_REQ,
		REQUEST_MUSIC_LIST_ACK,
        //-------------------------------------
        // 게임 프로토콜.
        //-------------------------------------        
        ROOM_REMOVED,

        END,
    }        
}
