﻿using UnityEngine;
using System.Collections;

public class Quad : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        CreateQuad();
    }

    private float m_width = 1;
    private float m_length = 1;

    public void CreateQuad()
    {
        /* 1. 顶点，三角形，法线，uv坐标, 绝对必要的部分只有顶点和三角形。 
              如果模型中不需要场景中的光照，那么就不需要法线。如果模型不需要贴材质，那么就不需要UV */
        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uv = new Vector2[4];

        vertices[0] = new Vector3(0, 0, 0);
        uv[0] = new Vector2(0, 0);
        normals[0] = Vector3.up;

        vertices[1] = new Vector3(0, 0, m_length);
        uv[1] = new Vector2(0, 1);
        normals[1] = Vector3.up;


        vertices[2] = new Vector3(m_width, 0, m_length);
        uv[2] = new Vector2(1, 1);
        normals[2] = Vector3.up;

        vertices[3] = new Vector3(m_width, 0, 0);
        uv[3] = new Vector2(1, 0);
        normals[3] = Vector3.up;

        /*2. 三角形,顶点索引： 
         三角形是由3个整数确定的，各个整数就是角的顶点的index。 各个三角形的顶点的顺序通常由下往上数， 可以是顺时针也可以是逆时针，这通常取决于我们从哪个方向看三角形。 通常，当mesh渲染时，"逆时针" 的面会被挡掉。 我们希望保证顺时针的面与法线的主向一致 */
        int[] indices = new int[6];
        indices[0] = 0;
        indices[1] = 1;
        indices[2] = 2;

        indices[3] = 0;
        indices[4] = 2;
        indices[5] = 3;

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.triangles = indices;
        mesh.RecalculateBounds();

        MeshFilter filter = this.gameObject.AddComponent<MeshFilter>();
        if (filter != null)
        {
            filter.sharedMesh = mesh;
        }

        MeshRenderer meshRender = this.gameObject.AddComponent<MeshRenderer>();
        Shader shader = Shader.Find("Diffuse");
        meshRender.sharedMaterial = new Material(shader);
    }
}
