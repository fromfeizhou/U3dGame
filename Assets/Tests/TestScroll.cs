using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScroll : MonoBehaviour
{
    private GameObject _prefab;
    private List<ICell> list;
    // Use this for initialization
    void Start()
    {
        AssetManager.LoadAsset(PathManager.GetResPathByName("Prefabs", "ItemView.prefab", "UILib"), AssetCallBack);
    }

    private void AssetCallBack(Object target, string path)
    {
        //Transform tForm = transform.Find("MScrollView").Find("Container");
        list = new List<ICell>();
        for (int i = 0; i <11; i++)
        {
           ItemInfo info = new ItemInfo(10001 + i,i+1);
            list.Add(info);
            
        }
        _prefab = target as GameObject;
        GameObject scrollView = transform.Find("MScrollView").gameObject;
        scrollView.GetComponent<MScrollViewFormat>().SetCellFunc(list,InitItemFunc, UpdateItemFunc);

        //Invoke("testAddItem", 3f);
    }

    private void testAddItem()
    {
        for (int i = 0; i < 20; i++)
        {
            ItemInfo info = new ItemInfo(10001 + i,i);
            list.Add(info);
        }
        Debug.Log("testAddItem" + list.Count);

        GameObject scrollView = transform.Find("MScrollView").gameObject;
        scrollView.GetComponent<MScrollViewFormat>().UpdateInfoList();
        
        //Invoke("testRemoveItem", 3f);
    }

    private void testRemoveItem()
    {
        list.RemoveRange(11, 20);
        Debug.Log("testRemoveItem" + list.Count);

        GameObject scrollView = transform.Find("MScrollView").gameObject;
        scrollView.GetComponent<MScrollViewFormat>().UpdateInfoList();

        //Invoke("testAddItem", 3f);
    }

    private GameObject InitItemFunc(ICell info)
    {
        GameObject cell = Instantiate(_prefab) as GameObject;
        ItemView cellView = cell.GetComponent<ItemView>();
        if (null != cellView)
        {
            cellView.itemInfo = info as ItemInfo;
        }
        return cell;
    }

    private void UpdateItemFunc(GameObject cell, ICell info)
    {
        ItemView cellView = cell.GetComponent<ItemView>();
        if (null != cellView)
        {
            cellView.itemInfo = info as ItemInfo;
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
