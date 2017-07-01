using UnityEngine;
using System.Collections;

public class Sound_Play : MonoBehaviour
{


	public AudioClip[] Sound; //ÒôÐ§

	void Start()
	{

	}
	public void SoundPlay(int SoundNumber)
	{

		GetComponent<AudioSource>().clip = Sound[SoundNumber];
		GetComponent<AudioSource>().Play();

	}

}
