using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;
using System;

public class LeapMotion {
	public string version = "0.9.1.20131110.a";
	
	private Controller controller;
	private Frame frame;
	
	private int handsNum = 0;
	private int fingersNum = 0;
	private long lastFrameId = 0;
	private Frame sinceFrame = null;
	
	public LeapMotion()
	{
		controller = new Controller();
		controller.EnableGesture(Gesture.GestureType.TYPESWIPE);
		controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
		controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
		controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
	}
	
	private bool ProcessFrame()
    {
		if(controller.IsConnected)
		{
			frame = controller.Frame();
			return true;
		}
		return false;
    }

	public int TwoHandsPostition(string axis)
	{
		if(!ProcessFrame()) return 0;
		if(getHandsNum() == 2)
		{
			Hand rightHand = frame.Hands[0];
			Hand leftHand = frame.Hands[1];
			if(!IsRightHand(rightHand, leftHand))
			{
				Hand minhand = rightHand;
				rightHand = leftHand;
				leftHand = minhand;
			}
			switch(axis)
			{
			case "x":
				if(leftHand.PalmPosition.x - rightHand.PalmPosition.x > 0.0f)
				{
					return 1;
				}
				else if(leftHand.PalmPosition.x - rightHand.PalmPosition.x < 0.0f)
				{
					return 2;
				}
				break;
			case "y":
				if(leftHand.PalmPosition.y - rightHand.PalmPosition.y > 0.0f)
				{
					return 3;
				}
				else if(leftHand.PalmPosition.y - rightHand.PalmPosition.y < 0.0f)
				{
					return 4;
				}
				break;
			case "z":
				if(leftHand.PalmPosition.z - rightHand.PalmPosition.z > 0.0f)
				{
					return 5;
				}
				else if(leftHand.PalmPosition.z - rightHand.PalmPosition.z < 0.0f)
				{
					return 6;
				}
				break;
			}
		}
		return 0;
	}
	
	//  1
	//4 6 3       
	//  2
	public int FingerAndPalmPosition(string axis)
	{
		if(!ProcessFrame()) return 0;
		float PalmPositionX = 0.0f;
		float PalmPositionY = 0.0f;
		float PalmPositionZ = 0.0f;
		float FingerPositionX = 0.0f;
		float FingerPositionY = 0.0f;
		float FingerPositionZ = 0.0f;
		
		Hand hand = frame.Hands[0];
		Finger finger = hand.Fingers[0];
		
		PalmPositionX += hand.PalmPosition.x;
		PalmPositionY += hand.PalmPosition.y;
		PalmPositionZ += hand.PalmPosition.z;
		FingerPositionX += finger.TipPosition.x;
		FingerPositionY += finger.TipPosition.y;
		FingerPositionZ += finger.TipPosition.z;
		
		switch(axis)
		{
		case "y":
			if(FingerPositionY - PalmPositionY > 3.0f)
			{
				//Fingertip position higher than the position of the palm
				return 1;
			}
			else if(PalmPositionY - FingerPositionY > 0.0f)
			{
				//fingertip position lower than the palm of your hand
				return 2;
			}
			break;
		case "x":
			if(FingerPositionX - PalmPositionX > 0.0f)
			{
				return 3;
			}
			else if(FingerPositionX - PalmPositionX < 0.0f)
			{
				return 4;
			}
			break;
		case "z":
			if(FingerPositionZ - PalmPositionZ > 0.0f)//is it possible ?
			{
				return 5;
			}
			else if(FingerPositionZ - PalmPositionZ < 0.0f)
			{
				return 6;
			}
			break;
		}
		return 0;
	}
	
	public bool FiveFingersShrink()
	{
		if(!ProcessFrame()) return false;
		bool isFivefingers = false;
		int isZerofinger = 0;
//		if(controller.Frame(10).Id < lastFrameId)
//		{
//			return false;
//		}
		for(int f=20; f>0; f--)
		{
			frame = controller.Frame(f);
			HandList hands = frame.Hands;
			Hand hand = hands[0];
			FingerList fingers = hand.Fingers;
			if(fingers.Count >= 3)
			{
				isFivefingers = true;
			}
			if(isFivefingers)
			{
				if(fingers.Count == 0 && hands.Count != 0)
				{
					isZerofinger ++;
				}
			}
			lastFrameId = frame.Id;
		}
		if(isZerofinger > 10)
		{
			return true;
		}
		return false;
	}
	
	public bool FiveFingersExpand()
	{
		if(!ProcessFrame()) return false;
		bool hasZeroFinger = false;
		int isFiveFingers = 0;
//		if(controller.Frame(10).Id < lastFrameId)
//		{
//			return false;
//		}
		for(int f = 20; f > 0; f--)
		{
			frame = controller.Frame(f);
			HandList hands = frame.Hands;
			Hand hand = hands[0];
			FingerList fingers = hand.Fingers;
			if(fingers.Count == 0 && hands.Count != 0)
			{
				hasZeroFinger = true;
			}
			if(hasZeroFinger && hands.Count != 0)
			{
				if(fingers.Count >= 3)
				{
					isFiveFingers ++;
				}
			}
			lastFrameId = frame.Id;
		}
		if(isFiveFingers > 10)
		{
			return true;
		}
		return false;
	}
	
