using UnityEngine;
using UnityEditor;
using System.IO;
using System;
public class BuildAssetBundlesFromDirectory 
{
    [@MenuItem("Assets/Build AssetBundles From Directory of Files")]
    static void ExportAssetBundles () 
	{
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        Debug.Log("Selected Folder: " + path);
        if (path.Length != 0) 
		{
            path = path.Replace("Assets/", "");
            string [] fileEntries = Directory.GetFiles(Application.dataPath+"/"+path);
            string[] div_line = new string[] { "Assets/" };
            
			foreach(string fileName in fileEntries) 
			{
                Debug.Log("fileName="+fileName);
                string[] sTemp = fileName.Split(div_line, StringSplitOptions.RemoveEmptyEntries);
                string filePath = sTemp[1];
                filePath = "Assets/" + filePath;
                string localPath = filePath;

                UnityEngine.Object t = AssetDatabase.LoadMainAssetAtPath(localPath);
                if (t != null) 
				{
                    Debug.Log(t.name);
                    string bundlePath = "Assets/"+path+"/"  + t.name + ".unity3d";
                    Debug.Log("Building bundle at: " + bundlePath);
                    BuildPipeline.BuildAssetBundle(t, null, bundlePath, BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.CollectDependencies);
                }

            }
        }
    }
}