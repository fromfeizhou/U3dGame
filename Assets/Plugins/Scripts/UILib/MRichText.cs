using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.Text;
using UnityEditor;


public class SpriteTagInfo
{
    //顶点
    public int vertId;
    //图集ID
    public int faceId;
    //标签标签
    public string action;
    //标签大小
    public Vector2 size;
    //表情位置
    public Vector3 pos;
}

public class MRichText : Text
{
    /// <summary>
    /// 图片池
    /// </summary>
    private readonly List<GameObject> _imagesPool = new List<GameObject>();

    /// <summary>
    /// 图片的最后一个顶点的索引
    /// </summary>
    private readonly List<SpriteTagInfo> _imagesTagInfoList = new List<SpriteTagInfo>();

    private string _outputText = "";

    private Transform _faceAction = null;
    private Object _prefabObj = null;

    /// <summary>
    /// 正则取出所需要的属性
    /// </summary>
    // 用正则取  [图集ID#表情Tag] ID值==-1 ,表示为超链接
    private static readonly Regex _inputRegex = new Regex(@"\[(\-{0,1}\d{0,})#(.+?)\]", RegexOptions.Singleline);

    public override void SetVerticesDirty()
    {
        base.SetVerticesDirty();
        string outputText = GetOutputText();
        if (_outputText == outputText)
            return;
        _outputText = outputText;
        
        if (GameManager.gameInit)
        {
            UpdateQuadImage();
        }
        else
        {
            GameStartEvent.getInstance().addEventListener(GameLoadStepEvent.LOAD_COM, LoadDataCom);
        }
    }

    private void LoadDataCom(Notification note)
    {

        GameStartEvent.getInstance().removeEventListener(GameLoadStepEvent.LOAD_COM, LoadDataCom);
        UpdateQuadImage();
    }

    protected void UpdateQuadImage()
    {
        if (_prefabObj == null)
        {
            _faceAction = transform.Find("FaceAction");
            _prefabObj = AssetDatabase.LoadMainAssetAtPath(PathManager.GetResPathByName("Prefabs", "FaceActoin.prefab", "UILib"));
        }
        for (int i = 0; i < _imagesTagInfoList.Count; i++)
        {
            SpriteTagInfo spInfo = _imagesTagInfoList[i];

            if (i >= _imagesPool.Count)
            {
                GameObject go = Object.Instantiate(_prefabObj) as GameObject;
                SpriteFaceAction face = go.GetComponent<SpriteFaceAction>();
                face.setIndexAction(spInfo.faceId, spInfo.action);
                var rt = go.GetComponent<RectTransform>();
                if (rt)
                {
                    rt.SetParent(_faceAction, false);
                    rt.localPosition = spInfo.pos;
                    rt.localRotation = Quaternion.identity;
                    rt.localScale = Vector3.one;
                    rt.sizeDelta = spInfo.size;
                }
                _imagesPool.Add(go);
            }
            else
            {
                GameObject go = _imagesPool[i];
                SpriteFaceAction face = go.GetComponent<SpriteFaceAction>();
                face.setIndexAction(spInfo.faceId, spInfo.action);
                go.SetActive(true);
            }
        }
        for (var i = _imagesTagInfoList.Count; i < _imagesPool.Count; i++)
        {
            if (_imagesPool[i])
            {
                _imagesPool[i].SetActive(false);
            }
        }
    }


    #region 根据正则规则更新文本
    private string GetOutputText()
    {
        _imagesTagInfoList.Clear();
        IList<UIVertex> verts = cachedTextGenerator.verts;
        StringBuilder textBuilder = new StringBuilder();
        int textIndex = 0;

        foreach (Match match in _inputRegex.Matches(text))
        {
            int tempId = 0;
            if (!string.IsNullOrEmpty(match.Groups[1].Value) && !match.Groups[1].Value.Equals("-"))
                tempId = int.Parse(match.Groups[1].Value);
            string tempTag = match.Groups[2].Value;
            //更新超链接
            if (tempId < 0)
            {
                textBuilder.Append(text.Substring(textIndex, match.Index - textIndex));
                textBuilder.Append("<color=blue>");
                int _startIndex = textBuilder.Length * 4;
                textBuilder.Append("[" + match.Groups[2].Value + "]");
                int _endIndex = textBuilder.Length * 4 - 2;
                textBuilder.Append("</color>");

                //var hrefInfo = new HrefInfo
                //{
                //    id = Mathf.Abs(tempId),
                //    startIndex = _startIndex, // 超链接里的文本起始顶点索引
                //    endIndex = _endIndex,
                //    name = match.Groups[2].Value
                //};
                //_ListHrefInfos.Add(hrefInfo);

            }
            //更新表情
            else
            {
                textBuilder.Append(text.Substring(textIndex, match.Index - textIndex));

                SpriteAsset spriteAsset = SpriteFaceCache.GetAsset(tempId);
                SpriteInforGroup tempGroup = SpriteFaceCache.GetAsset(tempId, tempTag);
                float imgSize = spriteAsset == null ? 24.0f : spriteAsset.size;
                float imgWidth = spriteAsset == null ? 1.0f : spriteAsset.width;

                int vertexIndex = textBuilder.Length * 4;
                textBuilder.Append(@"<quad size=" + imgSize + " width=" + imgWidth + " />");
                SpriteTagInfo spInfo = new SpriteTagInfo();
                spInfo.vertId = vertexIndex;
                spInfo.faceId = tempId;
                spInfo.action = tempTag;
                spInfo.size = new Vector2(imgSize * imgWidth, imgSize);
                _imagesTagInfoList.Add(spInfo);

            }

            textIndex = match.Index + match.Length;
        }

        textBuilder.Append(text.Substring(textIndex, text.Length - textIndex));
        return textBuilder.ToString();
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
        for (int i = 0; i < _imagesTagInfoList.Count; i++)
        {
            SpriteTagInfo spInfo  = _imagesTagInfoList[i];
            if ((spInfo.vertId + 4) > verts.Count)
                continue;
            for (int k = spInfo.vertId; k < spInfo.vertId + 4; k++)
            {
                //清除乱码
                UIVertex tempVertex = verts[k];
                tempVertex.uv0 = Vector2.zero;
                verts[k] = tempVertex;
            }
            if (i < _imagesPool.Count)
            {
                GameObject go = _imagesPool[i];
                RectTransform rt = go.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(verts[spInfo.vertId + 3].position.x + rt.sizeDelta.x / 2, verts[spInfo.vertId + 3].position.y + rt.sizeDelta.y / 2);

            }
           
        }
    }
    #endregion

    protected virtual void OnDestroy()
    {
        for (int i = _imagesPool.Count - 1; i >= 0; i--)
        {
            Destroy(_imagesPool[i]);
        }
        _imagesPool.Clear();
    }
}