using System.Collections;
using System.Runtime.Serialization;
using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    public int damage;
    public float speed;
    public float destroyTime;
    public string cannonExplosion;
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
        SoundManager.instance.PlaySFX(cannonExplosion);
        Destroy(this.gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ENEMY"))
        {
            StartCoroutine(Explosion());
        }
    }

    IEnumerator Explosion()
    {
        rigid.isKinematic = true;
        Instantiate(explosionParticle,transform.position,transform.rotation);
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 4f, Vector3.up, 0f, LayerMask.GetMask("ENEMY"));
        foreach(RaycastHit hitObj in rayHits)
        {
            // hitObj.transform.GetComponent<Enemys>().Damage(damage,transform.position);
            hitObj.transform.GetComponent<Enemys>().Dead();
        }
        SoundManager.instance.PlaySFX(cannonExplosion);
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }
}
