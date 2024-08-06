using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public AnimalSpawner animalSpawner;

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
    }
}
