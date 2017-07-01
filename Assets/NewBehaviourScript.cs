using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    Vector3 p;
	// Use this for initialization
	void Start () {
        p = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position += Vector3.left * Time.deltaTime * 200;
        if (this.transform.position.x < p.x -116)
        {
            this.transform.position = p;
        }
	}
}
