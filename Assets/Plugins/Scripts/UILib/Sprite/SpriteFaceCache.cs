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

    public static void ParseAsset()
    {
        _spAssetDic = new Dictionary<int, SpriteAsset>();
        string path = System.IO.Path.Combine(PathManager.GetResPath("FaceSpAsset"), "emoji.asset");
        AssetManager.LoadAsset(path, loadAssetCom);
    }

    private static void loadAssetCom(Object target, string path)
    {
        SpriteAsset spAsset = target as SpriteAsset;
        if (null != spAsset)
        {
            _spAssetDic[spAsset.ID] = spAsset;
        }
        GameStartEvent.getInstance().dispatchEvent(GameLoadStepEvent.LOAD_FACE_ASSET);
    }

    public static SpriteAsset GetAsset(int index)
    {
        if (_spAssetDic.ContainsKey(index))
        {
            return _spAssetDic[index];
        }
        return null;
    }
}
