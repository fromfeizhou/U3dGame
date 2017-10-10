using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TestScroll : MonoBehaviour
{
    private GameObject _prefab;
    // Use this for initialization
    void Start()
    {
        AssetManager.LoadAsset(PathManager.GetResPathByName("Prefabs", "CellView.prefab", "UILib"), new UnityAction<Object, string>(AssetCallBack));
    }

    private void AssetCallBack(Object target, string path)
    {
        //Transform tForm = transform.Find("MScrollView").Find("Container");
        List<CellInfo> list = new List<CellInfo>();
        for (int i = 0; i < 20; i++)
        {
           CellInfo info = new CellInfo(10001 + i);
            list.Add(info);
            
        }
        _prefab = target as GameObject;
        GameObject scrollView = transform.Find("MScrollView").gameObject;
        scrollView.GetComponent<MScrollViewFormat>().SetCellFunc(list,InitItemFunc, UpdateItemFunc);
    }

    private void InitItemFunc(Transform transform,CellInfo info)
    {
        GameObject cell = Instantiate(_prefab, transform) as GameObject;
        CellView cellView = cell.GetComponent<CellView>();
        if (null != cellView)
        {
            cellView.info = info;
        }
    }

    private void UpdateItemFunc(GameObject cell,CellInfo info)
    {
        CellView cellView = cell.GetComponent<CellView>();
        if (null != cellView)
        {
            cellView.info = info;
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
