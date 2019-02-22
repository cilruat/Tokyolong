using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class AdminSettingDiscountProb : SingletonMonobehaviour<AdminSettingDiscountProb> 
{

    public List<InputField> listProbs = new List<InputField>();

    public Text textTotalProb;

    public void ShowDiscountProb(List<float> list)
    {
        gameObject.SetActive(true);

        float totalProb = 0f;
        for (int i = 0; i < listProbs.Count; i++)
        {
            listProbs[i].text = (list[i] * 100f).ToString("N2");
            totalProb += list[i];
        }

        textTotalProb.text = (totalProb * 100f).ToString("N2");
    }

    public void OnValueChanged()
    {
        float calcProb = 0f;
        for (int i = 0; i < listProbs.Count; i++)
        {
            float val = 0;
            if (float.TryParse(listProbs[i].text, out val) == false)
                calcProb += 0f;
            else
                calcProb += val;
        }

        textTotalProb.text = calcProb.ToString("N2");
    }

    public void OnSettingDiscount()
    {
        List<float> list = new List<float>();
        for (int i = 0; i < listProbs.Count; i++)
        {
            float val = 0f;
            if (float.TryParse(listProbs[i].text, out val) == false)
                val = 0f;
            else
                val = (val * 0.01f);

            list.Add(val);
        }

        float n100Per = 0f;
        if (float.TryParse(textTotalProb.text, out n100Per) == false)
            n100Per = 0f;

        if (n100Per != 100f)
        {
            SystemMessage.Instance.Add("100%를 맞춰 주세요");
            return;
        }
        
        NetworkManager.Instance.SetDiscountProb_REQ(list);
        OnClose();
    }

	public void OnClose() {	gameObject.SetActive (false); }
}
