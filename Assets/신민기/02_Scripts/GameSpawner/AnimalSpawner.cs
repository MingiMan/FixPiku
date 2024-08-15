using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AnimalSpawner : MonoBehaviour
{
    [SerializeField] int maxCount;

    List<Transform> points = new List<Transform>();

    public int currentCount = 0;

    float timer = 0;

    int animalCount;

    private void Awake()
    {
        GameObject.Find("AnimalSpawner").GetComponentsInChildren<Transform>(points);
        points.RemoveAt(0);
    }

    private void Start()
    {
        animalCount = PoolManager.Instance.animalsPrefab.Length;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer > 3f && currentCount <= maxCount)
        {
            timer = 0;
            AnimalSpawn();
            currentCount++;
        }
    }

    void AnimalSpawn()
    {
        GameObject animal = PoolManager.Instance.AnimalGet(Random.Range(0, animalCount));
        animal.transform.position = points[Random.Range(0, points.Count)].position;
    }

    public void AnimalDeath(GameObject animal)
    {
        animal.SetActive(false);
        currentCount--;
        timer = 0;
    }
}
