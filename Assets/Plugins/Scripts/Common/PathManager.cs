using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
[System.Serializable]
public class JsonPathMode
{
    //JsonArrayModel类型的列表
    public List<JsonPathArrModel> infoList;
}
[System.Serializable]
public class JsonPathArrModel
{
    //对应Json中属性 名字要一样
    public string key;
    public string path;

}

public class PathManager
{
    public static Dictionary<string, string> pathDic;
    public static string configPath = "Assets/ResoucesLib/Config";
    public static string resoucePath = "Assets/ResoucesLib";
    //获取resourceLib下的目录
    public static string GetResLibPath(string key)
    {
        string path = "";
        if (pathDic == null)
        {
            ParsePath();
        }
         if (pathDic.ContainsKey(key))
        {
            path = System.IO.Path.Combine(PathManager.resoucePath, pathDic[key]);
        }
        return path;

    }

    //解析path配置
    public static void ParsePath()
    {
        if (pathDic != null) return;
        Debug.Log("PathManager ParsePath");
        //列表初始化
        pathDic = new Dictionary<string, string>();
        //资源加载
        AssetManager.LoadAsset(GetPathConfigPath(), new UnityAction<Object, string>(AssetLoadCallBack));
    }

    //pathJson加载回调
    private static void AssetLoadCallBack(Object target, string path)
    {
        if (null == target)
        {
            return;
        }
        TextAsset txt = target as TextAsset;
        JsonPathMode jsonObject = JsonUtility.FromJson<JsonPathMode>(txt.text);
        foreach (var info in jsonObject.infoList)
        {
            pathDic.Add(info.key, info.path);
        }
    }

    //获取地址配置文件 地址
    public static string GetPathConfigPath()
    {
        return System.IO.Path.Combine(PathManager.configPath, "pathConfig.json");
    }

    //获取语音包配置文件 地址
    public static string GetLocalStringPath()
    {
        return System.IO.Path.Combine(PathManager.configPath, "localString.txt");
    }

    //销毁
    public static void Destory()
    {
        pathDic = null;
    }
}
