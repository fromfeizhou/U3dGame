using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private GameStartState _state = GameStartState.LOAD_PATH;
    // Use this for initialization
    void Awak()
    {
       
    }

    void Start()
    {
        Debug.Log("GameManager Start");
    }

    private void LoadDataIndex(GameStartState state)
    {
        //地址解析
        GameCenterEvent.getInstance().addEventListener(_state.ToString(), LoadDataCom);
        PathManager.ParsePath();
    }
    private void LoadDataCom(Notification note)
    {
        //文字本地化解析
        GameCenterEvent.getInstance().addEventListener(GameStartState.LOAD_WORD.ToString(), LoadWordFlieCom);
        LocalString.ParseWord();
    }

    private void LoadWordFlieCom(Notification note)
    {
        GameCenterEvent.getInstance().removeEventListener(GameStartState.LOAD_WORD.ToString(), LoadWordFlieCom);
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
