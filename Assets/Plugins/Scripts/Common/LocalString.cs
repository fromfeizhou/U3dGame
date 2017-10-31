using System.Collections.Generic;
using UnityEngine;

public class LocalString
{
    private static Dictionary<string, string> localWord;

    /**
     * @brief 本地化文字 解析
     */
    public static void ParseWord(){
        if (localWord != null) return;
        Debug.Log("LocalString ParseWord");
        //初始化 列表
        localWord = new Dictionary<string, string>();
        //加载资源
        AssetManager.LoadAsset(PathManager.GetLocalStringPath(),AssetLoadCallBack);
    }

    //localstring加载回调
    private static void AssetLoadCallBack(Object target,string path)
    {
        if (null == target)
        {
            return;
        }
        TextAsset txt = target as TextAsset;
        string[] lines = txt.text.Split("\n"[0]);
        for (int i = 0; i < lines.Length; i++) {
            string strLine = lines[i];
            if (strLine != "")
            {
                string[] keyValue = strLine.Split(':');
                if (keyValue.Length >= 2)
                {
                    localWord[keyValue[0]] = keyValue[1].Replace("\\n", "\n").Replace("\\t", "\t");
                }
                else
                {
                    localWord[keyValue[0]] = "";
                }
            }
        }

        GameStartEvent.getInstance().dispatchEvent(GameLoadStepEvent.LOAD_WORD);
    }
    
    /**
     * @brief 获取本地化文字
     */
    public static string GetWord(string key)
    {
        if (localWord == null)
        {
            ParseWord();
        }
        if (localWord.ContainsKey(key))
        {
            return localWord[key];
        }
        return "";
    }

    /**
     * @brief 清空本地化文字记录
     */
    public static void Destroy()
    {
        localWord = null;
    }

}
