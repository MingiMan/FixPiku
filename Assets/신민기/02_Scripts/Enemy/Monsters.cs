using NUnit.Framework;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

// 밤에만 나오는 몬스터들은 이 스크립트를 적용시켜준다.
public class Monsters : Enemys
{
    [SerializeField] protected BoxCollider meleeArea;

    public float targetRadius = 0.5f;

    public float targetRange = 1f;

    private RaycastHit[] sphereCastHits;

    public Transform point;

    bool IsAttack;

    bool IsTest;


    public float checkDistance;
    protected override void OnEnable()
    {
        base.OnEnable();
        nav.ResetPath();
        nav.speed = Stats.runSpeed;
        IsRunning = true;
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

            if (Vector3.Distance(transform.position, destination) <= nav.stoppingDistance)
            {
                IsRunning = false;
                nav.isStopped = true;
                IsTest = true;
                //if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out RaycastHit hit, checkDistance, LayerMask.GetMask("HOUSE")))
                //{
                //    if(hit.collider.gameObject.layer == LayerMask.NameToLayer("HOUSE"))
                //    {
                //        IsTest = true;
                //    }
                //}
                //else
                //{
                //    destination = point.position;
                //    IsRunning = true;
                //    nav.isStopped = false;
                //}
            }
        }
    }


    protected override void Update()
    {
        if (!IsDead)
        {
            Move();
            Test();
            // PlayerDetaticon();
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
        float minimumDistance = 1.0f; // 최소 허용 거리
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
        sphereCastHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("HOUSE"));

        foreach (var hit in sphereCastHits)
        {
            if (hit.collider.CompareTag("HOUSE"))
            {
                Vector3 directionToBase = (hit.transform.position - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(directionToBase);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1.5f);        
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ENEMY"))
            rigid.isKinematic = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("ENEMY"))
            rigid.isKinematic = false;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        other.GetComponent<PlayerMovement>().OnDamage(Stats.atkDamage);
    //    }
    //}

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position + Vector3.up, transform.forward * checkDistance, Color.green);
        //if (sphereCastHits == null) return;

        //Gizmos.color = Color.blue;
        //Vector3 start = transform.position;
        //Vector3 direction = transform.forward * targetRange;

        //// 스피어캐스트의 시작과 끝을 원으로 시각화
        //Gizmos.DrawWireSphere(start, targetRadius);
        //Gizmos.DrawWireSphere(start + direction, targetRadius);

        //// 스피어캐스트 경로를 선으로 시각화
        //Gizmos.DrawLine(start, start + direction);

        //// 스피어캐스트가 감지한 모든 충돌 지점을 시각화
        //Gizmos.color = Color.red;
        //foreach (var hit in sphereCastHits)
        //{
        //    Gizmos.DrawSphere(hit.point, 0.1f); // 충돌 지점에 작은 구체를 그립니다.
        //}
    }

    protected override void OnAnimatorMove()
    {
        base.OnAnimatorMove();
        animator.SetBool("IsAttack", IsAttack);
    }
}