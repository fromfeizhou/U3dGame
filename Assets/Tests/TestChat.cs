using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestChat : MonoBehaviour {
    [SerializeField]
    private GameObject _PreChatItem;
    [SerializeField]
    private RectTransform _ChatParent;
    [SerializeField]
    private RectTransform _ViewContent;
    [SerializeField]
    private InputField _InputText;

    private GameObject _imgBtn;
    float _ViewHight = 0.0f;
	
	// Use this for initialization
	void Start () {
        _imgBtn = GameObject.Find("MImgBtn").gameObject;
        _imgBtn.GetComponent<MImgBtnFormat>().OnBtnClick.AddListener(Click);
	}

    private void Click(string val)
    {
        string _chatString = _InputText.text;
        if (string.IsNullOrEmpty(_chatString))
            return;

        GameObject _chatClone = Instantiate(_PreChatItem);
        _chatClone.transform.SetParent(_ViewContent);
        MRichText _chatText = _chatClone.transform.Find("MRichText").GetComponent<MRichText>();
        //Image _chatImage = _chatClone.transform.Find("Image").GetComponent<Image>();
        _chatText.text = _chatString;
        //  _chatText.ActiveText();
        Vector2 _imagSize = new Vector2(_chatText.preferredWidth, _chatText.preferredHeight);
        _chatClone.GetComponent<RectTransform>().sizeDelta = _imagSize;
        Vector2 _pos = new Vector2(0.0f, _ViewHight);
        _chatClone.GetComponent<RectTransform>().anchoredPosition = _pos;

        _ViewHight += -_imagSize.y;
        
        _ViewContent.sizeDelta = new Vector2(_ViewContent.sizeDelta.x, Mathf.Abs(_ViewHight));
    }
	// Update is called once per frame
	void Update () {
		
	}
}
