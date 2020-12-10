﻿using UnityEngine;
using System.Collections;

/// <summary>
// Player Meta Class
/// </summary>
public class PlayerMeta {
	private int _gold = 1000;

	public int Gold
    {
		get { return this._gold; }
        set { this._gold = value; }
    }
	
	//static variable
	private static PlayerMeta _instance = null;
	//static functions
	public static PlayerMeta Instance(){
		if(_instance == null){
			_instance = new PlayerMeta();
            _instance.Gold = Info.GamePlayCnt;
		}
		return _instance;
	}

	// get more gold!
	public static void incraseGold(int g){
        // loading 화면 켜기
        // working source

        NetworkManager.Instance.GameCountInput_REQ(Info.TableNum, g);

		//Instance().Gold += g;
	}

	// lose gold!
	public static void decreaseGold(int g){
        // loading 화면 켜기
        // working source
        NetworkManager.Instance.GameCountInput_REQ(Info.TableNum, -g);
        //Instance().Gold -= g;
    }

    public static void RefreshGold(int g)
    {
        Instance().Gold = Info.GamePlayCnt;
    }

	// now gold
	public static int GetGold(){
		return Instance().Gold;
	}
}
