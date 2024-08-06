using System.Collections;
using System.Runtime.Serialization;
using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    public int damage;
    public float speed;
    public float destroyTime;
    [SerializeField] ParticleSystem explosionParticle;
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Invoke(nameof(DestroyGameObject),2f);
    }

    public void FireBullet(Transform pos)
    {
        rigid.linearVelocity = pos.forward * speed;
    }

    private void DestroyGameObject()
    {
        rigid.isKinematic = true;
        Instantiate(explosionParticle, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ENEMY"))
        {
            StopAllCoroutines();
            other.GetComponent<Enemys>().Damage(damage, transform.position);
            rigid.isKinematic = true;
            Instantiate(explosionParticle,transform.position,transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
