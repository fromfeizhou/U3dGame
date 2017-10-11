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
    private MScrollRect _scrollRect; //滚动控制器
    private Transform _container;   //滚动容器
    private GridLayoutGroup _layoutGroup;   //布局对象 
    private List<GameObject> _itemList; //对象池
    private List<ICell> _infoList;   //数据池
    private int _scrollIndex; //滚动队列下标
    private int _scrollMax;    //滚动队列最大值

    public delegate GameObject InitFuncAction(Transform transform, ICell info);
    private InitFuncAction _initFunc;
    private UnityAction<GameObject, ICell> _updateFunc;
    private bool _canInit = false;
    void Start()
    {
        _scrollRect = gameObject.GetComponent<MScrollRect>();
        _container = transform.Find("Container");

        //计算容器大小
        CalculateRC();
        //监听滚动事件
        _scrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(OnScrollEvent));
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
        ClearItem();
        _itemList = new List<GameObject>();
        _initFunc = initFunc;
        _updateFunc = updateFunc;
        _infoList = infoList;
        UpdateScrollMax();

        _initIndex = 0;
        _scrollIndex = 0;
        _canInit = true;
    }

    //更新数据列表
    public void UpdateInfoList()
    {
        UpdateScrollMax();
        //列表元素增加
        if (_initIndex < _maxIndex && _initIndex < _infoList.Count)
        {
            _canInit = true;
            return;
        }
        //列表元素减少
        int maxNum = _row;
        if (IsHorizontal())
        {
            maxNum = _column;
        }
        int itemNum = _itemList.Count;

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
        _layoutGroup = _container.gameObject.GetComponent<GridLayoutGroup>();
        _startAxis = _layoutGroup.startAxis;

        float height = sForm.sizeDelta.y;
        if (height > _layoutGroup.cellSize.y)
        {
            height += _layoutGroup.spacing.y;
        }
        _row = (int)Mathf.Ceil(height * 1.0f / (_layoutGroup.cellSize.y + _layoutGroup.spacing.y));
        float width = sForm.sizeDelta.x;
        if (width > _layoutGroup.cellSize.x)
        {
            width += _layoutGroup.spacing.x;
        }
        _column = (int)Mathf.Ceil(width * 1.0f / (_layoutGroup.cellSize.x + _layoutGroup.spacing.x));
        if (IsHorizontal())
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

    //更新滚动队列
    private void UpdateScrollMax()
    {
        if (IsHorizontal())
        {
            _scrollMax = Mathf.CeilToInt(1.0f * (_infoList.Count - _maxIndex) / _column);
        }
        else
        {
            _scrollMax = Mathf.CeilToInt(1.0f * (_infoList.Count - _maxIndex) / _row);
        }
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
            if (IsHorizontal())
            {
                //每行起始位 计算变动
                if (isConst || _initIndex % _column == 1)
                {
                    //实际位置需要数据队列下标+1
                    int row = (int)Mathf.Ceil((_initIndex) * 1.0f / _column);
                    RectTransform tForm = _container.gameObject.GetComponent<RectTransform>();
                    float height = _layoutGroup.padding.top + _layoutGroup.padding.bottom + row * _layoutGroup.cellSize.y + (row - 1) * _layoutGroup.spacing.y;

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
                    float width = _layoutGroup.padding.left + _layoutGroup.padding.right + column * _layoutGroup.cellSize.x + (column - 1) * _layoutGroup.spacing.x;

                    RectTransform sForm = gameObject.GetComponent<RectTransform>();
                    width = width > sForm.sizeDelta.x ? width : sForm.sizeDelta.x;
                    tForm.sizeDelta = new Vector2(width, tForm.sizeDelta.y);
                }
            }
        }
    }

    //滚动监听
    private void OnScrollEvent(Vector2 pos)
    {

        //Debug.Log("V:\t" + _scrollRect.verticalNormalizedPosition + "\tH:\t" + _scrollRect.horizontalNormalizedPosition);
        if (IsHorizontal())
        {
            //水平排列 垂直滚动
            RectTransform tForm = _container.gameObject.GetComponent<RectTransform>();
            float max = _layoutGroup.padding.top + _layoutGroup.cellSize.y;
            float min = _layoutGroup.padding.top;
            if (_scrollIndex < _scrollMax && tForm.anchoredPosition.y > max)
            {
                tForm.anchoredPosition = new Vector2(tForm.anchoredPosition.x, min);
                _scrollRect.ResetDragState();
                for (int i = 0; i < _column; i++)
                {
                    _container.GetChild(0).SetSiblingIndex(_maxIndex - 1);
                }
                //滚动队列
                _scrollIndex++;
            }
            else if (_scrollIndex > 0 && tForm.anchoredPosition.y < min)
            {
                tForm.anchoredPosition = new Vector2(tForm.anchoredPosition.x, max);
                _scrollRect.ResetDragState();
                for (int i = 0; i < _column; i++)
                {
                    _container.GetChild(_maxIndex - 1).SetSiblingIndex(0);
                }
                //滚动队列
                _scrollIndex--;
            }
        }
        else
        {
            //垂直排列 水平滚动
            RectTransform tForm = _container.gameObject.GetComponent<RectTransform>();
            float max = -(_layoutGroup.padding.left + _layoutGroup.cellSize.x);
            float min = -_layoutGroup.padding.left;

            if (tForm.anchoredPosition.x < max)
            {
                tForm.anchoredPosition = new Vector2(min, tForm.anchoredPosition.y);
                _scrollRect.ResetDragState();
                for (int i = 0; i < _row; i++)
                {
                    _container.GetChild(_maxIndex - 1).SetSiblingIndex(0);
                }
                //滚动队列
                _scrollIndex++;
            }
            //else if (_scrollIndex > 0 && tForm.anchoredPosition.x < min)
            //{
            //    tForm.anchoredPosition = new Vector2(max, tForm.anchoredPosition.y);
            //    _scrollRect.ResetDragState();
            //    for (int i = 0; i < _row; i++)
            //    {
            //        _container.GetChild(0).SetSiblingIndex(_maxIndex - 1);
            //    }
            //    //滚动队列
            //    _scrollIndex--;
            //}
        }
    }

    private bool IsHorizontal()
    {
        return _startAxis == GridLayoutGroup.Axis.Horizontal;
    }

    protected virtual void OnDestroy()
    {
        ClearItem();
    }

    private void ClearItem()
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
