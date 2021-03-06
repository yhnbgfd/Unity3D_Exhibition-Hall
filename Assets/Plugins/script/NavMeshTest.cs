﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using ExhibitionHall.Common;

using StoneAnt.LeapMotion;

public class NavMeshTest : MonoBehaviour {
	private LeapMotionGesture LG;
	private LeapMotionParameter LP;
	private NavMeshAgent man;
	/// <summary>
	/// 自动寻路目标位置（cube）
	/// </summary>
	private Transform target;
	/// <summary>
	/// 当前路线
	/// </summary>
	private string CurrentRoute;
	/// <summary>
	/// 下一个路线
	/// </summary>
	private string NextRoute;
	/// <summary>
	/// 路线节点
	/// </summary>
	private int Sections;
	/// <summary>
	/// 如果可以开始走动，标记true，一般是在静止状态下，且target刚刚被重新赋值后
	/// </summary>
	private bool StartWalking;
	private class RoutePara
	{
		string[] path;

	}

	/// <summary>
	/// 路径关系字典
	/// </summary>
	Dictionary<string,string[]> NavRoute;

	// Use this for initialization
	void Start () 
	{
		man = gameObject.GetComponent<NavMeshAgent>();
		LG = new LeapMotionGesture ();
		LP = new LeapMotionParameter ();
		CurrentRoute = NavMeshPara.InitPath;
		NextRoute = CurrentRoute;
		Sections = NavMeshPara.InitSections;
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
			if(man.destination == man.nextPosition)//静止
			{
				//if(man.destination != target.position)//problem:Invalid
				if(StartWalking)
				{
					man.SetDestination(target.position);
					//Debug.Log("man.SetDestination");
					StartWalking = false;//开始走了，标记false
				}
				//如果静止，查找当前路线的下一节点
				else if(GameObject.Find("NavRotue/"+CurrentRoute+"/" + (++Sections).ToString() ))
				{
					Debug.Log("find: "+CurrentRoute+"_"+Sections);
					target = GameObject.Find("NavRotue/"+CurrentRoute+"/" + Sections.ToString() ).transform;
					StartWalking = true;
				}
				//如果静止，且没找到当前路线的下一节点，则当前路线已走完
				else
				{
					StartWalking = false;
					Debug.Log(CurrentRoute + " end");//这里可以加入走完路时的触发事件
					//下一路线切换判断
					if(LG.Circle() == 1)
					{
						Debug.Log("--->");
						setNextRoute(GetNextRoute(CurrentRoute, 0));
					}
					else if(LG.Circle() == 2)
					{
						Debug.Log("<---");
						setNextRoute(GetNextRoute(CurrentRoute, 1));
					}
					//下一路线切换判断^
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
	/// 设置下一个路线
	/// </summary>
	/// <param name="route">Route.</param>
	public void setNextRoute(string route)
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
	/// 设置下一个路线
	/// </summary>
	/// <param name="id">Identifier.</param>
	public void setNextRoute(int id)
	{
		setNextRoute (GetNextRoute (CurrentRoute, id));
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
