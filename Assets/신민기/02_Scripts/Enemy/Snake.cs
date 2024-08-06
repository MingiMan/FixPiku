using System.Collections;
using UnityEngine;

public class Snake : Animals
{
    [SerializeField] BoxCollider meleeArea;
    bool IsTracking;
    bool IsAttack;
    //protected override void Start()
    //{
    //    base.Start();
    //    meleeArea.enabled = false;
    //}
    protected override void Update()
    {
        if (!IsDead)
        {
            ElapseTime();
            PlayerTracking();
            FreezeRotation();
        }
    }

    protected override void FixedUpdate()
    {
        if (!IsDead)
            Move();
    }


    public override void Run(Vector3 _tarGetPos)
    {

        IsAction = false;
        IsTracking = true;
        PlayerTracking();
    }

    public override void Damage(int _dmg, Vector3 _tarGetPos)
    {
        base.Damage(_dmg, _tarGetPos);

        IsTracking = true;

        if (!IsDead)
            PlayerTracking();
    }

    protected override void Initialize()
    {
        IsTracking = false;
        IsRunning = false;
        IsAction = true;
        nav.speed = Stats.runSpeed;
        nav.ResetPath();
        FindRandomPoint(transform.position, 10f, out destination);
        RandomAction();
    }


    protected override void ElapseTime()
    {
        base.ElapseTime();
    }

    protected override void RandomAction()
    {
        IsAction = true;
        int randomAction = Random.Range(0, 2);
        switch (randomAction)
        {
            case 0:
                Wait();
                break;
            case 1:
                Patrol();
                break;
        }
    }

    protected override void Wait()
    {
        currentTime = Stats.waitTime;
        if (idle_Sound != null)
            PlaySE(idle_Sound);
    }

    protected void Patrol()
    {
        currentTime = Stats.walkTime;
        nav.speed = Stats.walkSpeed;
        IsRunning = true;
    }


    protected override void Move()
    {
        if (IsTracking)
        {
            nav.SetDestination(playerTr.transform.position);
            float targetRadius = 0.5f;
            float targetRange = 1f;

            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("PLAYER"));

            if (rayHits.Length > 0 && !IsAttack)
                StartCoroutine(Attack());

            else if (Vector3.Distance(transform.position, playerTr.transform.position) > 15f)
            {
                Initialize();
            }
        }

        else if (IsRunning)
        {
            nav.SetDestination(destination);
        }
    }

    IEnumerator Attack()
    {
        IsAttack = true;
        animator.SetTrigger("OnAttack");
        yield return new WaitForSeconds(0.6f);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(1f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(1f);
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

    void PlayerTracking()
    {
        if (IsTracking)
        {
            IsAction = false;
            IsHappy = false;
            IsRunning = true;
            nav.speed = Stats.runSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().OnDamage(Stats.atkDamage);
        }
    }

    protected override void OnAnimatorMove()
    {
        animator.SetBool("IsRunning", IsRunning);
        animator.SetBool("IsAttack", IsAttack);
    }
}
