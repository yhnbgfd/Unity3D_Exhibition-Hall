using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExhibitionHall.Common;

public class GuideArrowManager : MonoBehaviour 
{
	private const string ArrowPath = "Scene/Guid_Arrow";
	private static Dictionary<string, List<UIStateToggleBtn>> navArrows = new Dictionary<string, List<UIStateToggleBtn>> ();
	private static Dictionary<string, Transform> ArrowsParrentTrans = new Dictionary<string, Transform>();

	void Start () 
	{
		Transform arrowsTrans = GameObject.Find (ArrowPath).transform;
		for(int i = 0; i < NavMeshPara.NavRoutes.Length; i++)
		{
			Transform arrowsParaent = arrowsTrans.FindChild(NavMeshPara.NavRoutes[i]);
			List<UIStateToggleBtn> arrows = new List<UIStateToggleBtn>();
			for(int j = 0; j < arrowsParaent.childCount; j++)
			{
				UIStateToggleBtn arrow = arrowsParaent.FindChild(j.ToString()).GetComponent<UIStateToggleBtn>();
				arrows.Add(arrow);
			}
			ArrowsParrentTrans.Add(NavMeshPara.NavRoutes[i], arrowsParaent);
			navArrows.Add(NavMeshPara.NavRoutes[i],arrows);
			arrowsParaent.gameObject.SetActive = false;
		}
	}

	public void ShowArrow(string routeIndex)
	{
		ArrowsParrentTrans [routeIndex].gameObject.SetActive = true;
	}

	void Update () 
	{
	
	}
}
