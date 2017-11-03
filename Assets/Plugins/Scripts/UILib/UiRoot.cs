using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiRoot : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Button btn1 = GameObject.Find("Button1").GetComponent<Button>();
        Button btn2 = GameObject.Find("Button2").GetComponent<Button>();
        btn1.onClick.AddListener(onClick1);
        btn2.onClick.AddListener(onClick2);
    }

    private void onClick1()
    {
        Debug.Log("Button1 Clicked. ClickHandler.");
        UIManager.getInstance().setup(this.transform.parent);
    }

    private void onClick2()
    {
        Debug.Log("Button2 Clicked. ClickHandler.");
        UIManager.getInstance().openUiById(0);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
