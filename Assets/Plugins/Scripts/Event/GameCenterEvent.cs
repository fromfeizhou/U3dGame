using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCenterEvent:NotificationDelegate {

    private static GameCenterEvent _instance = null;

    //Single 
    public static GameCenterEvent getInstance()
    {
        if (_instance == null)
        {
            _instance = new GameCenterEvent();
            return _instance;
        }
        return _instance;
    }
}
