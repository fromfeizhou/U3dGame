using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using System.IO;

/**
* 功 能： 
* ───────────────────────────────────
* V0.01 2017/8/14 10:55:13 leeming 
* Copyright (c) 2017 edo Corporation. All rights reserved.
*/
public class AutoCreatePrefab
{
    private static string ParentFold = "/Resources/Character/";
    private static string ParentPath = Application.dataPath + ParentFold;

    [MenuItem("EDOTools/Charactor/3.CreatePrefabAll")]
    public static void CreatePrefabAll()
    {
        string[] dirs = Directory.GetDirectories(ParentPath, "*", SearchOption.TopDirectoryOnly);

        foreach (string dir in dirs)
        {
            if (dir.EndsWith(".svn"))
                continue;

            int lastLineIndex = dir.LastIndexOf("/");
            if (lastLineIndex == -1)
                continue;

            string foldName = dir.Substring(lastLineIndex + 1);

            try
            {
                CreatePrefab(foldName);
            }
            catch (Exception e)
            {
                Debug.LogError("----create charactor prefab fail----name=" + foldName + ",msg=" + e.Message);
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("EDOTools/Charactor/3.CreatePrefabSelect")]
    public static void CreatePrefabSelect()
    {
        UnityEngine.Object[] objs = Selection.GetFiltered(typeof(DefaultAsset), SelectionMode.TopLevel);
        foreach (UnityEngine.Object o in objs)
        {
            string path = ParentPath + o.name;
            if (Directory.Exists(path) && Directory.Exists(path + "/models"))
                CreatePrefab(o.name);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    //命名规则参考：http://192.168.1.59:10023/zentao/doc-view-2.html
    private static void CreatePrefab(string name)
    {
        string foldPath = string.Format("Assets{0}{1}", ParentFold, name);

        string path = foldPath + "/models/skin.FBX";
        GameObject go = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;

        for (int i = 0; i < go.transform.childCount; i++)
        {
            Transform tf = go.transform.GetChild(i);
            string partName = tf.name;
            if (partName.StartsWith("equip") || partName.StartsWith("body"))
            {
                string matPath = foldPath + string.Format("/materials/{0}.mat", partName);
                SkinnedMeshRenderer sr = tf.GetComponent<SkinnedMeshRenderer>();
                sr.material = AssetDatabase.LoadAssetAtPath(matPath, typeof(UnityEngine.Material)) as UnityEngine.Material;
            }
        }

        AnimatorController ac = AutoCreateControl.CreateControl(name);
        go.GetComponent<Animator>().runtimeAnimatorController = ac;
        GlobalDefine.SetLayer(go, GlobalDefine.Layer_character);

        foldPath = foldPath + "/prefab";
        if (!Directory.Exists(foldPath))
        {
            Directory.CreateDirectory(foldPath);
        }
        string prefabPath = foldPath + "/a.prefab";
        PrefabUtility.CreatePrefab(prefabPath, go);
        // GameObject.DestroyImmediate(go);
    }
}

/**
* 功 能： 
* ───────────────────────────────────
* V0.01 2017/7/18 21:35:18 leeming 
* Copyright (c) 2017 edo Corporation. All rights reserved.
*/
public class GlobalDefine
{
    public const int Layer_default = 0;
    public const int Layer_transparentfx = 1;
    public const int Layer_ignoreRaycast = 2;
    public const int Layer_water = 4;
    public const int Layer_ui = 5;
    public const int Layer_Terrain = 8;
    public const int Layer_Effect = 9;
    public const int Layer_character = 10;

    public static void SetLayer(GameObject go, int layer)
    {
        go.layer = layer;
        Transform[] children = go.GetComponentsInChildren<Transform>();
        for (int i=0;i<children.Length;i++)
        {
            if (children[i].gameObject == go)
                continue;
            SetLayer(children[i].gameObject,layer);
        }
    }

    public static bool CamerRoMode = false;      //摄像机移动模式，用于策划调角度
}

