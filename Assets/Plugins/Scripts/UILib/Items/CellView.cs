using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CellView : MonoBehaviour {
    private CellInfo _info;
    private Image _imgIcon;

	// Use this for initialization
	void Awake () {
        _imgIcon = gameObject.GetComponent<Image>();
	}

    public CellInfo info{
        get { return _info; }
        set
        {
            if (null != info && null != value && _info.templateId == value.templateId)
                return;
            _info = value;
            UpdateImgView();
        }
    }

    // 刷新显示
	private void UpdateImgView(){
        if (null == _info)
        {
            _imgIcon.sprite = null;
        }
        else
        {
           AssetManager.LoadAsset(_info.iconPath, new UnityAction<Object, string>(IconCallBack), typeof(Sprite));
        }
    }
    //资源加载回调
    private void IconCallBack(Object target, string path)
    {
        _imgIcon.sprite = target as Sprite;
    }

    public void destroy()
    {
        Destroy(this);
        _info = null;
    }
}
