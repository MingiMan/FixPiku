using NUnit.Framework;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

// 밤에만 나오는 몬스터들은 이 스크립트를 적용시켜준다.
public class Monsters : Enemys
{
    [SerializeField] protected BoxCollider meleeArea;
    [SerializeField] protected float targetRadius;
    protected Transform houseTr;
    protected Transform targetPoint;
    bool IsAttack;
    bool IsTargeting;
    bool IsPlayerTarget;
    public bool OnGizmos;

    protected override void Awake()
    {
        base.Awake();
        houseTr = GameObject.FindGameObjectWithTag("HOUSE").transform;
    }

    protected override void OnEnable()
    {
        currentHp = Stats.health;
        nav.speed = Stats.runSpeed;
        IsRunning = false;
        IsAction = true;
        nav.autoTraverseOffMeshLink = false;
        mat.color = Color.white;
        Invisible = false;
        IsDead = false;
        rigid.isKinematic = false;
        IsPlayerTarget = false;
        IsTargeting = false;
        gameObject.layer = 8;
        gameObject.tag = "ENEMY";
        StartCoroutine(CheckOffMeshLink());
    }

    protected override void Update()
    {
        if (!IsDead && targetPoint != null)
        {
            FreezeRotation();
            PlayerDetection();
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
            float distanceToHouse= Vector3.Distance(transform.position, targetPoint.transform.position);
            if(distanceToHouse <= nav.stoppingDistance)
            {
                IsTargeting = true;
                Quaternion targetRotation = Quaternion.LookRotation(houseTr.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20f);
                StartCoroutine(CoroutineAttack());
            }
        }
        if (IsPlayerTarget)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTr.transform.position);
            nav.SetDestination(playerTr.transform.position);
            if (!IsAttack && distanceToPlayer <= nav.stoppingDistance)
            {
                Quaternion targetRotation = Quaternion.LookRotation(playerTr.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 50f);
                StartCoroutine(CoroutineAttack());
            }
            else if (distanceToPlayer > 15f)
            {
                IsPlayerTarget = false;
            }
        }
    }
    public override void Damage(int _dmg, Vector3 _tarGetPos)
    {
        if (!IsDead)
        {
            base.Damage(_dmg, _tarGetPos);
            IsPlayerTarget = true;
        }
    }

    protected void PlayerDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, targetRadius, LayerMask.GetMask("PLAYER"));
        if (colliders.Length > 0 && !IsTargeting)
        {
            IsPlayerTarget = true;
        }
    }

    IEnumerator CoroutineAttack()
    {
        nav.velocity = Vector3.zero;
        IsAttack = true;
        IsRunning = false;
        nav.isStopped = true;
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("OnAttack");
        yield return new WaitForSeconds(1f);
        nav.isStopped = false;
        IsRunning = true;
        IsAttack = false;
    }

    public void EnableCollider()
    {
        meleeArea.enabled = true;
    }

    public void TargetSetDestination(Transform point)
    {
        targetPoint = point;
        IsRunning = true;
    }

    private void OnDrawGizmos()
    {
        if (OnGizmos)
            Gizmos.DrawWireSphere(transform.position, targetRadius);
    }

    protected override void OnAnimatorMove()
    {
        animator.SetBool("IsAttack", IsAttack);
        animator.SetBool("IsRunning", IsRunning);
    }

}