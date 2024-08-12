using System.Collections;
using UnityEngine;

public class Spike : Animals
{
    [SerializeField] BoxCollider meleeArea;
    [SerializeField] Transform point;
    [SerializeField] float targetRadius = 0.5f;
    bool IsAttack;
    bool IsActive;
    public bool OnGizmos;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        transform.position = point.position;
        nav.speed = Stats.runSpeed;
        rigid.isKinematic = false;
        meleeArea.enabled = false;
        Initialize();
    }

    protected override void Initialize()
    {
        Invisible = false;
        IsActive = false;
        IsRunning = false;
        IsAttack = false;
        nav.isStopped = true;
        nav.ResetPath();
    }

    protected override void Update()
    {
        if (!IsDead)
            PlayerDetection();
    }

    protected override void FixedUpdate()
    {
        if (!IsDead)
            Move();
    }

    protected override void Move()
    {
        if (IsActive && IsRunning)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTr.transform.position);
            nav.SetDestination(playerTr.transform.position);

            if (!IsAttack && distanceToPlayer <= nav.stoppingDistance)
            {
                StartCoroutine(Attack());
            }

            else if (Vector3.Distance(transform.position, playerTr.transform.position) > 15f)
                Initialize();
        }
    }

    IEnumerator Attack()
    {
        IsAttack = true;
        IsRunning = false;
        nav.isStopped = true;
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("OnAttack");
        yield return new WaitForSeconds(1f);
        nav.isStopped = false;
        meleeArea.enabled = false;
        IsRunning = true;
        IsAttack = false;
    }

    public void EnableCollider()
    {
        meleeArea.enabled = true;
    }

    protected void PlayerDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, targetRadius, LayerMask.GetMask("PLAYER"));
        if (colliders.Length > 0 && !IsActive)
        {
            StartCoroutine(SpikeAcitve());
        }
    }

    IEnumerator SpikeAcitve()
    {
        animator.SetTrigger("OnActive");
        yield return new WaitForSeconds(0.5f);
        IsRunning = true;
        IsActive = true;
        nav.isStopped = false;
    }
    
    public override void Damage(int _dmg, Vector3 _tarGetPos)
    {
        base.Damage(_dmg, _tarGetPos);

        if (!IsDead)
        {
            IsActive = true;
            IsRunning = true;
        }

    }
    public override void Dead()
    {
        StopAllCoroutines();
        mat.color = Color.red;
        nav.isStopped = true;
        IsRunning = false;
        IsDead = true;

        if (dead_Sound != null)
            PlaySE(dead_Sound);

        gameObject.layer = 6;
        gameObject.tag = "Untagged";
        animator.SetTrigger("OnDie");
        rigid.isKinematic = false;
        StartCoroutine(SpikeDeath());
    }

    IEnumerator SpikeDeath()
    {
        yield return new WaitForSeconds(3f);
        Instantiate(deadDustParticle, transform.position, transform.rotation);
        gameObject.SetActive(false);
        GameManager.Instance.ReactivateSpike(gameObject, 10f);
    }

    protected override void OnAnimatorMove()
    {
        animator.SetBool("IsRunning", IsRunning);
        animator.SetBool("IsActive", IsActive);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().OnDamage(Stats.atkDamage);
        }
    }
    private void OnDrawGizmos()
    {
        if (OnGizmos)
            Gizmos.DrawSphere(transform.position, targetRadius);
    }
}