	public int SwipeWithTwoHands()
	{
		if(!ProcessFrame()) return 0;
		GestureList gestures = frame.Gestures();
		Gesture gesture0 = gestures[0];
		Gesture gesture1 = gestures[1];
		
		if(gesture0.Type == Gesture.GestureType.TYPESWIPE && gesture1.Type == Gesture.GestureType.TYPESWIPE)
		{
			SwipeGesture swipe0 = new SwipeGesture(gesture0);
			SwipeGesture swipe1 = new SwipeGesture(gesture1);
			if((swipe0.StartPosition.x > swipe1.StartPosition.x && swipe0.Direction.x > 0 && swipe1.Direction.x < 0) 
				|| swipe0.StartPosition.x < swipe1.StartPosition.x && swipe0.Direction.x < 0 && swipe1.Direction.x > 0)
			{
				//Debug.Log ("<--| |-->");
				return 1;
			}
			if((swipe0.StartPosition.x > swipe1.StartPosition.x && swipe0.Direction.x < 0 && swipe1.Direction.x > 0) 
				|| swipe0.StartPosition.x < swipe1.StartPosition.x && swipe0.Direction.x > 0 && swipe1.Direction.x < 0)
			{
				//Debug.Log("|--> <--|");
				return 2;
			}
		}
		return 0;
	}
	
	public int Swipes(string axis)
	{
		if(!ProcessFrame()) return 0;
		GestureList gestures = frame.Gestures();
		SwipeGesture swipeLeft = null;
		SwipeGesture swipeRight = null;
		for(int g=0; g<gestures.Count; g++)
		{
			if(gestures[g].Type == Gesture.GestureType.TYPESWIPE)
			{
				SwipeGesture swipeGesture = new SwipeGesture(gestures[g]);
				if(Math.Abs(swipeGesture.Position.x - swipeGesture.StartPosition.x) > 40.0f)
				{
					if(swipeGesture.Direction.x > 0.0f && swipeRight == null)
					{
						swipeRight = swipeGesture;
					}
					else if(swipeGesture.Direction.x < 0.0f && swipeLeft == null)
					{
						swipeLeft = swipeGesture;
					}
				}
			}
			if(swipeLeft != null && swipeRight != null)
			{
				if(swipeLeft.StartPosition.x < swipeRight.StartPosition.x)
				{
//					Debug.Log("<>");
					return 1;
				}
				else if(swipeLeft.StartPosition.x > swipeRight.StartPosition.x)
				{
//					Debug.Log("><");
					return 2;
				}
			}
		}
		return 0;
	}
	
	public int Zoom()
	{
		
		return 0;
	}
	
	public float Swipe(string axis)
	{
		if(!ProcessFrame()) return 0;
		float direction = 0.0f;
		GestureList gestures = frame.Gestures();
		for(int g=0; g<gestures.Count; g++)
		{
			if(gestures[g].Type == Gesture.GestureType.TYPESWIPE)
			{
				SwipeGesture swipe = new SwipeGesture(gestures[g]);
				switch(axis)
				{
				case "x":
					if(swipe.Speed < 500 || Math.Abs(swipe.Position.x - swipe.StartPosition.x) < 200)
					{
						continue;
					}
					direction = swipe.Direction.x;
					break;
				case "y":
//					if(swipe.Speed > 1000 && swipe.Position.y < 100)
//					{
//						direction = swipe.Direction.y;
//					}
					if(swipe.Direction.y > 0.95f)
					{
						if(swipe.Speed > 1000 && swipe.Position.y > 200)
						{
							return 1.0f;
						}
					}
					else if(swipe.Direction.y < -0.95f)
					{
						if(swipe.Speed > 1000 && swipe.Position.y < 100)
						{
							return -1.0f;
						}
					}
					break;
				case "z":
					if(swipe.Speed < 500)
					{
						continue;
					}
					direction = swipe.Direction.z;
					break;
				default:
					return 0.0f;
				}
				//direction between -1 ~ 1
				return direction;
			}
		}
		return 0.0f;
	}
	
	public int FingerSwipe(string axis)
	{
		if(!ProcessFrame()) return 0;
		Finger finger = frame.Hands[0].Fingers[0];
		Vector tipVelocity = finger.TipVelocity;
//		Debug.Log(tipVelocity.x);
		switch(axis)
		{
		case "x":
			if(Math.Sqrt(Math.Pow(tipVelocity.x,2)+Math.Pow(tipVelocity.y, 2)) > 800
				&& tipVelocity.y > 0)
			{
				if(tipVelocity.x > 0)
				{
					return 1;
				}
				else if(tipVelocity.x < 0)
				{
					return 2;
				}
			}
			break;
		case "y":
			if(tipVelocity.y > 1000)
			{
				return 3;
			}
			else if(tipVelocity.y < -1000)
			{
				return 4;
			}
			break;
		case "z":
			//if( Math.Abs(tipVelocity.x) < 200 && Math.Abs(tipVelocity.z) < 200)
			{
				if(tipVelocity.z > 500)
				{
					return 5;
				}
				else if(tipVelocity.z < -500)
				{
					return 6;
				}
			} 
			break;
		}
		return 0;
	}
	
