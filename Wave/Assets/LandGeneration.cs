using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandGeneration : MonoBehaviour
{
    [Header("Mesh")]
    [SerializeField] int xLenght;
    [SerializeField] int zLenght;

    [Header("Perlin Noise")]
    [SerializeField] float lacunarity;
    [SerializeField] float strenght;
    [Range(0.0f, 1.0f)]
    [SerializeField] float persistance;

    Mesh mesh;

    Vector3[] v3Vertices;
    int[] iTriangles;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        //Create Plane of Vertices
        v3Vertices = new Vector3[(xLenght + 1) * (zLenght + 1)];

        for (int i = 0, z = 0; z <= zLenght; z++)
        {
            for (int x = 0; x <= xLenght; x++)
            {
                
                v3Vertices[i] = new Vector3((float)x, GetY((float)x, (float)z), (float)z);
                i++;
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

        mesh.RecalculateNormals();
    }

    float GetY(float x, float z)
    {
        float y = 0;

        float y1 = 0;
        float y2 = 0;
        float y3 = 0;

        y1 = Mathf.PerlinNoise(x, z);
        y2 = Mathf.PerlinNoise((float)x * lacunarity, (float)z / lacunarity);
        y3 = Mathf.PerlinNoise((float)x * lacunarity * lacunarity, (float)z / lacunarity * lacunarity);

        y = (y1 + y2 * persistance + y3 * persistance * persistance) * strenght;

        return y;
    }
}
