using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    // Use this for initialization
    void Awak()
    {
       
    }

    void Start()
    {
        Debug.Log("GameManager Start");
        //地址解析
        PathManager.ParsePath();
        //文字本地化解析
        LocalString.ParseWord();
        
    }


    // Update is called once per frame
    void Update()
    {

    }

    void Destroy()
    {
        LocalString.Destroy();
    }
}
