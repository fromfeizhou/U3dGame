using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.Text;

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
    //表情位置索引信息
    Dictionary<int, SpriteTagInfo> _SpriteInfo = new Dictionary<int, SpriteTagInfo>();


    private string _outputText = "";

    /// <summary>
    /// 正则取出所需要的属性
    /// </summary>
    // 用正则取  [图集ID#表情Tag] ID值==-1 ,表示为超链接
    private static readonly Regex _inputRegex = new Regex(@"\[(\-{0,1}\d{0,})#(.+?)\]", RegexOptions.Singleline);

    public override void SetVerticesDirty()
    {
        base.SetVerticesDirty();
        _outputText = GetOutputText();
        UpdateQuadImage();

    }

    protected void UpdateQuadImage()
    {



    }


    #region 根据正则规则更新文本
    private string GetOutputText()
    {
        _SpriteInfo = new Dictionary<int, SpriteTagInfo>();
        StringBuilder _textBuilder = new StringBuilder();
        int _textIndex = 0;

        foreach (Match match in _inputRegex.Matches(text))
        {
            int _tempID = 0;
            if (!string.IsNullOrEmpty(match.Groups[1].Value) && !match.Groups[1].Value.Equals("-"))
                _tempID = int.Parse(match.Groups[1].Value);
            string _tempTag = match.Groups[2].Value;
            //更新超链接
            if (_tempID < 0)
            {
                _textBuilder.Append(text.Substring(_textIndex, match.Index - _textIndex));
                _textBuilder.Append("<color=blue>");
                int _startIndex = _textBuilder.Length * 4;
                _textBuilder.Append("[" + match.Groups[2].Value + "]");
                int _endIndex = _textBuilder.Length * 4 - 2;
                _textBuilder.Append("</color>");

                //var hrefInfo = new HrefInfo
                //{
                //    id = Mathf.Abs(_tempID),
                //    startIndex = _startIndex, // 超链接里的文本起始顶点索引
                //    endIndex = _endIndex,
                //    name = match.Groups[2].Value
                //};
                //_ListHrefInfos.Add(hrefInfo);

            }
            //更新表情
            else
            {
                _textBuilder.Append(text.Substring(_textIndex, match.Index - _textIndex));

                SpriteAsset spriteAsset = SpriteFaceCache.GetAsset(_tempID);
                SpriteInforGroup tempGroup = SpriteFaceCache.GetAsset(_tempID, _tempTag);
                float imgSize = spriteAsset == null ? 24.0f : spriteAsset.size;
                float imgWidth = spriteAsset == null ? 1.0f : spriteAsset.width;

                int vertexIndex = _textBuilder.Length * 4;
                _textBuilder.Append(@"<quad size=" + imgSize + " width=" + imgWidth + " />");

                SpriteTagInfo _tempSpriteTag = new SpriteTagInfo
                {
                    _ID = _tempID,
                    _Tag = _tempTag,
                    //_Size = new Vector2(spriteAsset.size * spriteAsset.width, spriteAsset.size),
                    _Pos = new Vector3[4],
                    _UV = tempGroup == null? new Vector2[4] : tempGroup.listSpriteInfor[0].uv,
                };
                if (!_SpriteInfo.ContainsKey(vertexIndex))
                    _SpriteInfo.Add(vertexIndex, _tempSpriteTag);

            }

            _textIndex = match.Index + match.Length;
        }

        _textBuilder.Append(text.Substring(_textIndex, text.Length - _textIndex));
        return _textBuilder.ToString();
    }
    #endregion

    readonly UIVertex[] m_TempVerts = new UIVertex[4];
    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        if (font == null)
            return;

        // We don't care if we the font Texture changes while we are doing our Update.
        // The end result of cachedTextGenerator will be valid for this instance.
        // Otherwise we can get issues like Case 619238.
        m_DisableFontTextureRebuiltCallback = true;

        Vector2 extents = rectTransform.rect.size;

        var settings = GetGenerationSettings(extents);
        cachedTextGenerator.Populate(_outputText, settings);

        // Apply the offset to the vertices
        IList<UIVertex> verts = cachedTextGenerator.verts;
        float unitsPerPixel = 1 / pixelsPerUnit;
        //Last 4 verts are always a new line... (\n)
        int vertCount = verts.Count - 4;
        Vector2 roundingOffset = new Vector2(verts[0].position.x, verts[0].position.y) * unitsPerPixel;
        roundingOffset = PixelAdjustPoint(roundingOffset) - roundingOffset;

        toFill.Clear();

        ClearQuadUVs(verts);


        if (roundingOffset != Vector2.zero)
        {
            for (int i = 0; i < vertCount; ++i)
            {
                int tempVertsIndex = i & 3;
                m_TempVerts[tempVertsIndex] = verts[i];
                m_TempVerts[tempVertsIndex].position *= unitsPerPixel;
                m_TempVerts[tempVertsIndex].position.x += roundingOffset.x;
                m_TempVerts[tempVertsIndex].position.y += roundingOffset.y;
                if (tempVertsIndex == 3)
                    toFill.AddUIVertexQuad(m_TempVerts);
            }
        }
        else
        {
            for (int i = 0; i < vertCount; ++i)
            {
                int tempVertsIndex = i & 3;
                m_TempVerts[tempVertsIndex] = verts[i];
                m_TempVerts[tempVertsIndex].position *= unitsPerPixel;
                if (tempVertsIndex == 3)
                    toFill.AddUIVertexQuad(m_TempVerts);
            }
        }
        m_DisableFontTextureRebuiltCallback = false;
    }

    #region 清除乱码
    private void ClearQuadUVs(IList<UIVertex> verts)
    {
        foreach (var item in _SpriteInfo)
        {
            if ((item.Key + 4) > verts.Count)
                continue;
            for (int i = item.Key; i < item.Key + 4; i++)
            {
                //清除乱码
                UIVertex tempVertex = verts[i];
                tempVertex.uv0 = Vector2.zero;
                verts[i] = tempVertex;
            }
        }
    }
    #endregion
}

public class SpriteTagInfo
{
    //图集ID
    public int _ID;
    //标签标签
    public string _Tag;
    //标签大小
    public Vector2 _Size;
    //表情位置
    public Vector3[] _Pos;
    //uv
    public Vector2[] _UV;
}