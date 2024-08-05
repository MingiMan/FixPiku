using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public Color color = Color.yellow;
    public float radius = 0.1f;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);  
    }
}
