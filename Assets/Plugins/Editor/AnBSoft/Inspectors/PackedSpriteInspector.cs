// by KK
using UnityEditor;
using UnityEngine;
using System.Collections;

// Only compile if not using Unity iPhone
#if !UNITY_IPHONE || (UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_3_6 || UNITY_3_7 || UNITY_3_8 || UNITY_3_9)
[CustomEditor(typeof(PackedSprite))]
#endif
public class PackedSpriteInspector : Editor
{
	Texture2D staticTexture;
	
	// TODO: 仅有静态图片功能
	public override void OnInspectorGUI()
	{
		if (this.target is PackedSprite)
		{
			staticTexture = (Texture2D)EditorGUILayout.ObjectField("Static Texture:", AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(((PackedSprite)this.target).staticTexGUID), typeof(Texture2D)), typeof(Texture2D), false);
			
			((PackedSprite)this.target).staticTexPath = AssetDatabase.GetAssetPath(staticTexture);
			((PackedSprite)this.target).staticTexGUID = AssetDatabase.AssetPathToGUID(((PackedSprite)this.target).staticTexPath);
		}
		base.OnInspectorGUI();	
	}
}