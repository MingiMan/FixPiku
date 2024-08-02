using System.Collections;
using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    public int damage;
    public float speed;
    public float destroyTime;
    [SerializeField] ParticleSystem explosion;
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void FireBullet(Transform pos)
    {
        rigid.linearVelocity = pos.forward * speed;
        StartCoroutine(DestroyBullet());
    }


    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(destroyTime);
        explosion.Play();
        yield return new WaitForSeconds(0.5F);
        Destroy(gameObject);
    }
}
