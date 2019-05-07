using UnityEngine;
using System.Collections;

namespace BSK
{
public class SoundController : MonoBehaviour {
	
	public AudioClip[] ballImpactFloor;
	public AudioClip[] ballImpactNet;
	public AudioClip[] ballImpactRing;
	public AudioClip[] ballImpactSheet;
	public AudioClip[] ballImpactPole;
	public AudioClip[] ballWoofs;
	public AudioClip ballInWind;
	public AudioClip goal;
	public AudioClip goalClear;
	public AudioClip goalClearSpecial;
	public AudioClip bonusOpen;
	public AudioClip newRecord;
	public AudioClip gameOver;
	public static SoundController data;
	private AudioSource thisAudio;
	private bool playedNR;
	
	void Start () {
		data = this;
		thisAudio = GetComponent<AudioSource>();
	}
	
	public void Stop(){
		if(thisAudio.isPlaying) {
			thisAudio.Stop();
			thisAudio.clip = null;
			thisAudio.loop = false;
		}
	}
	
	public void playBallInWind(){
		if(!thisAudio.isPlaying) {
			thisAudio.clip = ballInWind;
			thisAudio.loop = true;
			thisAudio.Play();
		}
	}
	
	public void playGoal(){
		thisAudio.PlayOneShot(goal);
	}
	
	public void playClearGoal(){
		thisAudio.PlayOneShot(goalClear);
	}
	
	public void playClearSpecialGoal(){
		thisAudio.PlayOneShot(goalClearSpecial);
	}
	
	public void playNewRecord(){
		thisAudio.PlayOneShot(newRecord);
	}
	
	public void playGameOver(){
		thisAudio.PlayOneShot(gameOver);
	}
	}
}
