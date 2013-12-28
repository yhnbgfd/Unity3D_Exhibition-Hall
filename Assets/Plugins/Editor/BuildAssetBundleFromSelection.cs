using UnityEngine;
using UnityEditor;

public class ExportAssetBundles {
    [MenuItem("Assets/Build AssetBundle From Selection - Track dependencies")]
    static void ExportResource () {
		Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
		
		string path;
        // Bring up save panel
		if (selection.Length == 1)  // 选择了一个文件，则保存其名字
		{
			path = EditorUtility.SaveFilePanel ("Save Resource", Application.dataPath + "/Assets", (selection[0] as GameObject).name, "unity3d");
		}
		else
		{
        	path = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d");
		}
		
        if (path.Length != 0) {
            // Build the resource file from the active selection.
            BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets);
            Selection.objects = selection;
        }
    }
    [MenuItem("Assets/Build AssetBundle From Selection - No dependency tracking")]
    static void ExportResourceNoTrack () {
        // Bring up save panel
        string path = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d");
        if (path.Length != 0) {
            // Build the resource file from the active selection.
            BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path);
        }
    }
}