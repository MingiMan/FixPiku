using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [Header("0 : 울프 1: 웨어울프")]
    public GameObject[] monstersPrefab;
    List<GameObject>[] monstersPool;

    [Header("0번 돼지 1번 소 2번 양")]
    public GameObject[] animalsPrefab;
    List<GameObject>[] animalPool;

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
        monstersPool = new List<GameObject>[monstersPrefab.Length];

        for (int index = 0; index < monstersPool.Length; index++)
            monstersPool[index] = new List<GameObject>();

        animalPool = new List<GameObject>[animalsPrefab.Length];
        for (int index = 0; index < animalsPrefab.Length; index++)
            animalPool[index] = new List<GameObject>();
    }

    public GameObject MonsterGet(int index)
    {
        GameObject select = null;
        foreach(GameObject monster in monstersPool[index])
        {
            if (!monster.activeSelf)
            {
                select = monster;
                select.SetActive(true);
                break;
            }
        }

        if(select == null)
        {
            select = Instantiate(monstersPrefab[index],transform);
            monstersPool[index].Add(select);
        }
        return select;
    }

    public GameObject AnimalGet(int index)
    {
        GameObject select = null;
        foreach (GameObject animal in animalPool[index])
        {
            if (!animal.activeSelf)
            {
                select = animal;
                select.SetActive(true);
                break;
            }
        }

        if (select == null)
        {
            select = Instantiate(animalsPrefab[index], transform);
            animalPool[index].Add(select);
        }
        return select;
    }
}
