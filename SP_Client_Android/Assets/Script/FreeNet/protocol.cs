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
		FAILED,
        FAILED_DB,
		FAILED_NOT_NUMBER,
        FAILED_ALREADY_SEND_LIKE,

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
        CHAT_REQ,
        CHAT_ACK,
        CHAT_NOT,
        ORDER_DETAIL_REQ,
        ORDER_DETAIL_ACK,
		GAME_DISCOUNT_REQ,
		GAME_DISCOUNT_ACK,
		REQUEST_MUSIC_LIST_REQ,
		REQUEST_MUSIC_LIST_ACK,
        REQUEST_MUSIC_REQ,
        REQUEST_MUSIC_ACK,
        REQUEST_MUSIC_NOT,
        REQUEST_MUSIC_REMOVE_REQ,
        REQUEST_MUSIC_REMOVE_ACK,
        REQUEST_MUSIC_REMOVE_NOT,
        ORDER_CONFIRM_REQ,
        ORDER_CONFIRM_ACK,
        ORDER_CONFIRM_NOT,
        TABLE_ORDER_CONFIRM_REQ,
        TABLE_ORDER_CONFIRM_ACK,
        TABLE_ORDER_INPUT_REQ,
        TABLE_ORDER_INPUT_ACK,
        TABLE_ORDER_INPUT_NOT,
		SLOT_START_REQ,
		SLOT_START_ACK,
        TABLE_DISCOUNT_INPUT_REQ,
        TABLE_DISCOUNT_INPUT_ACK,
        TABLE_DISCOUNT_INPUT_NOT,
		GET_RANDOM_DISCOUNT_PROB_REQ,
		GET_RANDOM_DISCOUNT_PROB_ACK,
		SET_RANDOM_DISCOUNT_PROB_REQ,
		SET_RANDOM_DISCOUNT_PROB_ACK,
        TABLE_PRICE_CONFIRM_REQ,
        TABLE_PRICE_CONFIRM_ACK,
		GAME_COUNT_INPUT_REQ,
		GAME_COUNT_INPUT_ACK,
		GAME_COUNT_INPUT_NOT,
		TABLE_MOVE_REQ,
		TABLE_MOVE_ACK,
		TABLE_MOVE_NOT,
		OWNER_GAME_NOT,

        //-------------------------------------
        // 우편함 프로토콜 
        //-------------------------------------
        MSG_SEND_REQ,
        MSG_SEND_ACK,
        MSG_SEND_NOT,
        LKE_SEND_REQ,
        LKE_SEND_ACK,
        LKE_SEND_NOT,
        PRESENT_SEND_REQ,
        PRESENT_SEND_ACK,
        PRESENT_SEND_NOT,
        PLZ_SEND_REQ,
        PLZ_SEND_ACK,
        PLZ_SEND_NOT,

        //-------------------------------------
        // 캐쉬 프로토콜.
        //-------------------------------------        
        CASH_SEND_REQ,
        CASH_SEND_ACK,
        CASH_SEND_NOT,

        //-------------------------------------
        // 게임 프로토콜.
        //-------------------------------------

        GAME_VERSUS_INVITE_REQ,
        GAME_VERSUS_INVITE_ACK,
        GAME_VERSUS_INVITE_NOT,

        GAME_REFUSE_REQ,
        GAME_REFUSE_ACK,
        GAME_REFUSE_NOT,

        GAME_ACCEPT_REQ,
        GAME_ACCEPT_ACK,
        GAME_ACCEPT_NOT,

        ROOM_REMOVED,

        END,
    }        
}
