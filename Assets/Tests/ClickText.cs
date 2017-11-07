using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickText : MonoBehaviour {
    private MRichText _richText;

    void Awake()
    {
        _richText = GetComponent<MRichText>();
    }

    void OnEnable()
    {
        _richText.OnHrefClick.AddListener(OnHrefClick);
    }

    void OnDisable()
    {
        _richText.OnHrefClick.RemoveListener(OnHrefClick);
    }

    private void OnHrefClick(string hrefName, int id)
    {
        Debug.Log("点击了 " + hrefName + "  id:" + id);
    }
}
