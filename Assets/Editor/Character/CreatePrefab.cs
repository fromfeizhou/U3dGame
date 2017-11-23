using System.Collections.Generic;  
using System.IO;  
using UnityEditor;  
using UnityEngine;  
using System.Xml;
using UnityEditor.Animations;

class CreatePrefabs
{
    public static string m_ModeName = "Male";

    //创建Prefab  
    [MenuItem("MyTool/Character/Create Prefabs")]

    static void CreatePrefabSelect()
    {
        foreach (Object o in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
        {
            if (!(o is GameObject)) continue;
            if (o.name.Contains("@")) continue;
            if (!AssetDatabase.GetAssetPath(o).Contains("/Characters/")) continue;

            //整体路径
            string filePathWithName = AssetDatabase.GetAssetPath(o);
            //带后缀的文件名
            string fileNameWithExtension = Path.GetFileName(filePathWithName);
            //不带文件名的路径
            string filePath = filePathWithName.Replace(fileNameWithExtension, "");

            CreatePrefab(filePath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void CreatePrefab(string filePath)
    {
        string path = PathManager.CombinePath(filePath, CreatePrefabs.m_ModeName + ".fbx");
        GameObject go = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
        //关联材质
        for (int i = 0; i < go.transform.childCount; i++)
        {
            //Transform tf = go.transform.GetChild(i);
            //string partName = tf.name;
            //if (partName.StartsWith("equip") || partName.StartsWith("body"))
            //{
            //    string matPath = filePath + string.Format("/materials/{0}.mat", partName);
            //    SkinnedMeshRenderer sr = tf.GetComponent<SkinnedMeshRenderer>();
            //    sr.material = AssetDatabase.LoadAssetAtPath(matPath, typeof(UnityEngine.Material)) as UnityEngine.Material;
            //}
        }
        //创建状态机
        AnimatorController ac = AutoCreateControl.CreateControl(filePath);
        go.GetComponent<Animator>().runtimeAnimatorController = ac;
        //GlobalDefine.SetLayer(go, GlobalDefine.Layer_character);

        string foldPath = filePath + "Prefab";
        if (!Directory.Exists(foldPath))
        {
            Directory.CreateDirectory(foldPath);
        }
        string prefabPath = PathManager.CombinePath(filePath,"model.prefab");
        PrefabUtility.CreatePrefab(prefabPath, go);
        // GameObject.DestroyImmediate(go);
    }
}