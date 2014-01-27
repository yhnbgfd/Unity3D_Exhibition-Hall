using UnityEngine;
using System.Collections;
using Leap;

public class Helper : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Tos the vector3.
	/// </summary>
	/// <returns>The vector3.</returns>
	/// <param name="v">V.</param>
	public Vector3 ToVector3(Vector v)
	{
		return new Vector3(v.x, v.y, v.z);
	}
}
