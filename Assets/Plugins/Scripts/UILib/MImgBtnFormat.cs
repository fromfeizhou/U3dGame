using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;


public class MImgBtnFormat : MBaseBtnFormat
{
    [HideInInspector]
    public string imgLabName = "queding";

    public override void Start()
    {
        base.Start();
        AssetManager.LoadAsset(GetBtnLabResPath(), new UnityAction<Object, string>(ImgLabCallBack));

    }
    //返回按钮 文本资源地址(resourcesLib目录下)
    public string GetBtnLabResPath()
    {
        string path = System.IO.Path.Combine(PathManager.GetResLibPath("ImgBtnLabPath"), imgLabName + ".png");
        return path;
    }

    private void ImgLabCallBack(Object target, string path)
    {
        RawImage rawImage = gameObject.GetComponentInChildren<RawImage>();
        rawImage.texture = target as Texture;
        rawImage.SetNativeSize();
    }
   
}
