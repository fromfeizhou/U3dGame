using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MScrollViewFormat : MonoBehaviour
{
    // Use this for initialization
    public GameObject prefab;
    //private int _initIndex;
    //private int _initMax;
    private Transform _container;
    void Start()
    {
        _container = transform.Find("Container");
        RectTransform sForm = gameObject.GetComponent<RectTransform>();
        RectTransform tForm = _container.gameObject.GetComponent<RectTransform>();
        GridLayoutGroup layout = _container.gameObject.GetComponent<GridLayoutGroup>();

        //计算容器大小
        if (layout.startAxis == GridLayoutGroup.Axis.Horizontal)
        {
            float height = sForm.sizeDelta.y - layout.padding.top - layout.padding.bottom;
            if (height > layout.cellSize.y)
            {
                height += layout.spacing.y;
            }
            int row = Mathf.CeilToInt(height / (layout.cellSize.y + layout.spacing.y)) + 1;

            height = layout.padding.top + layout.padding.bottom + row * layout.cellSize.y + (row - 1) * layout.spacing.y;
            tForm.sizeDelta = new Vector2(tForm.sizeDelta.x, height);
        }
        else
        {
            float width = sForm.sizeDelta.x - layout.padding.left - layout.padding.right;
            if (width > layout.cellSize.x)
            {
                width += layout.spacing.x;
            }
            int column = Mathf.CeilToInt(width / (layout.cellSize.x + layout.spacing.x)) + 1;
            width = layout.padding.left + layout.padding.right + column * layout.cellSize.x + (column - 1) * layout.spacing.x;
            tForm.sizeDelta = new Vector2(width, tForm.sizeDelta.y);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private GameObject AddChild(GameObject prefab)
    {
        if (prefab == null || _container == null)
        {
            Debug.LogError("Could not add child with null of prefab or parent.");
            return null;
        }

        GameObject go = GameObject.Instantiate(prefab) as GameObject;
        go.layer = _container.gameObject.layer;
        go.transform.SetParent(_container, false);

        return go;
    }
}
