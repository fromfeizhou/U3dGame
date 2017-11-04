using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiRoot : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject root = GameObject.Find("UiRoot");
        UIManager.GetInstance().Setup(root);
        UIManager.GetInstance().InitMainToolView();
    }

    // Update is called once per frame
    void Update () {
        
	}
}
