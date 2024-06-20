using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject grayCubePrefab;
    private int cubesCount;

    public void Awake()
    {
        grayCubePrefab = Resources.Load<GameObject>("Prefabs/GreyCube");
    }

    public void Start()
    {
        SpawnCubes();
        SpawnMovableCubes();
    }

    public void Update()
    {
        if(CheckCubes())
        {
            Debug.Log("WINNER!!!!!!!!!!!!!!!");
        }
    }

    private void SpawnCubes()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject cell = TagComponent.GetObjectByTag($"c{i}r{j}");

                if (cell != null)
                {
                    int isCube = Random.Range(0, 100);

                    if (isCube < 30)
                    {
                        continue;
                    }
                    else
                    {
                        GameObject gameObject = Instantiate(grayCubePrefab, cell.transform);
                        
                        Vector3 vector = new Vector3(cell.transform.position.x, cell.transform.position.y + 0.5f, cell.transform.position.z);

                        cubesCount++;

                        gameObject.transform.position = vector;
                    }
                }
            }
        }
    }

    public void SpawnMovableCubes()
    {
        GameObject spawn = TagComponent.GetObjectByTag("Spawn");

        for (int i = 1; i <= cubesCount; i++)
        {
            GameObject gameObject = Instantiate(grayCubePrefab);

            Vector3 vector = new Vector3(spawn.transform.position.x, spawn.transform.position.y + 0.5f, spawn.transform.position.z);

            gameObject.transform.position = vector;
            gameObject.AddComponent<BoxCollider>();
        }
    }

    private int GetSecondZoneTotalCubesCount()
    {
        int cubesCount = 0; 

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject secondZoneCell = TagComponent.GetObjectByTag($"c{i + 10}r{j + 10}");

                if (secondZoneCell.transform.childCount > 1)
                {
                    cubesCount++;
                }
            }
        }

        return cubesCount;
    }

    public bool CheckCubes()
    {
        bool equalCell = false;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject firstZoneCell = TagComponent.GetObjectByTag($"c{i}r{j}");
                GameObject secondZoneCell = TagComponent.GetObjectByTag($"c{i+10}r{j+10}");

                int firstZoneCellChildCount = firstZoneCell.transform.childCount;
                int secondZoneCellChildCount = secondZoneCell.transform.childCount;

                if (firstZoneCellChildCount == secondZoneCellChildCount)
                {
                    equalCell = true;
                }
                else
                {
                    equalCell = false;
                    break;
                }
            }
        }

        return equalCell && (cubesCount == GetSecondZoneTotalCubesCount());
    }
}
