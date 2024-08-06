using UnityEngine;

// �㿡�� ������ ���͵��� �� ��ũ��Ʈ�� ��������ش�.
public class Monsters : Enemys
{
    [Header("Monsters")]
    public Transform houseTr;
    public float wanderRadius = 10f; // ���Ͱ� �̵��� �� �ִ� �ݰ�

    private Vector3 randomDestination;
    protected override void Start()
    {
        base.Start();
        houseTr = GameObject.FindGameObjectWithTag("HOUSE").transform;
    }

    private void OnEnable()
    {
        nav.ResetPath();
        nav.speed = Stats.runSpeed;
        IsRunning = true;
    }

    protected override void Move()
    {
        if (IsRunning)
        {
            nav.SetDestination(houseTr.transform.position);
            if(Vector3.Distance(transform.position, houseTr.transform.position) <= nav.stoppingDistance)
            {
                IsRunning = false;
            }
        }
    }

    protected override void Update()
    {

    }

    protected override void FixedUpdate()
    {
        if (!IsDead)
            Move();
    }

    protected override void OnAnimatorMove()
    {
        base.OnAnimatorMove();
    }


    public override void Run(Vector3 _tarGetPos)
    {

    }
}
