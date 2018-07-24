using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public partial class PageGame : PageBase {

	public UnfinishGameList unfinishGame;

	public void RefreshUnfinishList(string packing) { unfinishGame.SetInfo (packing); }
	public void RemoveUnfinishGame(int id) { unfinishGame.RemoveUnfinish (id); }
}
