using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
public class AssetManager
{
    /* 
     * @brief 加载资源
     * @param path 资源路径
     * @param callback 回调函数
     */
    public static void LoadAsset(string path, UnityAction<Object, string> callback = null,System.Type type = null)
    {
#if UNITY_EDITOR
        //编辑器模式下 资源获取
        Object obj = null;
        if (null != type)
        {
            obj = AssetDatabase.LoadAssetAtPath(path,type);
        }
        else
        {
            obj = AssetDatabase.LoadMainAssetAtPath(path);
        }
        if (null != callback)
        {
            callback(obj, path);
        }
        return;
#endif

    }


    //加载所有资源
    public static void LoadAllAsset(string path, UnityAction<Object[], string> callback = null)
    {
    #if UNITY_EDITOR
        //编辑器模式下 资源获取
        Object[] objs = null;
        objs = AssetDatabase.LoadAllAssetsAtPath(path);
        if (null != callback)
        {
            callback(objs, path);
        }
        return;
#endif
    }
}
