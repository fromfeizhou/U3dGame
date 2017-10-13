using UnityEngine;
using UnityEngine.UI;

public class CellView : MonoBehaviour {
    private ICell _info;
    private Image _imgIcon;

	// Use this for initialization
	protected virtual void Awake () {
        _imgIcon = gameObject.GetComponent<Image>();
	}

    public ICell info
    {
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
            gameObject.SetActive(false);
        }
        else
        {
           gameObject.SetActive(true);
           AssetManager.LoadAsset(_info.iconPath, IconCallBack, typeof(Sprite));
        }
    }
    //资源加载回调
    private void IconCallBack(Object target, string path)
    {
        _imgIcon.sprite = target as Sprite;
    }

    public virtual void OnDestroy()
    {
        _info = null;
    }
}
