using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public AnimalSpawner animalSpawner;
    public WildMonsterSpawner wildMonsterSpawner;
    public MonsterSpawner monsterSpawner;

    public int level = 1;

    [Header("Monsters Per Level")]
    public int monstersPerLevel1 = 24; // 울프 
    public int monstersPerLevel2 = 12; // 웨어 울프
    public int monstersPerLevel3 = 30; // 폭탄맨
    public int monstersPerLevel4 = 50; // 울프 웨어 울프
    public int monstersPerLevel5 = 60; // 울프 웨어울프 폭탄맨

    [Space(10)]
    [Header("UI")]
    [SerializeField] TextMeshProUGUI waringUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
            Destroy(Instance);
    }

    private void Start()
    {
        animalSpawner = FindObjectOfType<AnimalSpawner>();
        wildMonsterSpawner = FindObjectOfType<WildMonsterSpawner>();
        if(monsterSpawner == null)
            monsterSpawner = FindObjectOfType<MonsterSpawner>();

        waringUI.gameObject.SetActive(false);

        // dayText.gameObject.SetActive(true);
    }

    public void GameStart()
    {
        OnMonsterSpawnForLevel();
    }

    public void LevelUp()
    {
        level++;
    }

    public void OnMonsterSpawnForLevel()
    {
        switch (level)
        {
            case 1:
                monsterSpawner.OnMonsterSpawn(monstersPerLevel1);
                break;
            case 2:
                monsterSpawner.OnMonsterSpawn(monstersPerLevel2);
                break;
            case 3:
                monsterSpawner.OnMonsterSpawn(monstersPerLevel3);
                break;
            case 4:
                monsterSpawner.OnMonsterSpawn(monstersPerLevel4);
                break;
            case 5:
                monsterSpawner.OnMonsterSpawn(monstersPerLevel5);
                break;
            default:
                Debug.Log("게임 종료"); // 모든 레벨 완료 시
                break;
        }
    }

    public void WaringUISetAcitve()
    {
        StartCoroutine(WaringUICoroutine());
    }

    IEnumerator WaringUICoroutine()
    {
        waringUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        waringUI.gameObject.SetActive(false);
    }
}
