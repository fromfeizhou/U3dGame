/// ========================================================
/// file：InlineManager.cs
/// brief：
/// author： coding2233
/// date：
/// version：v1.0
/// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFaceCache
{
    private static Dictionary<int,SpriteAsset> _spAssetDic = null;

    public static void ParseFaceSpriteData()
    {
        _spAssetDic = new Dictionary<int, SpriteAsset>();
        string path = System.IO.Path.Combine(PathManager.GetResPath("FaceSpAsset"), "emoji.asset");
        AssetManager.LoadAsset(path, loadAssetCom);
    }

    private static void loadAssetCom(Object target, string path)
    {
        Debug.Log(target);
    }
}
