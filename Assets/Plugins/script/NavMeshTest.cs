using UnityEngine;
using System.Collections;

public class NavMeshTest : MonoBehaviour {
	private NavMeshAgent man;
	public Transform target;
	// Use this for initialization
	void Start () {
		man = gameObject.GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		man.SetDestination(target.position);
	}
}
