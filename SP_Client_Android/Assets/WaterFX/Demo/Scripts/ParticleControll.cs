using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleControll : MonoBehaviour {


    public ParticleSystem objFireWork;


    private void Start()
    {

        InvokeRepeating("objFireWorkTest", 0, 2f);


    }


    public void objFireWorkTest()
    {
        objFireWork.Play(true);
    }





}
