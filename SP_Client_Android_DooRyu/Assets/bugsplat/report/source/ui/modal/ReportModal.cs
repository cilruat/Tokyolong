using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that implements the feedback modal.
/// </summary>
public class ReportModal : MonoBehaviour {

    

    /// <summary>
    /// Username Field
    /// </summary>
    public string userName { get { return m_name ? m_name.text : ""; } set { if(m_name)m_name.text = value; }  }

    /// <summary>
    /// Email Field
    /// </summary>
    public string userEmail { get { return m_email ? m_email.text : ""; } set { if(m_email)m_email.text = value; }  }
        
    /// <summary>
    /// Description Field
    /// </summary>
    public string description { get { return m_description ? m_description.text : ""; } set { if(m_description)m_description.text = value; }  }

    /// <summary>
    /// Internals.
    /// </summary>    
    private InputField m_description;    
    private InputField m_name;    
    private InputField m_email;    
    private Fade m_fade;

    private Action<ReportModal> m_callback;

    /// <summary>
    /// Shows the modal and waits the input
    /// </summary>
    /// <param name="p_callback"></param>
    public void Open(Action<ReportModal> p_callback) {

        m_fade.In();
        m_callback = p_callback;
    }

    /// <summary>
    /// Closes the modal and destroys it.
    /// </summary>
    public void Close() {
        m_fade.Out();    
        Destroy(gameObject,0.8f);
    }

	/// <summary>
    /// CTOR.
    /// </summary>
	private void Awake () {

        m_fade          = GetComponent<Fade>();
        m_fade.alpha    = 0f;
        m_fade.duration = 0.25f;

        m_description = Find<InputField>("modal.content.form.description");
        m_name        = Find<InputField>("modal.content.form.name.field");
        m_email       = Find<InputField>("modal.content.form.email.field");
	}

    /// <summary>
    /// Finds a children element by its path.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="p_path"></param>
    /// <returns></returns>
    private T Find<T>(string p_path)
    {
        Transform t = transform;
        string[] p = p_path.Split('.');
        for(int i=0;i<p.Length;i++)
        {
            t = t.Find(p[i]);
            if(!t) return default(T);
        }
        return t.gameObject.GetComponent<T>();
    }
	
    /// <summary>
    /// Finds a transform by its path.
    /// </summary>
    /// <param name="p_path"></param>
    /// <returns></returns>
    private Transform Find(string p_path) { return Find<Transform>(p_path); }


    /// <summary>
    /// Callback called when any of the modal's button is clicked.
    /// </summary>
    /// <param name="p_button"></param>
    public void OnButtonClick(Button p_button) {
    
        switch(p_button.name) {

            case "send":
                Close();
                if(m_callback!=null) m_callback(this);
            break;
            
            case "cancel":
                Close();  
                if(m_callback!=null) m_callback(null);          
            break;

            case "details":
                //TBD
            break;
        }
    }

}
