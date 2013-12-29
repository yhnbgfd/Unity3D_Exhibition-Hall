using UnityEngine;
using System.Collections;

public class JSBridge : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	int Test()
	{
		Debug.Log("js asd");
		return 1;
	}

	float Move()
	{
		LMGesture lg = new LMGesture();
		float x = lg.Moving().x;
		float y = lg.Moving().y;
		float z = lg.Moving().z;
		return x;
	}
}
