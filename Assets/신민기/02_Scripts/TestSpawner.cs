using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TestSpawner : MonoBehaviour
{
    List<Transform> fronPoints = new List<Transform>();
    List<Transform> rightPoints = new List<Transform>();
    
    private void Awake()
    {
        GameObject.Find("FrontPoint").GetComponentsInChildren<Transform>(fronPoints);
        GameObject.Find("RightPoint").GetComponentsInChildren<Transform>(rightPoints);
        fronPoints.RemoveAt(0);
        rightPoints.RemoveAt(0);
    }
}
