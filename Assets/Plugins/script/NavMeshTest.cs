using UnityEngine;
using System.Collections;

public class NavMeshTest : MonoBehaviour {
	private LeapMotion lm;
	private NavMeshAgent man;
	private Transform target;

	private string CurrentRoute = "a";
	private string NextRoute;
	private int Sections = 0;

	private bool StartWalking = true;

	// Use this for initialization
	void Start () {
		man = gameObject.GetComponent<NavMeshAgent>();
		lm = new LeapMotion();
		NextRoute = CurrentRoute;
		target = GameObject.Find("NavRotue/"+CurrentRoute+"/0").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(lm.getFingersNum() == 5)
		{
			if(man.destination == man.nextPosition)//Stagnant
			{
				//if(man.destination != target.position)//problem:Invalid
				if(StartWalking)
				{
					man.SetDestination(target.position);
					Debug.Log("man.SetDestination");
					StartWalking = false;
				}
				else if(GameObject.Find("NavRotue/"+CurrentRoute+"/" + (++Sections).ToString() ))
				{
					Debug.Log("find: "+CurrentRoute+"_"+Sections);
					target = GameObject.Find("NavRotue/"+CurrentRoute+"/" + Sections.ToString() ).transform;
					StartWalking = true;
				}
				else
				{
					Debug.Log(CurrentRoute + " end");
					StartWalking = false;
					if(lm.Circle() == 1)
					{
						Debug.Log("--->");
						setNextRoute("c");
					}
					else if(lm.Circle() == 2)
					{
						Debug.Log("<---");
						setNextRoute("b");
					}
				}
			}
		}
		else if(lm.getFingersNum() < 4)
		{
			man.SetDestination(man.nextPosition);
			StartWalking = true;
		}
	}

	public void setNextRoute(string route)
	{
		CurrentRoute = route ;
		Sections = -1;
		StartWalking = true;
	}

}
