using UnityEngine;

public class AnimalExplosion : MonoBehaviour
{
    Rigidbody rigid;
    public float forceMagnitude = 20f;

    [Header("체크는 왼쪽 언체크는 오른쪽")]
    public bool LeftOrRight;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rigid.isKinematic = false;
    }

    private void Start()
    {
        if (LeftOrRight)
        {
            Vector3 forceDirection = Vector3.up + Vector3.right;
            rigid.AddForce(forceDirection.normalized * forceMagnitude, ForceMode.Impulse);
            rigid.AddTorque(forceDirection* forceMagnitude, ForceMode.Impulse);
        }
        else
        {
            Vector3 forceDirection = Vector3.up + Vector3.left;
            rigid.AddForce(forceDirection.normalized * forceMagnitude, ForceMode.Impulse);
            rigid.AddTorque(forceDirection * forceMagnitude, ForceMode.Impulse);
        }

        Invoke(nameof(AnimalStop),2.3f);
    }

    void AnimalStop()
    {
        rigid.isKinematic = true;
    }
}
