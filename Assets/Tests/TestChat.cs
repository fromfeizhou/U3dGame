using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChat : MonoBehaviour {
    private GameObject _imgBtn;
    private GameObject _scrollView;
	// Use this for initialization
	void Start () {
        _imgBtn = GameObject.Find("MImgBtn").gameObject;
        _imgBtn.GetComponent<MImgBtnFormat>().OnBtnClick.AddListener(Click);
        _scrollView = GameObject.Find("Scroll View").gameObject;
	}
    private void Click(string val)
    {
        Debug.Log(val);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
