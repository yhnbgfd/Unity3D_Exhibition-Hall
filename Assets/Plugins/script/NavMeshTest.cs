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
			if(man.destination == man.nextPosition)
			{
				if(GameObject.Find(goOn (target.name)))
				{
					target = GameObject.Find(goOn (target.name)).transform;
				}
				else
				{
					//Debug.Log("end");
				}
			}
		}
		else if(lm.getFingersNum() < 5)
		{
			man.SetDestination(man.nextPosition);
		}
	}

	private string goOn(string route)
	{
		string nextNode = "";
		string[] routeSplit = route.Split('_');
		nextNode += routeSplit[0]+"_";
		nextNode += routeSplit[1]+"_";
		nextNode += routeSplit[2].Substring(0,1);
		nextNode += ((int.Parse(routeSplit[2].Substring(1)))+1).ToString();
		return nextNode;
	}
}
