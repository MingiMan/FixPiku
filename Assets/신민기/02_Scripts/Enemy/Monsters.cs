using UnityEngine;

// 밤에만 나오는 몬스터들은 이 스크립트를 적용시켜준다.
public class Monsters : Enemys
{
    [Header("Monsters")]
    public Transform houseTr;
    public float wanderRadius = 10f; // 몬스터가 이동할 수 있는 반경

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
