
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
    private static readonly int ANGLE_DEGREE_PRECISION = 1000;
    private static readonly int SIDE_LENGTH_PRECISION = 1000;
    private CircleMeshCreator creator = new CircleMeshCreator();  
    private MeshFilter meshFilter;

    [ExecuteInEditMode]
    private void Awake()
    {

        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = creator.CreateMesh();
    }

    private void Update()
    {
        meshFilter.mesh = creator.CreateMesh();
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
        vertices[1] = new Vector3(cosA * sideLength, sinA * sideLength, 0);
        vertices[2] = new Vector3(cosA * sideLength, -sinA * sideLength, 0);

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

    private class CircleMeshCreator
    {
        private float _radius = 5f;
        private float _insideRaius = 3f;
        private int _segment = 50;

        private Mesh _cacheMesh;
        public Mesh CreateMesh()
        {
            if (_cacheMesh == null)
            {
                _cacheMesh = Create();
            }
            return _cacheMesh;
        }

        private Mesh Create()
        {
            //Mesh mesh = new Mesh();
            //int vlen = 1 + _segment;
            //Vector3[] vertices = new Vector3[vlen];
            //vertices[0] = Vector3.zero;  

            //float angleDegree = 360;
            //float angle = angleDegree * Mathf.Deg2Rad;
            //float curAngle = angle / 2;
            //float deltaAngle = angle / _segment;
            //for (int i = 0; i < vlen; i += 2)
            //{
            //    float cosA = Mathf.Cos(curAngle);
            //    float sinA = Mathf.Sin(curAngle);
            //    vertices[i] = new Vector3(cosA * _insideRaius, 0, sinA * _insideRaius);
            //    vertices[i + 1] = new Vector3(cosA * _radius, 0, sinA * _radius);
            //    curAngle -= deltaAngle;
            //}

            //int tlen = _segment * 3;
            //int[] triangles = new int[tlen];
            //for (int i = 0, vi = 1; i < tlen - 3; i += 3, vi++)
            //{
            //    triangles[i] = 0;
            //    triangles[i + 1] = vi;
            //    triangles[i + 2] = vi + 1;
            //}
            //triangles[tlen - 3] = 0;
            //triangles[tlen - 2] = vlen - 1;
            //triangles[tlen - 1] = 1;  //链接第一个点

            Mesh mesh = new Mesh();
            int vlen = _segment * 2 + 2;
            Vector3[] vertices = new Vector3[vlen];

            float angleDegree = 360;
            float angle = Mathf.Deg2Rad * angleDegree;
            float currAngle = angle / 2;
            float deltaAngle = angle / _segment;
            for (int i = 0; i < vlen; i += 2)
            {
                float cosA = Mathf.Cos(currAngle);
                float sinA = Mathf.Sin(currAngle);
                vertices[i] = new Vector3(cosA * _insideRaius, sinA * _insideRaius, 0);
                vertices[i + 1] = new Vector3(cosA * _radius, sinA * _radius, 0);
                currAngle -= deltaAngle;
            }

            int tlen = _segment * 6;
            int[] triangles = new int[tlen];
            for (int i = 0, vi = 0; i < tlen; i += 6, vi += 2)
            {
                triangles[i] = vi;
                triangles[i + 1] = vi + 1;
                triangles[i + 2] = vi + 3;
                triangles[i + 3] = vi + 3;
                triangles[i + 4] = vi + 2;
                triangles[i + 5] = vi;
            }

            Vector2[] uvs = new Vector2[vlen];
            for (int i = 0; i < vlen; i++)
            {
                uvs[i] = new Vector2(vertices[i].x / _radius / 2 + 0.5f, vertices[i].y / _radius / 2 + 0.5f);
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            return mesh;
        }
    }

    private class TriangleMeshCreator
    {
        private float _sideLength;
        private float _angleDegree;

        private Mesh _cacheMesh;
        public Mesh CreateMesh(float sideLength = 2, float angleDegree = 120)
        {
            if (checkDiff(sideLength, angleDegree))
            {
                Mesh newMesh = Create(sideLength, angleDegree);
                if (newMesh != null)
                {
                    _cacheMesh = newMesh;
                    this._sideLength = sideLength;
                    this._angleDegree = angleDegree;
                }
            }
            return _cacheMesh;
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
            uvs[0] = new Vector2(0, 0.5f);
            uvs[1] = Vector2.one;
            uvs[2] = Vector2.right; 
            mesh.uv = uvs;

            return mesh;
        }

        private bool checkDiff(float sideLength, float angleDegree)
        {
            return (int)((sideLength - this._sideLength) * SIDE_LENGTH_PRECISION) != 0 ||
                (int)((angleDegree - this._angleDegree) * ANGLE_DEGREE_PRECISION) != 0;
        }

    }

}