
using UnityEngine;
using System.Collections;

/* ==============================================================================
 * 功能描述：创建三角形Mesh
 * 创 建 者：Eci
 * 创建日期：2016/09/04
 * ==============================================================================*/
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class TestTriangle : MonoBehaviour
{


    public float sideLength = 2;
    public float angleDegree = 100;

    private MeshFilter meshFilter;

    [ExecuteInEditMode]
    private void Awake()
    {

        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = Create(sideLength, angleDegree);
    }

    private void Update()
    {
    }
    private Mesh Create(float sideLength, float angleDegree)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[3];

        float angle = Mathf.Deg2Rad * angleDegree;
        float halfAngle = angle / 2;
        vertices[0] = Vector3.zero;
        float cosA = Mathf.Cos(halfAngle);
        float sinA = Mathf.Sin(halfAngle);
        vertices[1] = new Vector3(cosA * sideLength, 0, sinA * sideLength);
        vertices[2] = new Vector3(cosA * sideLength, 0, -sinA * sideLength);

        int[] triangles = new int[3];
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = Vector2.zero;
        }
        mesh.uv = uvs;

        return mesh;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        DrawMesh();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        DrawMesh();
    }

    private void DrawMesh()
    {
        Mesh mesh = Create(sideLength, angleDegree);
        int[] tris = mesh.triangles;
        Gizmos.DrawLine(mesh.vertices[tris[0]], mesh.vertices[tris[1]]);
        Gizmos.DrawLine(mesh.vertices[tris[0]], mesh.vertices[tris[2]]);
        Gizmos.DrawLine(mesh.vertices[tris[1]], mesh.vertices[tris[2]]);
    }  

}