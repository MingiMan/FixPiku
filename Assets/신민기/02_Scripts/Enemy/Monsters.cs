using NUnit.Framework;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

// �㿡�� ������ ���͵��� �� ��ũ��Ʈ�� ��������ش�.
public class Monsters : Enemys
{
    [SerializeField] protected BoxCollider meleeArea;

    public float targetRadius = 0.5f;

    public float targetRange = 1f;

    private RaycastHit[] sphereCastHits;

    public Transform point;

    bool IsAttack;

    bool IsTest;

    protected override void Start()
    {
        base.Start();
        nav.ResetPath();
        nav.speed = Stats.runSpeed;
        IsRunning = true;
        destination = point.position;
        // RadomPointHouse();
    }

    void RadomPointHouse()
    {
        Transform[] childTransforms = GameObject.Find("HouseCheck").GetComponentsInChildren<Transform>();
        int randomIndex = Random.Range(0,childTransforms.Length);
        point = childTransforms[randomIndex];
        destination = point.position;   
    }

    protected override void Move()
    {
        if (IsRunning)
        {
            nav.SetDestination(destination);
            
            if(Vector3.Distance(transform.position, destination) <= nav.stoppingDistance)
            {
                IsRunning = false;
                nav.isStopped = true;
                IsTest = true;
            }
        }
    }


    protected override void Update()
    {
        if (!IsDead)
        {
            Move();
            Test();
        }
    }

    void Test()
    {
        if(IsTest &&  !IsAttack)
        {
            StartCoroutine(Attack());
        }
    }

    protected override void FixedUpdate()
    {

    }

    public override void Run(Vector3 _targetPos)
    {

    }


    protected virtual void AvoidOtherEnemie()
    {
        float minimumDistance = 1.0f; // �ּ� ��� �Ÿ�
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, minimumDistance);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("ENEMY") && hitCollider.transform != transform)
            {
                Vector3 directionAway = (transform.position - hitCollider.transform.position).normalized;
                transform.position += directionAway * Time.deltaTime;
            }
        }
    }

    IEnumerator Attack()
    {
        IsAttack = true;
        animator.SetTrigger("OnAttack");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
        IsAttack = false;
    }


    public void EnableCollider()
    {
        meleeArea.enabled = true;
    }

    public void DisableCollider()
    {
        meleeArea.enabled = false;
    }

    protected virtual void PlayerDetaticon()
    {
        sphereCastHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("PLAYER"));

        foreach (var hit in sphereCastHits)
        {
            if (hit.collider.CompareTag("Player"))
            {
                //Vector3 directionToBase = (hit.transform.position - transform.position).normalized;
                //Quaternion targetRotation = Quaternion.LookRotation(directionToBase);
                //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1.5f);
                destination = playerTr.transform.position;
                nav.isStopped = false;               
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (sphereCastHits == null) return;

        Gizmos.color = Color.blue;
        Vector3 start = transform.position;
        Vector3 direction = transform.forward * targetRange;

        // ���Ǿ�ĳ��Ʈ�� ���۰� ���� ������ �ð�ȭ
        Gizmos.DrawWireSphere(start, targetRadius);
        Gizmos.DrawWireSphere(start + direction, targetRadius);

        // ���Ǿ�ĳ��Ʈ ��θ� ������ �ð�ȭ
        Gizmos.DrawLine(start, start + direction);

        // ���Ǿ�ĳ��Ʈ�� ������ ��� �浹 ������ �ð�ȭ
        Gizmos.color = Color.red;
        foreach (var hit in sphereCastHits)
        {
            Gizmos.DrawSphere(hit.point, 0.1f); // �浹 ������ ���� ��ü�� �׸��ϴ�.
        }
    }

    protected override void OnAnimatorMove()
    {
        base.OnAnimatorMove();
        animator.SetBool("IsAttack", IsAttack);
    }   
}