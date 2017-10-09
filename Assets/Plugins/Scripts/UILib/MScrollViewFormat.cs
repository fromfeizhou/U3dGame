using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class MScrollViewFormat : MonoBehaviour
{
    // Use this for initialization
    public GameObject prefab;
    private int _initIndex;
    private int _maxIndex;
    private Transform _container;
    private GameObject[] _itemList;
    private List<CellInfo> _infoList;
    private UnityAction<Transform, CellInfo> _initFunc;
    private UnityAction<CellInfo> _updateFunc;
    private bool _canInit = false;
    void Start()
    {
        _container = transform.Find("Container");
        RectTransform sForm = gameObject.GetComponent<RectTransform>();
        RectTransform tForm = _container.gameObject.GetComponent<RectTransform>();
        GridLayoutGroup layout = _container.gameObject.GetComponent<GridLayoutGroup>();

        //计算容器大小
        float height = sForm.sizeDelta.y - layout.padding.top - layout.padding.bottom;
        if (height > layout.cellSize.y)
        {
            height += layout.spacing.y;
        }
        int row = Mathf.CeilToInt(height / (layout.cellSize.y + layout.spacing.y));
        float width = sForm.sizeDelta.x - layout.padding.left - layout.padding.right;
        if (width > layout.cellSize.x)
        {
            width += layout.spacing.x;
        }
        int column = Mathf.CeilToInt(width / (layout.cellSize.x + layout.spacing.x));
        if (layout.startAxis == GridLayoutGroup.Axis.Horizontal)
        {

            row++;
            height = layout.padding.top + layout.padding.bottom + row * layout.cellSize.y + (row - 1) * layout.spacing.y;
            tForm.sizeDelta = new Vector2(tForm.sizeDelta.x, height);

        }
        else
        {
            column++;
            width = layout.padding.left + layout.padding.right + column * layout.cellSize.x + (column - 1) * layout.spacing.x;
            tForm.sizeDelta = new Vector2(width, tForm.sizeDelta.y);
        }
        _maxIndex = row * column;

    }

    // Update is called once per frame
    void Update()
    {
        if (_canInit)
        {
            AddItem();
        }
    }

    public void SetCellFunc(List<CellInfo>infoList = null, UnityAction<Transform, CellInfo> initFunc = null, UnityAction<CellInfo> updateFunc = null)
    {
        clearItem();
        _initFunc = initFunc;
        _updateFunc = updateFunc;
        _infoList = infoList;
        _initIndex = 0;
        _canInit = true;
    }

    private void AddItem()
    {
      
        if (_initIndex >= _infoList.Count)
        {
            _initFunc(_container,null);
        }
        else
        {
            _initFunc(_container, _infoList[_initIndex]);
        }
        _initIndex++;
        if (_initIndex >= _maxIndex)
        {
            _canInit = false;
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
    }
}
