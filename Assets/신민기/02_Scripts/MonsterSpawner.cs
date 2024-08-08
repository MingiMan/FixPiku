using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("정면 기준 ")]
    List<Transform> points = new List<Transform>();


    // Level 1 늑대 등장 마리 수 20마리

    // Level 2 
    float timer;


    void MonsterSpawn()
    {
        // GameObject monster = PoolManager.Instance.AnimalGet(Random.Range(0, animalCount));
        // animal.transform.position = points[Random.Range(0, points.Count)].position;
    }


}
