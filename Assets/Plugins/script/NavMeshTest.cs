using UnityEngine;
using System.Collections;

public class NavMeshTest : MonoBehaviour {
	private LeapMotion lm;
	private NavMeshAgent man;
	private Transform target;

	private string CurrentRoute = "a";
	private string NextRoute;
	private int Sections = 0;

	// Use this for initialization
	void Start () {
		man = gameObject.GetComponent<NavMeshAgent>();
		lm = new LeapMotion();
		NextRoute = CurrentRoute;
		target = GameObject.Find("NavRotue/"+CurrentRoute+"/" + Sections.ToString() ).transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(lm.getFingersNum() == 5)
		{
			if(CurrentRoute == NextRoute && man.destination != man.nextPosition)
			{
				man.SetDestination(target.position);
				if(man.destination == man.nextPosition)
				{
					Sections++;
					if(GameObject.Find("NavRotue/"+CurrentRoute+"/" + Sections.ToString() ))
					{
						Debug.Log("find: "+CurrentRoute+"_"+Sections);
						target = GameObject.Find("NavRotue/"+CurrentRoute+"/" + Sections.ToString() ).transform;
					}
					else
					{
						Debug.Log(CurrentRoute + " end");
						if(lm.Swipe("x") > 0)
						{
							setNextRoute("c");
						}
						else if(lm.Swipe("x") < 0)
						{
							setNextRoute("b");
						}
					}
				}
			}
			else
			{
				CurrentRoute = NextRoute;
			}
		}
		else if(lm.getFingersNum() < 4)
		{
			man.SetDestination(man.nextPosition);
		}
	}

	public void setNextRoute(string route)
	{
		NextRoute = route ;
		Sections = 0;
	}

}
