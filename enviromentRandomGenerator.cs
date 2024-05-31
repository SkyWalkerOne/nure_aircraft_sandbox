using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enviromentRandomGenerator : MonoBehaviour
{
    public Transform center;
    public GameObject[] chunks;
    public int width, length;
    public float height;
    public float chunkLength;

    private GameObject [,] terrain;

    void Start()
    {
        float[,] perlin = GeneratePerlinNoise(width, length, chunks.Length);
        printArray(perlin);

        terrain = new GameObject [width, length];

        transform.Translate(-width * chunkLength / 2, 0, -length * chunkLength / 2);

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < length; j++) {
                terrain [i, j] = Instantiate(chunks[(int) perlin[i, j]]);

                terrain [i, j].transform.SetParent(transform);
                terrain [i, j].transform.localPosition = new Vector3(i * chunkLength, height, j * chunkLength);
                terrain [i, j].isStatic = true;

                for (int k = 0; k < terrain [i, j].transform.childCount; k++)
                    terrain [i, j].transform.GetChild(k).gameObject.isStatic = true;
                terrain [i, j].name = "tile " + i + " " + j;
            }
        }

        this.gameObject.isStatic = true;
    }

    float[,] GeneratePerlinNoise(int Awidth, int Aheight, float scale)
    {
        float[,] noiseMap = new float[Awidth, Aheight];

        for (int x = 0; x < Awidth; x++)
        {
            for (int y = 0; y < Aheight; y++)
            {
                float xCoord = (float)x / Awidth * scale * 0.5f;
                float yCoord = (float)y / Aheight * scale * 0.5f;

                float perlinValue = Mathf.Round(Mathf.PerlinNoise(xCoord, yCoord) * scale * 0.7f);
                noiseMap[x, y] = perlinValue;
            }
        }

        return noiseMap;
    }

    void printArray (float[,] array) {
        string outp = "";

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                outp += "[" + array[x, y] + "] ";
            }

            outp += "\n";
        }

        Debug.Log(outp);
    }
}
