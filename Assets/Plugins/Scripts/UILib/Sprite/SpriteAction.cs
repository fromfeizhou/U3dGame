using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Image))]
public class SpriteAction : MonoBehaviour
{

    SpriteInforGroup _infoGroup;
    private float fTime = 0.0f;
    private float tickTime = 1f;
    void Start()
    {
        _infoGroup = SpriteFaceCache.GetAsset(0).listSpriteGroup[0];
        tickTime = SpriteFaceCache.GetAsset(0).tickTime;
    }
    // Update is called once per frame
    void Update()
    {
        fTime += Time.deltaTime;

        if (fTime >= tickTime)
        {
            GetComponent<Image>().sprite = _infoGroup.curSprteInfo.sprite;
            _infoGroup.run();
            fTime = 0.0f;
        }
    }
}
