using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleManager : MonoBehaviour

{
    // ABSTRACTION
    public GameObject[] holePrefabs;
    public GameObject[] shapePrefabs;
    public Transform spawnArea;
    public float spawnInterval = 2f;

    private List<GameObject> spawnedHoles = new List<GameObject>();
    private GameObject currentShape;

    private static HoleManager instance;

    private void Start()
    {
        instance = this;
        SpawnInitialHoles();
        SpawnShape();
    }
    // ABSTRACTION
    private void SpawnInitialHoles()
    {
        SpawnHole(TriangleHolePrefab(), Quaternion.Euler(90f, 25f, 90f), 4f);
        SpawnHole(SquareHolePrefab(), Quaternion.identity, 4f);
        SpawnHole(CircleHolePrefab(), Quaternion.identity, 4f);

        int remainingHoles = 5 - spawnedHoles.Count;
        for (int i = 0; i < remainingHoles; i++)
        {
            SpawnRandomHole();
        }
    }

    // ABSTRACTION
    private void SpawnRandomHole()
    {
        int randomIndex = Random.Range(0, holePrefabs.Length);
        GameObject holePrefab = holePrefabs[randomIndex];

        Quaternion rotation = Quaternion.identity;
        if (holePrefab.CompareTag("TriangleHole"))
        {
            rotation = Quaternion.Euler(90f, 25f, 90f);
        }

        Vector3 spawnPosition = GetRandomSpawnPosition(100f, 10f); // Aumentar la distancia mínima a 6 unidades
        GameObject holeObject = Instantiate(holePrefab, spawnPosition, rotation);
        spawnedHoles.Add(holeObject);
    }

    // ABSTRACTION
    private bool IsOccupied(Vector3 position)
    {
        foreach (GameObject hole in spawnedHoles)
        {
            if (Vector3.Distance(position, hole.transform.position) < 4f)
            {
                return true;
            }
        }
        return false;
    }

    // ABSTRACTION
    private void SpawnHole(GameObject holePrefab, Quaternion rotation, float minDistance)
    {
        Vector3 spawnPosition = GetRandomSpawnPosition(100f, minDistance);
        while (IsOccupied(spawnPosition))
        {
            spawnPosition = GetRandomSpawnPosition(100f, minDistance);
        }

        GameObject holeObject = Instantiate(holePrefab, spawnPosition, rotation);
        spawnedHoles.Add(holeObject);
    }

    // ABSTRACTION
    public void SpawnShape()
    {
        if (currentShape != null)
        {
            Destroy(currentShape);
        }

        int randomIndex = Random.Range(0, shapePrefabs.Length);
        GameObject shapePrefab = shapePrefabs[randomIndex];

        if (shapePrefab.CompareTag("Triangle"))
        {
            currentShape = Instantiate(shapePrefab, GetRandomSpawnPosition(101f, 4f), Quaternion.Euler(90f, 25f, 90f));
        }
        else
        {
            currentShape = Instantiate(shapePrefab, GetRandomSpawnPosition(100f, 4f), Quaternion.identity);
        }

        bool hasMatchingHole = false;
        foreach (GameObject hole in spawnedHoles)
        {
            if (hole.CompareTag(shapePrefab.tag))
            {
                hasMatchingHole = true;
                break;
            }
        }

        if (!hasMatchingHole)
        {
            GameObject holePrefab = GetRandomHolePrefab(shapePrefab.tag);
            if (holePrefab != null)
            {
                Quaternion rotation = Quaternion.identity;
                if (holePrefab.CompareTag("TriangleHole"))
                {
                    rotation = Quaternion.Euler(90f, 25f, 90f);
                }

                SpawnHole(holePrefab, rotation, 4f);
            }
        }
    }

    // ABSTRACTION
    private GameObject GetRandomHolePrefab(string shapeTag)
    {
        List<GameObject> matchingHolePrefabs = new List<GameObject>();
        foreach (GameObject holePrefab in holePrefabs)
        {
            if (holePrefab.CompareTag(shapeTag))
            {
                matchingHolePrefabs.Add(holePrefab);
            }
        }

        if (matchingHolePrefabs.Count > 0)
        {
            int randomIndex = Random.Range(0, matchingHolePrefabs.Count);
            return matchingHolePrefabs[randomIndex];
        }

        return null;
    }

    // ABSTRACTION
    private Vector3 GetRandomSpawnPosition(float y, float minDistance)
    {
        Vector3 randomPoint = Random.insideUnitCircle * spawnArea.localScale.x / 2f;
        Vector3 spawnPosition = spawnArea.position + new Vector3(randomPoint.x, 0f, randomPoint.y);
        spawnPosition.y = y;

        // Verificar la distancia mínima con los agujeros existentes
        foreach (GameObject hole in spawnedHoles)
        {
            if (Vector3.Distance(spawnPosition, hole.transform.position) < minDistance)
            {
                return GetRandomSpawnPosition(y, minDistance); // Intentar nuevamente si la distancia no es suficiente
            }
        }

        return spawnPosition;
    }

    private GameObject TriangleHolePrefab()
    {
        return FindHolePrefab(typeof(TriangleHole));
    }

    private GameObject SquareHolePrefab()
    {
        return FindHolePrefab(typeof(SquareHole));
    }

    private GameObject CircleHolePrefab()
    {
        return FindHolePrefab(typeof(CircleHole));
    }

    private GameObject FindHolePrefab(System.Type holeType)
    {
        foreach (var prefab in holePrefabs)
        {
            if (prefab.GetComponent(holeType) != null)
            {
                return prefab;
            }
        }
        return null;
    }

  
    public static HoleManager GetInstance()
    {
        return instance;
    }

   
    public void SetShouldSpawnHole()
    {
        StartCoroutine(SpawnHoleAfterDelay());
    }

    private IEnumerator SpawnHoleAfterDelay()
    {
        yield return new WaitForSeconds(spawnInterval);
        SpawnRandomHole();
    }

   
    public void ResetHoles()
    {
        foreach (GameObject holeObject in spawnedHoles)
        {
            Destroy(holeObject);
        }
        spawnedHoles.Clear();

        SpawnInitialHoles();
    }
}