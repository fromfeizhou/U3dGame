using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TestScroll : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        AssetManager.LoadAsset(PathManager.GetResPathByName("Prefabs", "CellView.prefab", "UILib"), new UnityAction<Object, string>(AssetCallBack));
        GameObject srcollView = GameObject.Find("MScrollView");
        MScrollViewFormat format = srcollView.GetComponent<MScrollViewFormat>();
        RectTransform sForm = srcollView.GetComponent<RectTransform>();

        GameObject container = srcollView.transform.Find("Container").gameObject;
        RectTransform tForm = container.transform.GetComponent<RectTransform>();

        tForm.sizeDelta = new Vector2(tForm.sizeDelta.x,600);
        GridLayoutGroup layout = container.GetComponent<GridLayoutGroup>();
    }

    private void AssetCallBack(Object target, string path)
    {
        Transform tForm = transform.Find("MScrollView").Find("Container");
        for (int i = 0; i < 20; i++)
        {
            GameObject cell = Instantiate(target, tForm) as GameObject;
            CellView cellView = cell.GetComponent<CellView>();
            if (null != cellView)
            {
                cellView.info = new CellInfo(10001 + i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
