using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private static UIManager _instance;

    private GameObject _uiRoot;
    private Transform _toolLayer;
    private Transform _panelLayer;
    private Transform _tipLayer;
    


    public static UIManager GetInstance()
    {
        if (_instance == null) _instance = new UIManager();
        return _instance;
    }

    public void Setup(GameObject root)
    {
        _uiRoot = root;
        _toolLayer = _uiRoot.transform.Find("ToolLayer").transform;
        _panelLayer = _uiRoot.transform.Find("PanelLayer").transform;
        _tipLayer = _uiRoot.transform.Find("TipLayer").transform;
        //_toolLayer =  GameObject.Find("ToolLayer").transform;
        //_panelLayer = GameObject.Find("PanelLayer").transform;
        //_tipLayer = GameObject.Find("TipLayer").transform;
    }

    public void InitMainToolView()
    {
        AssetManager.LoadAsset(PathManager.GetResPath("toolView"), toolLoadCallBack);
    }

    private void toolLoadCallBack(Object obj, string path)
    {
        GameObject go = Object.Instantiate(obj) as GameObject;
        go.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        go.transform.SetParent(_toolLayer, false);
    }

    public void OpenUiPanelByName(string name)
    {
        AssetManager.LoadAsset(PathManager.GetResPath(name), panelCallBack);
    }

    private void panelCallBack(Object obj,string path)
    {
        Debug.Log(path);
        GameObject go = Object.Instantiate(obj) as GameObject;
        go.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        go.transform.SetParent(_panelLayer,false);
    }
}
