using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingEffectGeneral : MonoBehaviour {

    public Text tx;
    public string m_text = "";
    public float TimeToWait;
    public float stringInterval;

    void Start()
    {
        StartCoroutine(_typing());
    }



    IEnumerator _typing()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeToWait);
            for (int i = 0; i < m_text.Length; i++)
            {
                tx.text = m_text.Substring(0, i);

                yield return new WaitForSeconds(stringInterval);

            }
        }
    }

}
