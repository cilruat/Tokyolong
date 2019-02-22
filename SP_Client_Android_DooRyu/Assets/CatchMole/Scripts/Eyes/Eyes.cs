using UnityEngine;

public class Eyes : MonoBehaviour { // This class is responsible for eye animation

    float timeToShowAnimation;
    Animator animator;
    bool flag;

	// Use this for initialization
	void Awake () {
        animator = gameObject.GetComponent<Animator>();
        timeToShowAnimation = Random.Range(10, 15);
    }
	
	// Update is called once per frame
	void Update () {
        TimeToSwgoAnotherAnimation();
        if (flag)
        {
            animator.Play(Random.Range(2, 5).ToString());
            flag = false;
        }
	}

    void TimeToSwgoAnotherAnimation() { // Timer between running animations
        if (timeToShowAnimation > 0)
        {
            timeToShowAnimation -= Time.deltaTime;
        }
        else {
            timeToShowAnimation = Random.Range(10, 15); // Set the time interval between animations
            flag = true;
        }
    }
}
