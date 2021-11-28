using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{

    // https://en.wikipedia.org/wiki/File:Uniform_tiling_63-t0.svg

    // regular Hexagon = radius equals side length
    // TODO: decouple metrics into own class (avoid to repeat calculations everytime a hexagon is generated. not too bad though

    public const int sides = 6;

    public const float radius = 6f;

    public const float perimeter = sides * radius;

    public const float area = 2 * 1.732051f * ( radius*radius ); // 1.7320508075688772935274463415058723669428052538103806280558069794 (sqr of 3) to calc area

    public const float apothete = area * 2f / perimeter;

    public const float zOffset = radius * 1.5f;

    public const float XOffset = apothete * 2f;

    Vector3[] vertices;
    Vector2[] newUV;

    int[] triangles = new int[sides];

    public float height = -100f; // maybe do different heights for eich side based on the distance to the next lower surface. (no height when adjacent to higher side)



    void Start()
    {
        GenerateMesh();
    }

    private void GenerateMesh()
    {

        int verticesAmount = 31;

        vertices = new Vector3[verticesAmount];

        triangles = new int[18];

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.Clear();

        Vector3 centerV = new Vector3(1,1,1);

        vertices[0] = centerV;

        float angle = 15f;

        // top face (double vertices for hard edge

        for (int i = 1; i < 19; i++)
        {
            Vector3 v = new Vector3(centerV.x * radius, centerV.y, centerV.z * radius);      
            vertices[i] = Quaternion.AngleAxis(60f*i + 15f, Vector3.up) * v;
        }

        int offsetCounter = 1;

        // sides going down
        for (int i = 19; i < verticesAmount; i++)
        {
            Vector3 v = new Vector3(vertices[offsetCounter].x, vertices[offsetCounter].y - height, vertices[offsetCounter].z);
            vertices[i] = v;
            offsetCounter++;
        }

        // How its inside the array
        //         0
        // 1  2  3  4  5   6
        // 7  8  9  10 11 12
        // 13 14 15 16 17 18
        // 19 20 21 22 23 24 
        // 25 26 27 28 29 30

        // how the triangles should be built
        //         0
        // 1  2  3  4  5   6
        // 7  8  9  10 11 12
        // 19 20 21 22 23 24 
        // 13 14 15 16 17 18
        // 25 26 27 28 29 30

        triangles = new int[]
        {
            // top plane
            0,1,2,

            0,2,3,

            0,3,4,

            0,4,5,

            0,5,6,

            0,6,1,

            // sides

            8,7,19,
            8,19,20,

            10,9,21,
            10,21,22,

            12,11,23,
            12,23,24,

            // offset by one to the right
            15,14,26,
            15,26,27,

            17,16,28,
            17,28,29,

            13,18,25,
            30,25,18,
        };

        mesh.vertices = vertices;
        mesh.uv = newUV;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
    }


    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }

}
