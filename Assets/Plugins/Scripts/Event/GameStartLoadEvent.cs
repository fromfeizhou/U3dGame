using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartEvent : NotificationDelegate
{

    private static GameStartEvent _instance = null;

    //Single 
    public static GameStartEvent getInstance()
    {
        if (_instance == null)
        {
            _instance = new GameStartEvent();
            return _instance;
        }
        return _instance;
    }
}
