using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class MainToolController : MonoBehaviour {

    // Use this for initialization
    private GameObject avatar;
    public int m_actionIndex = 0;
    public int m_textureIndex = 0;
    private List<string> _animationList;
	void Start () {
        Button btn1 = transform.Find("Button1").gameObject.GetComponent<Button>();
        btn1.onClick.AddListener(onClick1);
        Button btn2 = transform.Find("Button2").gameObject.GetComponent<Button>();
        btn2.onClick.AddListener(onClick2);
        Button btn3 = transform.Find("Button3").gameObject.GetComponent<Button>();
        btn3.onClick.AddListener(onClick3);

        _animationList = new List<string>();
        _animationList.Add("run");
        _animationList.Add("pose");
        _animationList.Add("run2");
        _animationList.Add("attack1");
        _animationList.Add("attack2");
        _animationList.Add("attack3");
        _animationList.Add("chongjibo");
        _animationList.Add("chongjibo2");
    }

    private void onClick1()
    {
        if (avatar != null) return;
        string path = PathManager.CombinePath(PathManager.resoucePath, "model/jianshi.prefab");
        AssetManager.LoadAsset(path, loadCom);
    }
    private  void loadCom(Object obj, string path)
    {
        avatar = Object.Instantiate(obj, new Vector3(0, 0, -7), Quaternion.identity) as GameObject;
    }

    private void onClick2()
    {
        m_actionIndex++;
        if (m_actionIndex >= _animationList.Count) m_actionIndex = 0;
        string action = _animationList[m_actionIndex];
        avatar.transform.Find("jianshi_skin").gameObject.GetComponent<Animation>().Play(action);
    }

    private void onClick3()
    {
        Renderer render = avatar.transform.Find("jianshi_skin").Find("jianshi").gameObject.GetComponent<Renderer>();
        m_textureIndex++;
        if (m_textureIndex > 6) m_textureIndex = 1;
        string path = PathManager.CombinePath(PathManager.resoucePath, "model/mTexture/jianshi_0" + m_textureIndex + ".png");
        render.sharedMaterial.mainTexture = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture;
        Debug.Log(path);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
