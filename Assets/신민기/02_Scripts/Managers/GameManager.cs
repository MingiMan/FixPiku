using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public AnimalSpawner animalSpawner;
    public WildMonsterSpawner wildMonsterSpawner;
    public MonsterSpawner monsterSpawner;

    public int level = 1;

    [Header("Monsters Per Level")]
    public int monstersPerLevel1 = 24;
    public int monstersPerLevel2 = 40;
    public int monstersPerLevel3 = 30;
    public int monstersPerLevel4 = 50;
    public int monstersPerLevel5 = 60;

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
        // wildMonsterSpawner = FindObjectOfType<WildMonsterSpawner>();
        if(monsterSpawner == null)
            monsterSpawner = FindObjectOfType<MonsterSpawner>();
    }

    public void GameStart()
    {
        OnMonsterSpawnForLevel();
    }

    public void LevelUp()
    {
        level++;
    }

    private void OnMonsterSpawnForLevel()
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
}
