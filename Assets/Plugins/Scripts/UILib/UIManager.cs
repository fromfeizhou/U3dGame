using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private static UIManager _instance;

    private Transform _uiRoot;

    public static UIManager getInstance()
    {
        if (_instance == null) _instance = new UIManager();
        return _instance;
    }

    public void setup(Transform root)
    {
        _uiRoot = root;
    }

    public void openUiById(int id)
    {
        AssetManager.LoadAsset(PathManager.GetResPath("Panel1"), callBack);
    }

    private void callBack(Object obj,string path)
    {
        Debug.Log(path);
        GameObject go = Object.Instantiate(obj) as GameObject;
        //go.transform.localPosition = new Vector3(512, 384, 0);
        go.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        go.transform.SetParent(_uiRoot,false);
        
    }
}
