using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private int _loadIndex = 0;
    private List<string> _stateList;
    private List<UnityAction> _loadFuncList;
    // Use this for initialization
    void Awake()
    {
        Debug.Log("GameManager LoadStart");
        _loadIndex = 0;

        _stateList = new List<string>();
        _loadFuncList = new List<UnityAction>();

        //加载地址文件
        _stateList.Add(GameLoadStepEvent.LOAD_PATH);
        _loadFuncList.Add(PathManager.ParsePath);
        //加载本地化文本
        _stateList.Add(GameLoadStepEvent.LOAD_WORD);
        _loadFuncList.Add(LocalString.ParseWord);

        //加载表情资源
        _stateList.Add(GameLoadStepEvent.LOAD_FACE_ASSET);
        _loadFuncList.Add(SpriteFaceCache.ParseAsset);

        LoadDataIndex();
    }

    void Start()
    {
        
    }

    private void LoadDataIndex()
    {
        Debug.Log("GameManager LoadDataIndex: " + _loadIndex);
        GameStartEvent.getInstance().addEventListener(_stateList[_loadIndex].ToString(), LoadDataCom);
        _loadFuncList[_loadIndex]();
    }
    private void LoadDataCom(Notification note)
    {
        GameStartEvent.getInstance().removeEventListener(_stateList[_loadIndex].ToString(), LoadDataCom);
        _loadIndex++;
        if (_loadIndex >= _stateList.Count)
        {
            GameStart();
            return;
        }

        LoadDataIndex();
    }
   
    private void GameStart()
    {
        Debug.Log("GameManager GameStart");
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
