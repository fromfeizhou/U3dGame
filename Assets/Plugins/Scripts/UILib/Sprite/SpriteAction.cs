using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Image))]
public class SpriteAction : MonoBehaviour
{

    public SpriteAsset usa;
    SpriteInforGroup _infoGroup;
    private float fTime = 0.0f;

    void Start()
    {
        _infoGroup = usa.listSpriteGroup[0];
    }
    // Update is called once per frame
    void Update()
    {
        fTime += Time.deltaTime;

        if (fTime >= usa.tickTime)
        {
            GetComponent<Image>().sprite = _infoGroup.curSprteInfo.sprite;
            _infoGroup.run();
            fTime = 0.0f;
        }
    }
}
