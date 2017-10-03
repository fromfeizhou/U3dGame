using UnityEngine;
using System.Collections;

public class MScrollViewFormat : MonoBehaviour {
	// Use this for initialization
    public GameObject prefab;

    private int _initIndex;
    private int _initMax;
    private Transform _container;
	void Start () {
        _container = transform.Find("Container");
	}
	
	// Update is called once per frame
	void Update () {
	
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
