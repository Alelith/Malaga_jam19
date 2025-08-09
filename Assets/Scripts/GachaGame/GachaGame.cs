using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Random = UnityEngine.Random;

public class GachaGame : MonoBehaviour {

    [SerializeField]
    [Tooltip("The spawn points for the objects")]
    Transform[] spawnPoints;

    /// <summary>
    /// Maximum number of objects on screen.
    /// </summary>
    int maxObjectsOnScreen = 3;

    /// <summary>
    /// Current number of objects on screen.
    /// </summary>
    int currentObjectsOnScreen = 0;

    [SerializeField]
    [Tooltip("Minimum spawn time for the objects.")]
    float minSpawnTime = 0.4f, maxSpawnTime = 1.11f;

    [SerializeField]
    [Tooltip("Prefab of the object to spawn.")]
    GameObject counter, score;

    [SerializeField]
    [Tooltip("Array of prefabs for the objects to spawn.")]
    GameObject[] objectPrefabs;

    // Update is called once per frame
    void Update() {
        StartCoroutine(WaitForSpawn());
    }

    int lastSpawnPosition = -1;
    IEnumerator WaitForSpawn() {

      if (currentObjectsOnScreen < maxObjectsOnScreen) {
        currentObjectsOnScreen++;
        float spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        yield return new WaitForSeconds(spawnTime);

        int newSpawnPosition;
        do{
            newSpawnPosition = Random.Range(0, 5);
        }while(newSpawnPosition == lastSpawnPosition);

        Instantiate(objectPrefabs[Random.Range(0, objectPrefabs.Length)], spawnPoints[newSpawnPosition]);
        lastSpawnPosition = newSpawnPosition;
        currentObjectsOnScreen--;
      }
    }

}
