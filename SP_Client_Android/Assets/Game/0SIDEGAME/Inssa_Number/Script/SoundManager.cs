/********************************************************************
 * Game : Freaking Game
 * Scene : Common
 * Description : Sound Manager
 * History:
 *	2016/09/25	TungNguyen	First Edition
********************************************************************/
using UnityEngine;

namespace NumberTest
{

public class SoundManager : MonoBehaviour
{
	AudioSource snd;

	public AudioClip soundFaile;
	public AudioClip soundPush;
	public AudioClip soundScore;


	public static string SOUND_ON = "SOUND_ON";
	public static string MUSIC_ON = "MUSIC_ON";

	bool canSoundPlay
	{
		get
		{
			return SoundIsOn();
		}
	}

	void Awake()
	{
		snd = FindObjectOfType<AudioSource>();
	}

	#region Play Sound
	public void PlaySoundFaile()
	{
		if (canSoundPlay)
			snd.PlayOneShot(soundFaile);
	}

	public void PlaySoundPush()
	{
		if (canSoundPlay)
			snd.PlayOneShot(soundPush);
	}

	public void PlaySoundScore()
	{
		if (canSoundPlay)
			snd.PlayOneShot(soundScore);
	}

	#endregion

	#region setting sound
	public static void SetSound(bool ON)
	{
		if (ON)
			SetSoundOn();
		else
			SetSoundOff();
	}

	public static void SetSoundOn()
	{
		PlayerPrefs.SetInt(SOUND_ON, 1);
		PlayerPrefs.Save();
	}

	public static void SetSoundOff()
	{
		PlayerPrefs.SetInt(SOUND_ON, 0);
		PlayerPrefs.Save();
	}

	public static bool SoundIsOn()
	{
		return PlayerPrefs.GetInt(SOUND_ON, 1) == 1;
	}

	#endregion
}
}