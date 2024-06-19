using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMatrix : MonoBehaviour
{
    private GameObject grayCubePrefab;

    private GameObject yellowCubePrefab;
    
    private float spacing = 1.01f;

    private void Awake()
    {
        grayCubePrefab = Resources.Load<GameObject>("Prefabs/GreyCube");

        yellowCubePrefab = Resources.Load<GameObject>("Prefabs/YellowCube");
    }

    private void Start()
    {
        GenerateRandomCubeMatrix();
    }

    private void GenerateRandomCubeMatrix()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                GameObject cubePrefab = GetRandomCubePrefab();
                
                Vector3 position = new Vector3(col * spacing, 0f, row * spacing);
                
                Instantiate(cubePrefab, position, Quaternion.identity);
            }
        }
    }

    private GameObject GetRandomCubePrefab()
    {
        return Random.Range(0, 2) == 0 ? grayCubePrefab : yellowCubePrefab;
    }
}

