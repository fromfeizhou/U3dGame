using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

public class CameraExtension : Editor
{
	
	public override void OnInspectorGUI(){
		base.DrawDefaultInspector();
		if(GUILayout.Button("雨松MOMO")){
 
		}
	}
}
 
[CanEditMultipleObjects()]
[CustomEditor(typeof(Camera), true)]
public class CustomExtension : CameraExtension 
{
}