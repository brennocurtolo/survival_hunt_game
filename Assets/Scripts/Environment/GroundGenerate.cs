using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class LowPolyGround : MonoBehaviour
{
    public int width = 80;
    public int height = 80;
    public float scale = 1f;
    public float heightMultiplier = 6f;

    [Header("Low Poly Settings")]
    public int resolution = 4;

    void OnValidate()
    {
        GenerateMesh();
    }

    void GenerateMesh()
    {
        if (width <= 0 || height <= 0 || resolution <= 0) return;

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        for (int z = 0; z < height - resolution; z += resolution)
        {
            for (int x = 0; x < width - resolution; x += resolution)
            {
                Vector3 v00 = GetVertex(x, z);
                Vector3 v10 = GetVertex(x + resolution, z);
                Vector3 v01 = GetVertex(x, z + resolution);
                Vector3 v11 = GetVertex(x + resolution, z + resolution);

                bool flip = (x + z) % (resolution * 2) == 0;

                if (flip)
                {
                    AddTriangle(vertices, triangles, v00, v01, v10);
                    AddTriangle(vertices, triangles, v10, v01, v11);
                }
                else
                {
                    AddTriangle(vertices, triangles, v00, v01, v11);
                    AddTriangle(vertices, triangles, v00, v11, v10);
                }
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        // Flat shading
        mesh.normals = GenerateFlatNormals(vertices);

        // Aplicar mesh
        MeshFilter mf = GetComponent<MeshFilter>();
        MeshCollider mc = GetComponent<MeshCollider>();

        if (mf.sharedMesh != null)
        {
            DestroyImmediate(mf.sharedMesh);
        }

        mf.sharedMesh = mesh;
        mc.sharedMesh = mesh;
    }

    // 🔺 Flat shading real
    Vector3[] GenerateFlatNormals(List<Vector3> vertices)
    {
        Vector3[] normals = new Vector3[vertices.Count];

        for (int i = 0; i < vertices.Count; i += 3)
        {
            Vector3 v1 = vertices[i];
            Vector3 v2 = vertices[i + 1];
            Vector3 v3 = vertices[i + 2];

            Vector3 normal = Vector3.Cross(v2 - v1, v3 - v1).normalized;

            normals[i] = normal;
            normals[i + 1] = normal;
            normals[i + 2] = normal;
        }

        return normals;
    }

    void AddTriangle(List<Vector3> vertices, List<int> triangles, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int index = vertices.Count;

        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);

        triangles.Add(index);
        triangles.Add(index + 1);
        triangles.Add(index + 2);
    }

    Vector3 GetVertex(int x, int z)
    {
        float nx = x * 0.05f;
        float nz = z * 0.05f;

        float noise = Mathf.PerlinNoise(nx, nz);
        noise += 0.5f * Mathf.PerlinNoise(nx * 2f, nz * 2f);
        noise = Mathf.Pow(noise, 1.4f);

        float y = noise * heightMultiplier - (heightMultiplier / 2f);

        float halfWidth = (width * scale) / 2f;
        float halfHeight = (height * scale) / 2f;

        return new Vector3(
            x * scale - halfWidth,
            y,
            z * scale - halfHeight
        );
    }

    // 🔥 Botão manual no Inspector
    [ContextMenu("Generate Mesh")]
    void GenerateMeshFromInspector()
    {
        GenerateMesh();
    }
}