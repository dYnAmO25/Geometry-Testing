using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMesh : MonoBehaviour
{
    [Header("Mesh")]
    [SerializeField] int xLenght;
    [SerializeField] int zLenght;

    [Header("Perlin Noise")]
    [SerializeField] float PerlinNoiseXMod;
    [SerializeField] float PerlinNoiseStrenght;
    [SerializeField] bool WaveEffect;
    [SerializeField] float WaveSpeed;
    [SerializeField] float WaveConst;

    float y;

    Mesh mesh;

    Vector3[] v3Vertices;
    int[] iTriangles;

    Vector2[] v2UVertices;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void Update()
    {
        if (WaveEffect)
        {
            UpdateMod();
        } 
        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        //Create Plane of Vertices
        v3Vertices = new Vector3[(xLenght + 1) * (zLenght + 1)];

        v2UVertices = new Vector2[(xLenght + 1) * (zLenght + 1)];

        for (int i = 0, z = 0; z <= zLenght; z++)
        {
            for (int x = 0; x <= xLenght; x++)
            {
                y = Mathf.PerlinNoise(x * (WaveConst + Mathf.Sin(PerlinNoiseXMod)), z * (WaveConst + Mathf.Sin(PerlinNoiseXMod))) * PerlinNoiseStrenght;
                v3Vertices[i] = new Vector3(x, y, z);
                i++;

                //UVS

                v2UVertices[i] = new Vector2((float)x / xLenght, (float)z / zLenght);
            }
        }

        //Connects Vertices to a Plane
        iTriangles = new int[xLenght * zLenght * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zLenght; z++)
        {
            for (int x = 0; x < xLenght; x++)
            {
                iTriangles[tris + 0] = vert + 0;
                iTriangles[tris + 1] = vert + xLenght + 1;
                iTriangles[tris + 2] = vert + 1;
                iTriangles[tris + 3] = vert + 1;
                iTriangles[tris + 4] = vert + xLenght + 1;
                iTriangles[tris + 5] = vert + xLenght + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = v3Vertices;
        mesh.triangles = iTriangles;
        mesh.uv = v2UVertices;

        mesh.RecalculateNormals();
    }

    void UpdateMod()
    {
        PerlinNoiseXMod += WaveSpeed;
    }
}
