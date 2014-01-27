using UnityEngine;
using System.Collections;

using StoneAnt.LeapMotion;

public class LMGesture {
	private LeapMotionGesture LG;
	private LeapMotionParameter LP;
	private float mouseX;
	private float mouseY;
	private float introY;
	private Helper tool;

	public LMGesture () {
		LG = new LeapMotionGesture ();
		LP = new LeapMotionParameter ();
		tool = new Helper ();
		mouseX = 0.0f;
		mouseY = 0.0f;
		introY = 0.0f;
	}

	public bool CheckLMConnection()
	{
		if(LP.IsConnected())
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public float MoveHorizontal()
	{
		Vector3 movePosition = tool.ToVector3(LP.GetPalmPosition());// lm.PalmPosition(0,0,0,0);
		float x = movePosition.x;
		return x;
	}
	public float MoveVertical()
	{
		Vector3 movePosition = tool.ToVector3(LP.GetPalmPosition());//lm.PalmPosition(0,0,0,0);
		float z = movePosition.z;
		return z;
	}

	public float getMouseX() {
		if(LP.GetHandsNumber() == 0 || LP.GetFingersNumber() > 2 || LP.GetFingersNumber() == 0)
		{
			return 0.0f;
		}
		Vector3 lookPosition = tool.ToVector3(LP.GetFingertipPosition ());//lm.FingertipPosition();
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
				if(mouseX > 0.01f)
				{
					mouseX -= 0.01f;
				}
				else
				{
					mouseX = 0.0f;
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
				if(mouseX < -0.01f)
				{
					mouseX += 0.01f;
				}
				else
				{
					mouseX = 0.0f;
				}
			}

		}
		return mouseX * 1.5f;//-0.1 ~ 0.1(0.2)
	}

	public float getMouseY() {
		if(LP.GetHandsNumber() == 0 || LP.GetFingersNumber() > 2 || LP.GetFingersNumber() == 0)
		{
			return 0.0f;
		}
		//if(lm.HandEnter())
		//{
		//	introY = LP.GetFingertipPosition().y;//lm.FingertipPosition().y;
		//}
		introY = 200.0f;
		Vector3 lookPosition = tool.ToVector3(LP.GetFingertipPosition());//lm.FingertipPosition();
		if(lookPosition.y - introY > 50)
		{
			return 0.1f;
		}
		else if(lookPosition.y - introY < -50)
		{
			return -0.1f;
		}
		return 0.0f;
	}
}
