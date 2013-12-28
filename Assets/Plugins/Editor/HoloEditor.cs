using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

public class HoloEditor : EditorWindow
{
	public string radiusStr = "";
	public string distributeStr = "";
	public string scalRateStr = "";
	
    [MenuItem("Window/HoloEditor")]
    static void Init()
    {
        var holoEditor = EditorWindow.GetWindow(typeof(HoloEditor));
        holoEditor.Show();
    }
    void OnGUI()
    {
        GUILayout.Label("Distance Star Options");
		
		radiusStr = EditorGUILayout.TextField("radius", radiusStr);
		distributeStr = EditorGUILayout.TextField("distribute", distributeStr);
		scalRateStr = EditorGUILayout.TextField("scalRate", scalRateStr);
        
		if (GUILayout.Button("Star Selection"))
        {
            GameObject[] gameObjects = this.getSortedGameObjects();
			List<Vector3> starPosition = new List<Vector3>();
			
			starPosition = CalculateThePosition(gameObjects.Length);
			
            for (int i = 0; i < gameObjects.Length - 1; i++)
            {
                gameObjects[i].transform.localPosition = starPosition[i];
            }
        }

        GUILayout.BeginHorizontal("");

        GUILayout.EndHorizontal();
    }
	
	private GameObject[] getSortedGameObjects()
    {
        List<GameObject> tgameObjects = new List<GameObject>();
		List<GameObject> tempt = new List<GameObject>(Selection.gameObjects);
		GameObject paraGameObj = tempt[0];
		foreach(Transform child in paraGameObj.transform)
		{
			if(child.childCount != 0)
			{
				foreach(Transform childChild in child)
				{
					tgameObjects.Add(childChild.gameObject);
				}
			}
			else
			{
				tgameObjects.Add(child.gameObject);
			}
		}
		Debug.Log("" + tgameObjects.Count);
        return tgameObjects.ToArray();
    }
	
	private List<Vector3> CalculateThePosition(int totalNum)
	{
		List<Vector3> position = new List<Vector3>();
		
		int enterR = 0;
		int enterR2 = 0;

		float scalRate = Convert.ToSingle(scalRateStr); 
		float radius = Convert.ToSingle(radiusStr);
		float distribute = Convert.ToSingle(distributeStr);
		
        float x;
        float y;
        float z;
        double r = 0;

        System.Random rd = new System.Random();
		
        while (totalNum != 0)
        {
            x = (float)(rd.NextDouble() - 0.5f) * 2 * radius;
            y = (float)(rd.NextDouble() - 0.5f) * 2 * radius;
            z = (float)(rd.NextDouble() - 0.5f) * 2 * radius;
            r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
			
            if(r < radius && r > (distribute * radius))
            {
				enterR ++;
				position.Add(new Vector3(x * scalRate, y * scalRate, z * scalRate));
				totalNum--;
            }
			else
			{
				enterR2 ++;
			}
        }
		
		Debug.Log("uesable circulation " + enterR + "  unuseable circulattion  " + enterR2);
		return position;
	}
}
