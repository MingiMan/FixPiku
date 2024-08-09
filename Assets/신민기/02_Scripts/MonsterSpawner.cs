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

    private void Awake()
    {
        GameObject.Find("FrontPoint").GetComponentsInChildren<Transform>(frontPoints);
        GameObject.Find("RightPoint").GetComponentsInChildren<Transform>(rightPoints);
        frontPoints.RemoveAt(0);
        rightPoints.RemoveAt(0);
        Initalize();
    }

    void Initalize()
    {
        foreach (var point in frontPoints)
            availableFrontPoints.Enqueue(point);
        foreach (var point in rightPoints)
            availableRightPoints.Enqueue(point);
    }

    private void Start()
    {
        StartCoroutine(SpawnMonsters());
    }

    IEnumerator SpawnMonsters()
    {

        if (availableFrontPoints.Count > 0)
        {
            SpawnMonsterAtPoint(availableFrontPoints, frontSpawn);
        }
        if (availableRightPoints.Count > 0)
        {
            SpawnMonsterAtPoint(availableRightPoints, rightSpawn);
        }
        yield return new WaitForSeconds(1f);

    }

    void SpawnMonsterAtPoint(Queue<Transform> pointsQueue, Transform spawnPoint)
    {
        if (pointsQueue.Count == 0) return;

        GameObject monster = PoolManager.Instance.MonsterGet(0);
        monster.transform.position = spawnPoint.position;

        Transform targetPoint = pointsQueue.Dequeue();
        monster.GetComponent<Monsters>().TargetSetDestination(targetPoint);
        monster.SetActive(true);

        monsterQueue[monster] = targetPoint;
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
        StartCoroutine(RespawnMonster(monster));
    }

    IEnumerator RespawnMonster(GameObject monster)
    {
        yield return new WaitForSeconds(3f);

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
