using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleControll : MonoBehaviour {


    public ParticleSystem objFireWork;

    public ParticleSystem objFireWork1;


    // 뒷자리가 후딜
    private void Start()
    {

        InvokeRepeating("objFireWorkTest", 1f, 2f);

        InvokeRepeating("objFireWorkTest2", 5, 20f);

    }


    public void objFireWorkTest()
    {
        objFireWork.Play(true);
    }

    public void objFireWorkTest2()
    {
        objFireWork1.Play(true);
    }




}
