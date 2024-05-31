using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrainGeneraor : MonoBehaviour
{
    public Transform center;
    public GameObject[] chunks;
    public int width, length;
    public float height;
    public float chunkLength;
    public float gradientDist = 0.1f;

    private GameObject [,] terrain;

    void Start()
    {
        terrain = new GameObject [width, length];

        transform.Translate(-width * chunkLength / 2, 0, -length * chunkLength / 2);

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < length; j++) {
                int localWidth = i - width / 2;
                int localLength = j - length / 2;
                localWidth *= localWidth;
                localLength *= localLength;
                int max = (int) Mathf.Round(Mathf.Sqrt(localWidth + localLength) * chunks.Length * gradientDist);
                if (max >= chunks.Length - 1) max = chunks.Length - 1;
                terrain [i, j] = Instantiate(chunks[max]);

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
}