	public int Circle()
	{
		if(!ProcessFrame()) return 0;
		GestureList gestures = frame.Gestures();
		for(int g=0; g<gestures.Count; g++)
		{
			if(gestures[g].Type == Gesture.GestureType.TYPECIRCLE)
			{
				CircleGesture circle = new CircleGesture(gestures[g]);
				if(circle.Radius < 38.0f) return 0;
				if (circle.Pointable.Direction.AngleTo(circle.Normal) <= Math.PI/2) 
				{
					return 1;//Clockwise
				}
				else
				{
					return 2;//Counterclockwise
				}
			}
		}
		return 0;
	}
	
	public Vector3 PalmPosition(int OffsetX, int OffsetY, int OffsetZ, int Speed)
	{
		if(!ProcessFrame())return VectorToV3();
		Vector position = frame.Hands[0].PalmPosition;
		Speed = (Speed==0)? 1:Speed;
		if (position.x == 0 ) 
		{
			position.x = OffsetX;
		}
		position.x = (position.x - OffsetX)/Speed;
		if (position.y == 0 ) 
		{
			position.y = OffsetY;
		}
		position.y = -(position.y - OffsetY)/Speed;
		if (position.z == 0 ) 
		{
			position.z = OffsetZ;
		}
		position.z = -(position.z - OffsetZ)/Speed;
		
		return VectorToV3(position);
	}
	
	public Vector3 FingertipPosition()
	{
		if(!ProcessFrame()) return VectorToV3();
		Vector fingerPosition = frame.Hands[0].Fingers[0].TipPosition;
		return VectorToV3(fingerPosition);
	}
	
	public int getHandsNum()
	{
		if(!ProcessFrame()) return handsNum;
		HandList hands = frame.Hands;
		handsNum = hands.Count;
		return hands.Count;
	}
	
	public int getFingersNum()
	{
		if(!ProcessFrame()) return fingersNum;
		Hand hand = frame.Hands[0];
		FingerList fingers = hand.Fingers;
		fingersNum = fingers.Count;
		return fingers.Count;
	}
	
	public float getSphereRadius()
	{
		if(!ProcessFrame()) return 0;
		Hand hand = frame.Hands[0];
		float sphereRadius = hand.SphereRadius;
		return sphereRadius;
	}
	
	public Vector3 GetSphereCenter()
	{
		if(!ProcessFrame()) return VectorToV3();
		Hand hand = frame.Hands[0];
		Vector sphereCenter = hand.SphereCenter;
		return VectorToV3(sphereCenter);
	}
	
	private bool IsRightHand(Hand hand0, Hand hand1)
	{
		if(hand0.PalmPosition.x > hand1.PalmPosition.x)
		{
			return true;
		}
		
		return false;
	}
	
	public float GetHandRotationAngle()
	{
		if(!ProcessFrame()) return 0.0f;
		HandList hands = frame.Hands;
		if(hands.Count == 0)
		{
			sinceFrame = null;
			return 0.0f;
		}
		else if(sinceFrame == null)
		{
			sinceFrame = frame;
		}
		Hand hand = hands[0];
		//Debug.Log(hand.RotationAngle(sinceFrame));
//		Debug.Log(hand.Direction.Yaw);
		return hand.RotationAngle(sinceFrame);
		//return 0.0f;
	}
	
	public Vector3 GetHandRotationAxis()
	{
		if(!ProcessFrame()) return VectorToV3();
		HandList hands = frame.Hands;
		if(hands.Count == 0)
		{
			sinceFrame = null;
			return VectorToV3();
		}
		else if(sinceFrame == null)
		{
			sinceFrame = frame;
		}
		Hand hand = hands[0];
		//Debug.Log(hand.RotationAxis(sinceFrame));
		return VectorToV3(hand.RotationAxis(sinceFrame));
	}
	
	public bool AllHandLeave()
	{
		if(!ProcessFrame()) return false;
		Frame lastFrame = controller.Frame(1);
		HandList hands = frame.Hands;
		HandList lastHands = lastFrame.Hands;
		if(lastHands.Count != 0 && hands.Count == 0)
			return true;
		else
			return false;
	}
	public bool HandEnter()
	{
		if(!ProcessFrame()) return false;
		Frame lastFrame = controller.Frame(1);
		HandList hands = frame.Hands;
		HandList lastHands = lastFrame.Hands;
		if(lastHands.Count < hands.Count)
			return true;
		else
			return false;
	}
	
	private Vector3 VectorToV3(Vector vData)
	{
		Vector3 v3Data = new Vector3(0,0,0);
		v3Data.x = vData.x;
		v3Data.y = vData.y;
		v3Data.z = vData.z;
		return v3Data;
	}
	
	private Vector3 VectorToV3()
	{
		return new Vector3(0,0,0);
	}

}
