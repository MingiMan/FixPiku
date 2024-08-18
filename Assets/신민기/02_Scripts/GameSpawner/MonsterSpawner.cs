using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    List<Transform> frontPoints = new List<Transform>();
    List<Transform> rightPoints = new List<Transform>();
    List<Transform> leftPoints = new List<Transform>();
    List<Transform> backPoints = new List<Transform>();
    [SerializeField]
    Transform frontSpawn;
    [SerializeField]
    Transform rightSpawn;

    [SerializeField]
    Transform leftSpawn;

    [SerializeField]
    Transform backSpawn;

    Queue<Transform> availableFrontPoints = new Queue<Transform>();
    Queue<Transform> availableRightPoints = new Queue<Transform>();
    Queue<Transform> availableLeftPoints = new Queue<Transform>();
    Queue<Transform> availableBackPoints = new Queue<Transform>();

    Dictionary<GameObject, Transform> monsterQueue = new Dictionary<GameObject, Transform>();

    int currentMonster;
    int count;

    float watingTime;

    private void Awake()
    {
        GameObject.Find("FrontPoint").GetComponentsInChildren<Transform>(frontPoints);
        GameObject.Find("RightPoint").GetComponentsInChildren<Transform>(rightPoints);
        GameObject.Find("LeftPoint").GetComponentsInChildren<Transform>(leftPoints);
        GameObject.Find("BackPoint").GetComponentsInChildren<Transform>(backPoints);
        frontPoints.RemoveAt(0); // 부모 객체를 제거
        rightPoints.RemoveAt(0); // 부모 객체를 제거
        leftPoints.RemoveAt(0); // 부모 객체를 제거
        backPoints.RemoveAt(0); // 부모 객체를 제거
        Initalize();
    }


    void Initalize()
    {
        availableFrontPoints.Clear();
        availableRightPoints.Clear();
        availableLeftPoints.Clear();
        availableBackPoints.Clear();

        foreach (var point in frontPoints)
            availableFrontPoints.Enqueue(point);

        foreach (var point in rightPoints)
            availableRightPoints.Enqueue(point);

        foreach (var point in leftPoints)
            availableLeftPoints.Enqueue(point);

        foreach (var point in backPoints)
            availableBackPoints.Enqueue(point);

        monsterQueue.Clear();
    }

    public void OnMonsterSpawn(int totalMonster)
    {
        Initalize();
        currentMonster = totalMonster;
        count = totalMonster;
        StartCoroutine(SpawnMonsters());
    }

    IEnumerator SpawnMonsters()
    {
        while (currentMonster > 0) // 몬스터가 남아 있는 동안에만 스폰
        {
            if (availableFrontPoints.Count > 0)
                SpawnMonsterAtPoint(availableFrontPoints, frontSpawn);

            yield return new WaitForSeconds(1.5f);

            if (availableRightPoints.Count > 0)
                SpawnMonsterAtPoint(availableRightPoints, rightSpawn);

            yield return new WaitForSeconds(3f);

            if (availableLeftPoints.Count > 0)
                SpawnMonsterAtPoint(availableLeftPoints, leftSpawn);
            yield return new WaitForSeconds(2f);

            if (availableBackPoints.Count > 0)
                SpawnMonsterAtPoint(availableBackPoints, backSpawn);
            yield return new WaitForSeconds(1.5f);

        }
    }

    void SpawnMonsterAtPoint(Queue<Transform> pointsQueue, Transform spawnPoint)
    {
        if (pointsQueue.Count == 0 || currentMonster <= 0) return; // 몬스터가 다 소환되면 스폰 중지

        GameObject monster = null;

        switch (GameManager.Instance.level)
        {
            // 레벨 1 늑대
            case 1:
             monster = PoolManager.Instance.MonsterGet(0);
                break;

                // 웨어 울프
            case 2:
                 monster = PoolManager.Instance.MonsterGet(2); 
                break;

            // 폭탄 몬스터
            case 3:
                monster = PoolManager.Instance.MonsterGet(3);
                break;

                // 늑대 웨어울프
            case 4:
                monster = PoolManager.Instance.MonsterGet(Random.Range(0,3));
                break;

                // 늑대 웨어울프 폭탄몬스터
            case 5:
                monster = PoolManager.Instance.MonsterGet(Random.Range(0, 4));
                break;

        }

        monster.transform.position = spawnPoint.position;
        Transform targetPoint = pointsQueue.Dequeue();
        monster.GetComponent<Monsters>().TargetSetDestination(targetPoint);
        monster.SetActive(true);

        monsterQueue[monster] = targetPoint;
        currentMonster--;
    }

    public void OnMonsterDeath(GameObject monster)
    {
        monster.SetActive(false);
        count--;

        if (monsterQueue.ContainsKey(monster))
        {
            Transform point = monsterQueue[monster];

            if (frontPoints.Contains(point))
                availableFrontPoints.Enqueue(point);

            else if (rightPoints.Contains(point))
                availableRightPoints.Enqueue(point);

            else if (leftPoints.Contains(point))
                availableLeftPoints.Enqueue(point);

            else if (backPoints.Contains(point))
                availableBackPoints.Enqueue(point);


            monsterQueue.Remove(monster);
        }

        if(count == 0)
            GameManager.Instance.LevelUp();

        if (currentMonster <= 0)
            return;

        // 몬스터 다시 스폰
        StartCoroutine(RespawnMonster(monster));
    }

    IEnumerator RespawnMonster(GameObject monster)
    {
        yield return new WaitForSeconds(3f);

        if (currentMonster <= 0) yield break; // 남은 몬스터가 없으면 스폰 중지

        if (availableFrontPoints.Count > 0)
        {
            SpawnMonsterAtPoint(availableFrontPoints, frontSpawn);
        }
        else if (availableRightPoints.Count > 0)
        {
            SpawnMonsterAtPoint(availableRightPoints, rightSpawn);
        }
        else if (availableLeftPoints.Count > 0)
        {
            SpawnMonsterAtPoint(availableLeftPoints, leftSpawn);
        }
        else if (availableBackPoints.Count > 0)
        {
            SpawnMonsterAtPoint(availableBackPoints, backSpawn);
        }
    }

    public void AllMonsterDeath()
    {
        foreach (var monster in monsterQueue.Keys)
        {
            monster.GetComponent<Monsters>().Dead();
        }

        GameManager.Instance.LevelUp();
        Initalize();

        currentMonster = 0;
        count = 0;
    }
}
