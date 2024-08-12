using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;

public class SnakeSpawner : MonoBehaviour
{
    [Header("Snake")]
    List<Transform> snakePoint = new List<Transform>();
    [SerializeField] int snakeMaxCount;
    public int snakeCurrentCount = 0;
    float snakeTimer = 0;

    void Awake()
    {
        GameObject.Find("SnakeSpawn").GetComponentsInChildren<Transform>(snakePoint);
         snakePoint.RemoveAt(0);
    }

    private void Update()
    {
        CreateSnake();
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


    public void SnakeDeath(GameObject snake)
    {
        snake.SetActive(false);
        snakeCurrentCount--;
        snakeTimer = 0;
    }
}
