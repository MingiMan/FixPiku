using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;

public class WildMonsterSpawner : MonoBehaviour
{
    [Header("Snake")]
    List<Transform> snakePoint = new List<Transform>();
    [SerializeField] int snakeMaxCount;
    public int snakeCurrentCount = 0;
    int snakeCount;
    float snakeTimer = 0;

    [Header("Spike")]
    List<Transform>  spikePoint = new List<Transform>();
    [SerializeField] int spikeMaxCount;
    public int spikeCurrentCount = 0;
    int spikeCount;
    float spikeTimer = 0;
    int spikePointIndex = 0;
    HashSet<int> usedSpikePoints = new HashSet<int>();

    void Awake()
    {
        GameObject.Find("SnakeSpawn").GetComponentsInChildren<Transform>(snakePoint);
       // GameObject.Find("SpikeSpawn").GetComponentsInChildren<Transform>(spikePoint);
         snakePoint.RemoveAt(0);
       // spikePoint.RemoveAt(0);
    }

    private void Update()
    {
       //   CreateSpike();
        CreateSnake();
    }

    private void CreateSpike()
    {
        spikeTimer += Time.deltaTime;

        if (spikeTimer > 5f && spikeCurrentCount <= spikeMaxCount)
        {
            spikeTimer = 0f;
            SpikeSpawn();
            spikeCurrentCount++;
        }
    }

    void CreateSnake()
    {
        snakeTimer += Time.deltaTime;

        if (snakeTimer > 10f && snakeCurrentCount <= snakeMaxCount)
        {
            snakeTimer = 0f;
            SnakeSpawn();
            snakeCurrentCount++;
        }
    }

    void SnakeSpawn()
    {
        GameObject snake = PoolManager.Instance.WildMonsterGet(0);
        snake.transform.position = snakePoint[Random.Range(0, snakePoint.Count)].position;
    }

    void SpikeSpawn()
    {
        if (usedSpikePoints.Count >= spikePoint.Count)
        {
            Debug.LogWarning("All spike points are currently in use.");
            return;
        }

        int startIndex = spikePointIndex;
        do
        {
            spikePointIndex = (spikePointIndex + 1) % spikePoint.Count;
        } while (usedSpikePoints.Contains(spikePointIndex) && spikePointIndex != startIndex);

        if (!usedSpikePoints.Contains(spikePointIndex))
        {
            GameObject spike = PoolManager.Instance.WildMonsterGet(1);
            spike.transform.position = spikePoint[spikePointIndex].position;
            usedSpikePoints.Add(spikePointIndex);
        }
    }

    public void SpikeDeath(GameObject spike)
    {
        int pointIndex = spikePoint.FindIndex(t => t.position == spike.transform.position);
        if (pointIndex >= 0)
        {
            usedSpikePoints.Remove(pointIndex);
        }

        spike.SetActive(false);
        spikeCurrentCount--;
        spikeTimer = 0;
    }

    public void SnakeDeath(GameObject snake)
    {
        snake.SetActive(false);
        snakeCurrentCount--;
        snakeTimer = 0;
    }
}
