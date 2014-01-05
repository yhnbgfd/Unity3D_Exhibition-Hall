using UnityEngine;
using System.Collections;

public class NavMeshTest : MonoBehaviour {
	private LeapMotion lm;
	private NavMeshAgent man;
	public Transform target;
	// Use this for initialization
	void Start () {
		man = gameObject.GetComponent<NavMeshAgent>();
		lm = new LeapMotion();
	}
	
	// Update is called once per frame
	void Update () {
		if(lm.getFingersNum() == 5)
		{
			man.SetDestination(target.position);
		}
		else if(lm.getFingersNum() < 5)
		{
			man.SetDestination(man.nextPosition);
		}
	}
}
