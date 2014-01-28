using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

using StoneAnt.LeapMotion;

public class NavMeshTest : MonoBehaviour {
	private LeapMotionGesture LG;
	private LeapMotionParameter LP;
	private NavMeshAgent man;
	private Transform target;

	private string CurrentRoute;
	private string NextRoute;
	private int Sections;

	private bool StartWalking;

	Dictionary<string,string[]> NavRoute;

	// Use this for initialization
	void Start () {
		man = gameObject.GetComponent<NavMeshAgent>();
		LG = new LeapMotionGesture ();
		LP = new LeapMotionParameter ();
		CurrentRoute = "a";
		NextRoute = CurrentRoute;
		Sections = 0;
		target = GameObject.Find("NavRotue/"+CurrentRoute+"/0").transform;
		StartWalking = true;
		NavRoute = new Dictionary<string, string[]> ();//用来存储路径对应关系
		initNavRoute ();
	}

	/// <summary>
	/// 初始化节点关系数据
	/// </summary>
	private void initNavRoute()
	{
		NavRoute.Add ("a",new string[]{"b","c"});
		NavRoute.Add ("b",new string[]{"d","e","f"});
		NavRoute.Add ("c",new string[]{"d","e","f"});
		NavRoute.Add ("d",new string[]{"g","h"});
	}
	
	// Update is called once per frame
	void Update () {
		if(LP.GetFingersNumber() == 5)
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
					if(LG.Circle() == 1)//顺时针
					{
						Debug.Log("--->");
						setNextRoute(GetNextRoute(CurrentRoute, 0));
					}
					else if(LG.Circle() == 2)//逆时针
					{
						Debug.Log("<---");
						setNextRoute(GetNextRoute(CurrentRoute, 1));
					}
				}
			}
		}
		else if(LP.GetFingersNumber() < 4)
		{
			man.SetDestination(man.nextPosition);
			StartWalking = true;
		}
	}

	/// <summary>
	/// 设置当前节点为下一个节点
	/// </summary>
	/// <param name="route">Route.</param>
	private void setNextRoute(string route)
	{
		if(route == CurrentRoute)
		{
			return;
		}
		CurrentRoute = route ;
		Sections = -1;
		StartWalking = true;
	}

	/// <summary>
	/// 获取下一个路径节点
	/// </summary>
	/// <returns>The next route.</returns>
	/// <param name="StartingPoint">当前起始节点</param>
	/// <param name="NextRouteID">下一个节点的id</param>
	private string GetNextRoute(string StartingPoint, int NextRouteID)
	{
		//Debug.Log ("GetNextRoute"+StartingPoint +"_"+NextRouteID);
		if(NavRoute.ContainsKey(StartingPoint))//存在这路径
		{
			if(NavRoute[StartingPoint].Length > NextRouteID)//节点在可选择的下一个节点范围内
			{
				//Debug.Log("NextRoute: "+NavRoute[StartingPoint][NextRouteID]);
				return NavRoute[StartingPoint][NextRouteID];
			}
		}
		return StartingPoint;
	}

}
