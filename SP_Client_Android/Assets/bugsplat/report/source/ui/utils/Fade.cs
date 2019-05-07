using UnityEngine;
using System.Collections;

/// <summary>
/// Utility class to fade in/out UI elements.
/// </summary>
public class Fade : MonoBehaviour {

    /// <summary>
    /// Reference to the canvas group.
    /// </summary>
    public CanvasGroup group;
	
    /// <summary>
    /// Alpha of the target group.
    /// </summary>
    public float alpha { get { return group ? group.alpha : 0f; }  set { if(group)group.alpha = value; } }

    /// <summary>
    /// Duration of the alpha transition.
    /// </summary>
    public float duration = 0.25f;

    /// <summary>
    /// Next alpha value.
    /// </summary>
    public float m_target_alpha;

    /// <summary>
    /// CTOR.
    /// </summary>
    protected void Awake() {

        group = GetComponent<CanvasGroup>();
        if(group) m_target_alpha = group.alpha;

    }

    /// <summary>
    /// Fades In
    /// </summary>
    public void In() { SetAlpha(1f); }

    /// <summary>
    /// Fades Out
    /// </summary>
    public void Out() { SetAlpha(0f); }

    /// <summary>
    /// Sets the alpha for transition.
    /// </summary>
    /// <param name="p_alpha"></param>
    public void SetAlpha(float p_alpha) {
        m_target_alpha = p_alpha;
    }

	/// <summary>
    /// Updates the effect.
    /// </summary>
	private void Update () {

        if(!group) return;
	    if(Mathf.Abs(m_target_alpha - alpha)>=0.0000001f) {
            float d = Mathf.Max(duration,0.001f);
            alpha = Mathf.Lerp(alpha,m_target_alpha,Time.unscaledDeltaTime / d);
        }
	}
}
