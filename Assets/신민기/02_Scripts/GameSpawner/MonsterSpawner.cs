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
        frontPoints.RemoveAt(0); // �θ� ��ü�� ����
        rightPoints.RemoveAt(0); // �θ� ��ü�� ����
        leftPoints.RemoveAt(0); // �θ� ��ü�� ����
        backPoints.RemoveAt(0); // �θ� ��ü�� ����
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
        while (currentMonster > 0) // ���Ͱ� ���� �ִ� ���ȿ��� ����
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
        if (pointsQueue.Count == 0 || currentMonster <= 0) return; // ���Ͱ� �� ��ȯ�Ǹ� ���� ����

        GameObject monster = null;

        switch (GameManager.Instance.level)
        {
            // ���� 1 ����
            case 1:
             monster = PoolManager.Instance.MonsterGet(0);
                break;

                // ���� ����
            case 2:
                 monster = PoolManager.Instance.MonsterGet(2); 
                break;

            // ��ź ����
            case 3:
                monster = PoolManager.Instance.MonsterGet(3);
                break;

                // ���� �������
            case 4:
                monster = PoolManager.Instance.MonsterGet(Random.Range(0,3));
                break;

                // ���� ������� ��ź����
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

        // ���� �ٽ� ����
        StartCoroutine(RespawnMonster(monster));
    }

    IEnumerator RespawnMonster(GameObject monster)
    {
        yield return new WaitForSeconds(3f);

        if (currentMonster <= 0) yield break; // ���� ���Ͱ� ������ ���� ����

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
