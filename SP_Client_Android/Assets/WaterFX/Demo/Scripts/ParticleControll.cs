using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleControll : MonoBehaviour {


    public ParticleSystem objFireWork1_0;
    public ParticleSystem objFireWork1_1;
    public ParticleSystem objFireWork1_2;
    public ParticleSystem objFireWork1_3;
    public ParticleSystem objFireWork1_4;
    public ParticleSystem objFireWork1_5;
    public ParticleSystem objFireWork1_6;
    public ParticleSystem objFireWork1_7;
    public ParticleSystem objFireWork1_8;
    public ParticleSystem objFireWork1_9;




    // 뒷자리가 후딜
    private void Start()
    {

        InvokeRepeating("objFireWorkTest", 5f, 20f);
        InvokeRepeating("objFireWorkTest2", 5f, 20f);
        InvokeRepeating("objFireWorkTest3", 5f, 20f);
        InvokeRepeating("objFireWorkTest4", 5f, 20f);
        InvokeRepeating("objFireWorkTest5", 5f, 20f);
        InvokeRepeating("objFireWorkTest6", 5f, 20f);
        InvokeRepeating("objFireWorkTest7", 5f, 20f);
        InvokeRepeating("objFireWorkTest8", 5f, 20f);
        InvokeRepeating("objFireWorkTest9", 5f, 20f);
        InvokeRepeating("objFireWorkTest10", 5f, 20f);
    }


    public void objFireWorkTest()
    {
        objFireWork1_0.Play(true);
    }

    public void objFireWorkTest2()
    {
        objFireWork1_1.Play(true);
    }

    public void objFireWorkTest3()
    {
        objFireWork1_2.Play(true);
    }

    public void objFireWorkTest4()
    {
        objFireWork1_3.Play(true);
    }

    public void objFireWorkTest5()
    {
        objFireWork1_4.Play(true);
    }

    public void objFireWorkTest6()
    {
        objFireWork1_5.Play(true);
    }

    public void objFireWorkTest7()
    {
        objFireWork1_6.Play(true);
    }

    public void objFireWorkTest8()
    {
        objFireWork1_7.Play(true);
    }
    public void objFireWorkTest9()
    {
        objFireWork1_8.Play(true);
    }

    public void objFireWorkTest10()
    {
        objFireWork1_9.Play(true);
    }



}
