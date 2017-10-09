using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemView : CellView {
    private ItemInfo _itemInfo;
	// Update is called once per frame
    protected override void Awake()
    {
        base.Awake();
    }
    public ItemInfo itemInfo
    {
        get { return _itemInfo; }
        set
        {
            if (null != _itemInfo && null != value && _itemInfo.templateId == value.templateId)
                return;
            //父类数据更新
            info = value;
            _itemInfo = value;
        }
    }
}
