using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [Header("0 : 울프 1: 웨어울프")]
    public GameObject[] monstersPrefab;
    List<GameObject>[] monstersPool;

    [Header("0번 돼지 1번 소 2번 양")]
    public GameObject[] animalsPrefab;
    List<GameObject>[] animalPool;

    [Header("0번 뱀  1번 스파이크")]
    public GameObject[] wildMonsterPrefab;
    List<GameObject>[] wildMonsterPool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
            Destroy(Instance);


        monstersPool = new List<GameObject>[monstersPrefab.Length];

        for (int index = 0; index < monstersPool.Length; index++)
            monstersPool[index] = new List<GameObject>();

        animalPool = new List<GameObject>[animalsPrefab.Length];
        for (int index = 0; index < animalsPrefab.Length; index++)
            animalPool[index] = new List<GameObject>();


        wildMonsterPool = new List<GameObject>[wildMonsterPrefab.Length];
        for (int index = 0; index < wildMonsterPrefab.Length; index++)
            wildMonsterPool[index] = new List<GameObject>();

    }


    public GameObject MonsterGet(int index)
    {
        GameObject select = null;
        foreach (GameObject monster in monstersPool[index])
        {
            if (!monster.activeSelf)
            {
                select = monster;
                select.SetActive(true);
                break;
            }
        }

        if (select == null)
        {
            select = Instantiate(monstersPrefab[index], transform);
            monstersPool[index].Add(select);
        }
        return select;
    }

    public GameObject WildMonsterGet(int index)
    {
        GameObject select = null;
        foreach (GameObject wildMonster in wildMonsterPool[index])
        {
            if (!wildMonster.activeSelf)
            {
                select = wildMonster;
                select.SetActive(true);
                break;
            }
        }

        if (select == null)
        {
            select = Instantiate(wildMonsterPrefab[index], transform);
            wildMonsterPool[index].Add(select);
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

    public void MonsterDeath(GameObject monster)
    {
        monster.SetActive(false);
    }
}
