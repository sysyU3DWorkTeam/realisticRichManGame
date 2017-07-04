using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakData : MonoBehaviour {

	public Sprite[] debuffSpeak = new Sprite[7];
	public Sprite[] buffSpeak = new Sprite[6];

	public Sprite getDebuffSpeak() {
		int index = Random.Range (0, 7);
		return debuffSpeak [index];
	}
	public Sprite getBuffSpeak() {
		int index = Random.Range (0, 6);
		return buffSpeak [index];
	}
	public Sprite getSpeak() {
		int index = Random.Range (0, 2);
		if (index == 0)
			return getDebuffSpeak ();
		else
			return getBuffSpeak ();
	}
}
