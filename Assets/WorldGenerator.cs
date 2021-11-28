using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public int XAxis = 6;
    public int ZAxis = 6;
    public GameObject cellPrefab;
    Hexagon[] cells;

    public bool useNoise = false;

    public float height = 33;

    public enum NoiseType
    {
        Random,
        Perlin,
        Voronoi,
        Simplex,
    }

    public NoiseType noiseType = NoiseType.Perlin;

    private void Awake()
    {
        cells = new Hexagon[XAxis * ZAxis];
        int index = 0;
        for (int x = 0; x < XAxis; x++)
        {
            for (int z = 0; z < ZAxis; z++)
            {
                CreateCell(x, z, index);
                index++;
            }
        }
    }

    private void CreateCell(int x, int z, int index)
    {
        // x offset when even and uneven x iterations
        float xOffset;
        if (z % 2 == 0)
        {
            // even
            xOffset = x * (2*Hexagon.apothete);
        }
        else
        {
            // odd
            if (x == 0)
            {
                xOffset = Hexagon.apothete + +0.48f;
            } else
            {
                //xOffset = x * (3 * Hexagon.apothete);
                xOffset = Hexagon.apothete + x * (2 * Hexagon.apothete) + 0.48f;
            }

        }


        // z offset always, radius + half of radius
        //float zOffset = z * (Hexagon.radius + Hexagon.apothete / 2);
        float zOffset = z * (Hexagon.radius*2);
        
        float y = height;

        GameObject hexGameObject = Instantiate(cellPrefab, new Vector3(x,y,z), Quaternion.identity);

        Hexagon hexagon = hexGameObject.GetComponent<Hexagon>();

        if (useNoise /* add different kinds of noise */ )
        {
            float noiseValue = GetHeightFromNoise(x, z); // noiseFunction;
            y = height * noiseValue;
        }

        hexGameObject.transform.position = new Vector3(hexGameObject.transform.position.x + xOffset, y, hexGameObject.transform.position.z + zOffset);

        Debug.Log(xOffset);

        cells[index] = hexagon;
    }

    private float GetHeightFromNoise(float x, float z)
    {
        float y = 1;

        switch (noiseType)
        {
            
            case NoiseType.Random :
                y = Random.RandomRange(1, height);
                break;
            case NoiseType.Perlin:
                float noiseScale = 0.9f;
                y = Mathf.PerlinNoise(x * noiseScale, z * noiseScale);
                break;
            case NoiseType.Simplex:

                break;
            case NoiseType.Voronoi:

                break;

            default:
                break;
        }

        return y;
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


}
