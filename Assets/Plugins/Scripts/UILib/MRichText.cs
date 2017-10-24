using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class MRichText : Text
{
    /// <summary>
    /// 图片池
    /// </summary>
    private readonly List<Image> _imagesPool = new List<Image>();

    /// <summary>
    /// 图片的最后一个顶点的索引
    /// </summary>
    private readonly List<int> _imagesVertexIndex = new List<int>();

    /// <summary>
    /// 正则取出所需要的属性
    /// </summary>
    // 用正则取  [图集ID#表情Tag] ID值==-1 ,表示为超链接
    private static readonly Regex _inputRegex = new Regex(@"\[(\-{0,1}\d{0,})#(.+?)\]", RegexOptions.Singleline);

    public override void SetVerticesDirty()
    {
        base.SetVerticesDirty();
        UpdateQuadImage();
    }

    //文本替换quad
    protected void UpdateQuadImage()
    {
        foreach (Match match in _inputRegex.Matches(text))
        {
        }
    }

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        base.OnPopulateMesh(toFill);
    }
}