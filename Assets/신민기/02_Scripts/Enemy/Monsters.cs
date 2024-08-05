using System.Collections;
using UnityEngine;

// 밤에만 나오는 몬스터들은 이 스크립트를 적용시켜준다.
public class Monsters : Enemys
{
    Transform houseTR;
    [SerializeField] float distance;
    [SerializeField] Transform houseCheck;
    [SerializeField] protected BoxCollider meleeArea;
    public float targetRadius = 0.5f;
    public float targetRange = 1f;
    private RaycastHit[] sphereCastHits;
    bool IsAttack;
    bool IsCheck;

    protected override void Start()
    {
        base.Start();
        houseTR = GameObject.FindGameObjectWithTag("HOUSE").transform;
        nav.ResetPath();
        nav.speed = Stats.runSpeed;
        IsRunning = true;
        destination = houseTR.transform.position;
    }

    protected override void Move()
    {
        if (IsRunning)
        {
            nav.SetDestination(destination);
        }
    }

    protected override void Update()
    {
        if (!IsDead)
        {
            Move();
            BaseCheck();
            AvoidOtherEnemies();
        }
    }

    protected override void FixedUpdate()
    {

    }

    public override void Run(Vector3 _targetPos)
    {

    }

    protected virtual void BaseCheck()
    {
        sphereCastHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("HOUSE"));

        foreach (var hit in sphereCastHits)
        {
            if (hit.collider.CompareTag("HOUSE"))
            {
                Vector3 directionToBase = (hit.transform.position - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(directionToBase);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1.5f);

                destination = hit.point;
                nav.SetDestination(destination);

                nav.isStopped = true;
                IsRunning = false;
            }
        }
    }

    protected virtual void AvoidOtherEnemies()
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
        nav.isStopped = true;
        IsRunning = false;
        IsAttack = true;
        animator.SetTrigger("OnAttack");
        yield return new WaitForSeconds(0.6f);
        rigid.isKinematic = true;
        yield return new WaitForSeconds(1f);
        rigid.isKinematic = false;
        yield return new WaitForSeconds(0.1f);
        IsAttack = false;
        nav.isStopped = false;
    }

    public void EnableCollider()
    {
        meleeArea.enabled = true;
    }

    public void DisableCollider()
    {
        meleeArea.enabled = false;
    }

    protected override void OnAnimatorMove()
    {
        base.OnAnimatorMove();
        animator.SetBool("IsAttack", IsAttack);
    }

    private void OnDrawGizmos()
    {
        if (sphereCastHits == null) return;

        Gizmos.color = Color.blue;
        Vector3 start = transform.position;
        Vector3 direction = transform.forward * targetRange;

        // 스피어캐스트의 시작과 끝을 원으로 시각화
        Gizmos.DrawWireSphere(start, targetRadius);
        Gizmos.DrawWireSphere(start + direction, targetRadius);

        // 스피어캐스트 경로를 선으로 시각화
        Gizmos.DrawLine(start, start + direction);

        // 스피어캐스트가 감지한 모든 충돌 지점을 시각화
        Gizmos.color = Color.red;
        foreach (var hit in sphereCastHits)
        {
            Gizmos.DrawSphere(hit.point, 0.1f); // 충돌 지점에 작은 구체를 그립니다.
        }
    }
}
