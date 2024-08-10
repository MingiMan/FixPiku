using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    List<Transform> frontPoints = new List<Transform>();
    List<Transform> rightPoints = new List<Transform>();
    [SerializeField]
    Transform frontSpawn;
    [SerializeField]
    Transform rightSpawn;

    Queue<Transform> availableFrontPoints = new Queue<Transform>();
    Queue<Transform> availableRightPoints = new Queue<Transform>();
    Dictionary<GameObject, Transform> monsterQueue = new Dictionary<GameObject, Transform>();

    int currentMonster;
    int totalMonster;

    private void Awake()
    {
        GameObject.Find("FrontPoint").GetComponentsInChildren<Transform>(frontPoints);
        GameObject.Find("RightPoint").GetComponentsInChildren<Transform>(rightPoints);
        frontPoints.RemoveAt(0); // 부모 객체를 제거
        rightPoints.RemoveAt(0); // 부모 객체를 제거
        Initalize();
    }

    void Initalize()
    {
        foreach (var point in frontPoints)
            availableFrontPoints.Enqueue(point);
        foreach (var point in rightPoints)
            availableRightPoints.Enqueue(point);
    }

    public void GameStart()
    {
        currentMonster = totalMonster;
        StartCoroutine(SpawnMonsters());
    }

    IEnumerator SpawnMonsters()
    {
        while (currentMonster > 0) // 몬스터가 남아 있는 동안에만 스폰
        {
            if (availableFrontPoints.Count > 0)
                SpawnMonsterAtPoint(availableFrontPoints, frontSpawn);

            if (availableRightPoints.Count > 0)
                SpawnMonsterAtPoint(availableRightPoints, rightSpawn);

            yield return new WaitForSeconds(1.5f);
        }
    }

    void SpawnMonsterAtPoint(Queue<Transform> pointsQueue, Transform spawnPoint)
    {
        if (pointsQueue.Count == 0 || currentMonster <= 0) return; // 몬스터가 다 소환되면 스폰 중지

        GameObject monster = PoolManager.Instance.MonsterGet(0);
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

        if (monsterQueue.ContainsKey(monster))
        {
            Transform point = monsterQueue[monster];

            if (frontPoints.Contains(point))
            {
                availableFrontPoints.Enqueue(point);
            }
            else if (rightPoints.Contains(point))
            {
                availableRightPoints.Enqueue(point);
            }

            monsterQueue.Remove(monster);
        }

        if (currentMonster <= 0) return; // 더 이상 몬스터가 남아 있지 않으면 스폰 중지

        // 몬스터 다시 스폰
        StartCoroutine(RespawnMonster(monster));
    }

    IEnumerator RespawnMonster(GameObject monster)
    {
        yield return new WaitForSeconds(1.5f);

        if (currentMonster <= 0) yield break; // 남은 몬스터가 없으면 스폰 중지

        if (availableFrontPoints.Count > 0)
        {
            SpawnMonsterAtPoint(availableFrontPoints, frontSpawn);
        }
        else if (availableRightPoints.Count > 0)
        {
            SpawnMonsterAtPoint(availableRightPoints, rightSpawn);
        }
    }
}
