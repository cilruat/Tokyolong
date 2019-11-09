using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestCode : MonoBehaviour {

	void Start () {
        string path = Application.dataPath + @"\TokyoLive_QuestionBook.csv";
        List<Dictionary<string, object>> data = CSVReader.Read(path);

        for (int i = 0; i < data.Count; i++)
        {
            foreach (KeyValuePair<string, object> pair in data[i])
                Debug.Log(pair.Key + ": " + pair.Value);
        }
	}		
}