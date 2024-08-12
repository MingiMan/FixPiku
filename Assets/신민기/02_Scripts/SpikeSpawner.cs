using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpikeSpawner : MonoBehaviour
{
    // 66 6 80
    // 38 4 77 
    // 61 6 60 
    // 63 4 94
    [Header("Spike")]
    [SerializeField] Transform point;
    [SerializeField] int spikeMaxCount;
    public int spikeCurrentCount = 0;
    float spikeTimer = 0;

    private void Start()
    {
        GameObject spike = PoolManager.Instance.WildMonsterGet(1);
        spike.transform.position = point.transform.position;
    }
}
