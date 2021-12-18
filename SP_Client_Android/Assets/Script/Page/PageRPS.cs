using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageRPS : SingletonMonobehaviour<PageRPS>  {


    public GameObject objBoard;



    public void ReturnHome()
    {
        SceneChanger.LoadScene("Mail", PageBase.Instance.curBoardObj());
        //REQ 보내야하넹...ㅎㅎ 이사람이 나갔으니까 상대방이 나가서 취소됬다고 애니메이션 넣기..심플하게
    }






}
