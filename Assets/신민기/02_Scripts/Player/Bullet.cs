using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    Rigidbody rigid;
    public float speed;
    public float destroyTime;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Destroy(this.gameObject, destroyTime);
    }

    public void FireBullet(Transform pos)
    {
        rigid.linearVelocity = pos.forward*speed;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ENEMY"))
        {
            other.gameObject.GetComponent<Enemys>().Damage(damage,transform.position);
            Destroy(gameObject);
        }
    }

}
