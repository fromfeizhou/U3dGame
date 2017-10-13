﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScroll : MonoBehaviour
{
    private GameObject _prefab;
    private List<ICell> list;
    // Use this for initialization
    void Start()
    {
        AssetManager.LoadAsset(PathManager.GetResPathByName("Prefabs", "CellView.prefab", "UILib"), AssetCallBack);
    }

    private void AssetCallBack(Object target, string path)
    {
        //Transform tForm = transform.Find("MScrollView").Find("Container");
        list = new List<ICell>();
        for (int i = 0; i < 12; i++)
        {
           CellInfo info = new CellInfo(10001 + i);
            list.Add(info);
            
        }
        _prefab = target as GameObject;
        GameObject scrollView = transform.Find("MScrollView").gameObject;
        scrollView.GetComponent<MScrollViewFormat>().SetCellFunc(list,InitItemFunc, UpdateItemFunc);

        GameObject scrollViewII = transform.Find("MScrollViewII").gameObject;
        scrollViewII.GetComponent<MScrollViewFormat>().SetCellFunc(list, InitItemFunc, UpdateItemFunc);

        Invoke("testAddItem", 3f);
    }

    private void testAddItem()
    {
        for (int i = 0; i < 20; i++)
        {
            CellInfo info = new CellInfo(10001 + i);
            list.Add(info);
        }
        Debug.Log("testAddItem" + list.Count);

        GameObject scrollView = transform.Find("MScrollView").gameObject;
        scrollView.GetComponent<MScrollViewFormat>().UpdateInfoList();

        GameObject scrollViewII = transform.Find("MScrollViewII").gameObject;
        scrollViewII.GetComponent<MScrollViewFormat>().UpdateInfoList();
        Invoke("testRemoveItem", 3f);
    }

    private void testRemoveItem()
    {
        list.RemoveRange(12, 20);
        Debug.Log("testRemoveItem" + list.Count);

        GameObject scrollView = transform.Find("MScrollView").gameObject;
        scrollView.GetComponent<MScrollViewFormat>().UpdateInfoList();

        GameObject scrollViewII = transform.Find("MScrollViewII").gameObject;
        scrollViewII.GetComponent<MScrollViewFormat>().UpdateInfoList();
        Invoke("testAddItem", 3f);
    }

    private GameObject InitItemFunc(ICell info)
    {
        GameObject cell = Instantiate(_prefab) as GameObject;
        CellView cellView = cell.GetComponent<CellView>();
        if (null != cellView)
        {
            cellView.info = info;
        }
        return cell;
    }

    private void UpdateItemFunc(GameObject cell, ICell info)
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
