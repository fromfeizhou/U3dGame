using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class MScrollViewFormat : MonoBehaviour
{
    // Use this for initialization
    private int _initIndex;     //当前创建元素
    private int _maxIndex;  //最大创建元素
    private int _row;       //行数
    private int _column;    //列数
    private GridLayoutGroup.Axis _startAxis;     //排列顺序（通常与滚动方向相反）
    private Transform _container;   //滚动容器
    private List<GameObject> _itemList; //对象池
    private List<ICell> _infoList;   //数据池

    public delegate GameObject InitFuncAction(Transform transform, ICell info);
    private InitFuncAction _initFunc;
    private UnityAction<GameObject, ICell> _updateFunc;
    private bool _canInit = false;
    void Start()
    {
        _container = transform.Find("Container");


        //计算容器大小
        CalculateRC();
    }

    // Update is called once per frame
    void Update()
    {
        if (_canInit)
        {
            InitItem();
        }
    }
    //设置数据列表 构建函数 更新函数
    public void SetCellFunc(List<ICell> infoList = null, InitFuncAction initFunc = null, UnityAction<GameObject, ICell> updateFunc = null)
    {
        clearItem();
        _itemList = new List<GameObject>();
        _initFunc = initFunc;
        _updateFunc = updateFunc;
        _infoList = infoList;
        _initIndex = 0;
        _canInit = true;
    }

    //更新数据列表
    public void UpdateInfoList()
    {
        //列表元素增加
        if (_initIndex < _maxIndex && _initIndex < _infoList.Count)
        {
            _canInit = true;
            return;
        }
        //列表元素减少
        int maxNum = _row;
        int itemNum = _itemList.Count;
        if (_startAxis == GridLayoutGroup.Axis.Horizontal)
        {
            maxNum = _column;
        }
        //减少超过一行 容器变动
        if (_infoList.Count < itemNum && (itemNum - _infoList.Count) >= maxNum)
        {
            RemoveItem(maxNum);
        }
    }

    //初始化Item 超出数据列表不在创建
    private void InitItem()
    {
        if (_initIndex >= _infoList.Count || _initIndex >= _maxIndex)
        {
            _canInit = false;
            return;
        }
        GameObject cell = _initFunc(_container, _infoList[_initIndex]);
        _itemList.Add(cell);
        _initIndex++;
        UpdateContainerSize();
    }

    //移除Item
    private void RemoveItem(int itemCount)
    {
        for (int i = 0; i < itemCount; i++)
        {
            Debug.Log("RemoveItem " + _itemList.Count);
            GameObject cell = _itemList[_itemList.Count - 1];
            _itemList.RemoveAt(_itemList.Count - 1);
            Destroy(cell);
        }
        _initIndex -= itemCount;
        UpdateContainerSize(true);
    }

    //计算容器行列
    private void CalculateRC()
    {
        RectTransform sForm = gameObject.GetComponent<RectTransform>();
        GridLayoutGroup layout = _container.gameObject.GetComponent<GridLayoutGroup>();
        _startAxis = layout.startAxis;

        float height = sForm.sizeDelta.y - layout.padding.top - layout.padding.bottom;
        if (height > layout.cellSize.y)
        {
            height += layout.spacing.y;
        }
        _row = (int)Mathf.Ceil(height * 1.0f / (layout.cellSize.y + layout.spacing.y));
        float width = sForm.sizeDelta.x - layout.padding.left - layout.padding.right;
        if (width > layout.cellSize.x)
        {
            width += layout.spacing.x;
        }
        _column = (int)Mathf.Ceil(width * 1.0f / (layout.cellSize.x + layout.spacing.x));
        if (_startAxis == GridLayoutGroup.Axis.Horizontal)
        {
            //水平方向铺排（放置满一行 换行）
            _row++;
        }
        else
        {
            //垂直方向铺排
            _column++;
        }
        _maxIndex = _row * _column;
    }

    //更新滚动容器size
    private void UpdateContainerSize(bool isConst = false)
    {
        if (_initIndex > _maxIndex)
        {
            return;
        }
        else
        {
            if (_startAxis == GridLayoutGroup.Axis.Horizontal)
            {
                //每行起始位 计算变动
                if (isConst || _initIndex % _column == 1)
                {
                    //实际位置需要数据队列下标+1
                    int row = (int)Mathf.Ceil((_initIndex) * 1.0f / _column);
                    RectTransform tForm = _container.gameObject.GetComponent<RectTransform>();
                    GridLayoutGroup layout = _container.gameObject.GetComponent<GridLayoutGroup>();
                    float height = layout.padding.top + layout.padding.bottom + row * layout.cellSize.y + (row - 1) * layout.spacing.y;

                    RectTransform sForm = gameObject.GetComponent<RectTransform>();
                    height = height > sForm.sizeDelta.y ? height : sForm.sizeDelta.y;
                    tForm.sizeDelta = new Vector2(tForm.sizeDelta.x, height);
                }
            }
            else
            {
                //每列起始位 计算变动
                if (isConst || _initIndex % _row == 1)
                {
                    //实际位置需要数据队列下标+1
                    int column = (int)Mathf.Ceil((_initIndex) * 1.0f / _row);
                    RectTransform tForm = _container.gameObject.GetComponent<RectTransform>();
                    GridLayoutGroup layout = _container.gameObject.GetComponent<GridLayoutGroup>();
                    float width = layout.padding.left + layout.padding.right + column * layout.cellSize.x + (column - 1) * layout.spacing.x;

                    RectTransform sForm = gameObject.GetComponent<RectTransform>();
                    width = width > sForm.sizeDelta.x ? width : sForm.sizeDelta.x;
                    tForm.sizeDelta = new Vector2(width, tForm.sizeDelta.y);
                }
            }
        }
    }

    protected virtual void OnDestroy()
    {
        clearItem();
    }

    private void clearItem()
    {
        if (null != _itemList)
        {
            foreach (GameObject tCell in _itemList)
            {
                Destroy(tCell);
            }
            _itemList = null;
        }
        _infoList = null;
    }
}
