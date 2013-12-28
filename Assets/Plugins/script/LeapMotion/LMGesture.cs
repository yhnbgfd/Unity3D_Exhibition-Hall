using UnityEngine;
using System.Collections;

public class LMGesture {
	private LeapMotion lm;
	private float mouseX = 0.0f;
	private float mouseY = 0.0f;
	private float introY = 0.0f;

	public LMGesture () {
		lm = new LeapMotion();
	}
	
	public void Moving() {
		Vector3 movePosition = lm.PalmPosition(0,0,0,0);

	}

	public float getMouseX() {
		Vector3 lookPosition = lm.FingertipPosition();
		Debug.Log(lookPosition.x);
		if(lookPosition.x > 0)
		{
			if(lookPosition.x > 100)
			{
				if(mouseX < 0.1f )
				{
					mouseX += 0.01f;
				}
				else if (mouseX >= 0.1f)
				{
					mouseX = 0.1f;
				}
			}
			else
			{
				if(mouseX > 0)
				{
					mouseX -= 0.01f;
				}
			}
		}
		else if(lookPosition.x < 0)
		{
			if(lookPosition.x < -100)
			{
				if(mouseX > -0.1f )
				{
					mouseX -= 0.01f;
				}
				else if (mouseX <= -0.1f)
				{
					mouseX = -0.1f;
				}
			}
			else
			{
				if(mouseX < 0)
				{
					mouseX += 0.01f;
				}
			}

		}
		return mouseX;//-0.1 ~ 0.1(0.2)
	}

	public float getMouseY() {
		if(lm.HandEnter())
		{
			introY = lm.FingertipPosition().y;
			//Debug.Log("HandEnter_"+introY);
		}
		introY = 200.0f;
		Vector3 lookPosition = lm.FingertipPosition();
		if(lm.getHandsNum() == 0)
		{
			return 0.0f;
		}

		//Debug.Log(lookPosition.y - introY);
		if(lookPosition.y - introY > 0)
		{
			return 0.05f;
		}
		else if(lookPosition.y - introY < 0)
		{
			return -0.05f;
		}
		return mouseY;//-0.1 ~ 0.1(0.2)
	}
}
