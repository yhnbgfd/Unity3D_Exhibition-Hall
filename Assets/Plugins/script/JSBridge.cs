using UnityEngine;
using System.Collections;

public class JSBridge : MonoBehaviour {
	private LMGesture lg;
	// Use this for initialization
	void Start () {
		lg = new LMGesture();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	bool CheckLeapMotionConnection()
	{
		return lg.CheckLMConnection();
	}

	float MoveHorizontal()
	{
		return lg.MoveHorizontal();//-1~1
	}

	float MoveVertical()
	{
		return lg.MoveVertical();//-1~1
	}
}
