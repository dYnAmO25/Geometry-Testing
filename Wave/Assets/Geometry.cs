using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geometry : MonoBehaviour
{
    [Header("Circle")]
    [SerializeField] float radius;
    [SerializeField] int segments;
    [SerializeField] GameObject tester;

    [Header("Cylinder")]
    [SerializeField] bool Cylinder;
    [SerializeField] float height;

    Vector3[] vertices;
    int[] faces;

    Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }


    void Update()
    {
        if (Cylinder) { CreateCylinder(); }
        else { CreateCircle(); }
        
        UpdateMesh();
    }

    void CreateCircle()
    {
        if (segments <= 2) { return; }
        
        vertices = new Vector3[1 + segments];
        faces = new int[segments * 3];

        int verts = 0;
        float rotation = 360 / (float)segments;

        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(radius, 0, 0);
        verts += 2; ;

        GameObject slider = Instantiate(tester, vertices[0], Quaternion.identity, gameObject.transform);
        GameObject nextPosition = Instantiate(tester, vertices[1], Quaternion.identity, slider.transform);

        for (int i = 0; i < segments - 1; i++)
        {
            slider.transform.rotation = Quaternion.Euler(0, (1 + i) * rotation, 0);
            vertices[verts] = nextPosition.transform.position;
            verts++;
        }

        Destroy(slider);

        int fac = 0;
        for (int i = 0; i < segments - 1; i++)
        {
            faces[fac] = 0;
            faces[fac + 1] = i + 1;
            faces[fac + 2] = i + 2;

            fac += 3;
        }

        faces[fac] = 0;
        faces[fac + 1] = segments;
        faces[fac + 2] = 1;
        fac += 3;
    }

    void CreateCylinder()
    {
        if (segments <= 2) { return; }

        vertices = new Vector3[(1 + segments) * 2];
        faces = new int[segments * 9];

        int verts = 0;
        float rotation = 360 / (float)segments;

        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(radius, 0, 0);
        verts += 2; ;

        GameObject slider = Instantiate(tester, vertices[0], Quaternion.identity, gameObject.transform);
        GameObject nextPosition = Instantiate(tester, vertices[1], Quaternion.identity, slider.transform);

        for (int i = 0; i < segments - 1; i++)
        {
            slider.transform.rotation = Quaternion.Euler(0, (1 + i) * rotation, 0);
            vertices[verts] = nextPosition.transform.position;
            verts++;
        }

        Destroy(slider);

        int fac = 0;
        for (int i = 0; i < segments - 1; i++)
        {
            faces[fac] = 0;
            faces[fac + 1] = i + 1;
            faces[fac + 2] = i + 2;

            fac += 3;
        }

        faces[fac] = 0;
        faces[fac + 1] = segments;
        faces[fac + 2] = 1;
        fac += 3;
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = faces;

        mesh.RecalculateNormals();
    }
}
