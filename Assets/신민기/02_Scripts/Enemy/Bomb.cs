using UnityEngine;

public class Bomb : Monsters
{
    [SerializeField] ParticleSystem explosion;
    bool IsExplosion;
    public float explosionTime;
    float explosionTimer;

    protected override void OnEnable()
    {
        currentHp = Stats.health;
        nav.speed = Stats.runSpeed;
        nav.isStopped = false;
        IsExplosion = false;
        IsRunning = true;
        IsAction = true;
        nav.autoTraverseOffMeshLink = false;
        mat.color = Color.white;
        Invisible = false;
        IsDead = false;
        rigid.isKinematic = false;
        IsPlayerTarget = false;
        explosionTimer = 0;
        gameObject.layer = 8;
        gameObject.tag = "ENEMY";
        StartCoroutine(CheckOffMeshLink());
    }

    protected override void Update()
    {
        if (!IsDead)
        {
            PlayerDetection();
            Explosion();
        }
    }

    protected override void FixedUpdate()
    {
        if (!IsDead)
            Move();
    }

    protected override void Move()
    {
        if (!IsPlayerTarget && IsRunning)
        {
            nav.SetDestination(targetPoint.position);
            float distanceToHouse = Vector3.Distance(transform.position, targetPoint.transform.position);
            if (distanceToHouse <= nav.stoppingDistance)
            {
                nav.velocity = Vector3.zero;
                nav.isStopped = true;
                IsExplosion = true;
            }
        }

        if (IsPlayerTarget)
        {
            nav.isStopped = false;
            float distanceToPlayer = Vector3.Distance(transform.position, playerTr.transform.position);
            nav.SetDestination(playerTr.transform.position);
            if (distanceToPlayer <= nav.stoppingDistance)
            {
                IsExplosion = true;
            }
        }
    }


    void Explosion()
    {
        if (IsExplosion)
        {
            explosionTimer += Time.deltaTime;
            float lerp = Mathf.PingPong(explosionTimer * 2, 1f);
            mat.color = Color.Lerp(Color.white, Color.red, lerp);
            if (explosionTimer >= explosionTime)
            {
                Instantiate(explosion, transform.position, transform.rotation);
                Collider[] colliders = Physics.OverlapSphere(transform.position, targetRadius);
                foreach(Collider nearObject in colliders)
                {
                    if (nearObject.CompareTag("Player"))
                    {
                        if(nearObject != null)
                            nearObject.GetComponent<PlayerMovement>().OnDamage(Stats.atkDamage);
                    }
                    if (nearObject.CompareTag("HOUSE"))
                    {
                        // House
                    }
                }
                GameManager.Instance.monsterSpawner.OnMonsterDeath(this.gameObject);
            }
        }
    }
    public override void Dead()
    {
        IsExplosion = false;
        base.Dead();
    }

    protected override void OnAnimatorMove()
    {
        animator.SetBool("IsRunning", IsRunning);
        animator.SetBool("IsExplosion", IsExplosion);
    }
}
