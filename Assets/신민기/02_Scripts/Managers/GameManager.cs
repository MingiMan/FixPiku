using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<Transform> points = new List<Transform>();
    [Header("0번 돼지 1번 소 2번 양")]
    public GameObject[] animalPrefab;

    int animalCount;
    [SerializeField] int maxAnimalCount = 10;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);

        DontDestroyOnLoad(Instance);

    }

    private void Start()
    {
        GameObject.Find("AnimalPointGroup").GetComponentsInChildren<Transform>(points);
        points.RemoveAt(0);
        if (animalCount < maxAnimalCount)
        {
            InvokeRepeating(nameof(CreateAnimals), 2f, 3f);
        }

    }

    public void OnAnimalDeath()
    {
        animalCount--;
        if (animalCount < maxAnimalCount)
        {
            InvokeRepeating(nameof(CreateAnimals), 2f, 3f);
        }
    }

    void CreateAnimals()
    {
        if (animalCount >= maxAnimalCount)
        {
            CancelInvoke(nameof(CreateAnimals));
            return;
        }

        int index = Random.Range(0, points.Count);
        int animalIndex = Random.Range(0, animalPrefab.Length);

        Instantiate(animalPrefab[animalIndex], points[index].position, Quaternion.identity);
        animalCount++;

        if (animalCount >= maxAnimalCount)
        {
            CancelInvoke(nameof(CreateAnimals));
        }
    }
}
