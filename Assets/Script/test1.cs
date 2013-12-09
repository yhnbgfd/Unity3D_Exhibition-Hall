using UnityEngine;
using System.Collections;

public class test1 : MonoBehaviour {
	int testInt = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	int Test()
	{
		Debug.Log("get cs" + testInt++);
		return testInt;
	}
}
