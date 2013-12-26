using UnityEngine;
using System.Collections;

public class LMGesture {
	LeapMotion lm;

	public LMGesture () {
		lm = new LeapMotion();
	}
	
	public void Moving() {
		Vector3 movePosition = lm.PalmPosition(0,0,0,0);

	}

	public float getMouseX() {
		Vector3 lookPosition = lm.FingertipPosition();
		Debug.Log(lookPosition.x);


		return 0.0f;
	}

	public float getMouseY() {
		Vector3 lookPosition = lm.FingertipPosition();



		return 0.0f;
	}
}
