using UnityEngine;
using System.Collections;
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
    private GameObject[] _itemList; //对象池
    private List<CellInfo> _infoList;   //数据池
    private UnityAction<Transform, CellInfo> _initFunc;
    private UnityAction<GameObject, CellInfo> _updateFunc;
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
    public void SetCellFunc(List<CellInfo> infoList = null, UnityAction<Transform, CellInfo> initFunc = null, UnityAction<GameObject, CellInfo> updateFunc = null)
    {
        clearItem();
        _initFunc = initFunc;
        _updateFunc = updateFunc;
        _infoList = infoList;
        _initIndex = 0;
        _canInit = true;
    }

    //初始化Item 超出数据列表不在创建
    private void InitItem()
    {
        if (_initIndex >= _infoList.Count || _initIndex >= _maxIndex)
        {
            _canInit = false;
            return;
        }
        _initFunc(_container, _infoList[_initIndex]);
        UpdateContainerSize();
        _initIndex++;

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
    private void UpdateContainerSize()
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
                if (_initIndex % _column == 0)
                {
                    //实际位置需要数据队列下标+1
                    int row = (int)Mathf.Ceil((_initIndex + 1) * 1.0f / _column);
                    RectTransform tForm = _container.gameObject.GetComponent<RectTransform>();
                    GridLayoutGroup layout = _container.gameObject.GetComponent<GridLayoutGroup>();
                    float height = layout.padding.top + layout.padding.bottom + row * layout.cellSize.y + (row - 1) * layout.spacing.y;
                    if (height > tForm.sizeDelta.y)
                    {
                        tForm.sizeDelta = new Vector2(tForm.sizeDelta.x, height);
                    }
                }
            }
            else
            {
                //每列起始位 计算变动
                if (_initIndex % _row == 0)
                {
                    //实际位置需要数据队列下标+1
                    int column = (int)Mathf.Ceil((_initIndex + 1) * 1.0f / _row);
                    RectTransform tForm = _container.gameObject.GetComponent<RectTransform>();
                    GridLayoutGroup layout = _container.gameObject.GetComponent<GridLayoutGroup>();
                    float width = layout.padding.left + layout.padding.right + column * layout.cellSize.x + (column - 1) * layout.spacing.x;
                    if (width > tForm.sizeDelta.y)
                    {
                        tForm.sizeDelta = new Vector2(width,tForm.sizeDelta.y);
                    }
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
