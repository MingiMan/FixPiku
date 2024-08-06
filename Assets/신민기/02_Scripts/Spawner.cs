using UnityEngine;

public class Spawner : MonoBehaviour
{
    Transform[] spawnPoint;

    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > 0.5f)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
       //  GameManager.Instance.pool.MonsterGet(1);
    }
}
