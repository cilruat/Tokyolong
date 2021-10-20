using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceData {

    public string qeustionName;
    public string[] questAnswers;


    public ChoiceData(string name, string[] answer)
    {
        qeustionName = name;
        questAnswers = answer;
    }
}
