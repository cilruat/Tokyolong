using UnityEngine;
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
		}
		return _instance;
	}

	// get more gold!
	public static void incraseGold(int g){
		Instance().Gold += g;
	}

	// lose gold!
	public static void decreaseGold(int g){
		Instance().Gold -= g;
	}

	// now gold
	public static int GetGold(){
		return Instance().Gold;
	}
}
