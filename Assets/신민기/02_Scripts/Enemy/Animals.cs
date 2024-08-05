using UnityEngine;

public class Animals : Enemys
{
    protected bool IsHappy;

    public override void Run(Vector3 _tarGetPos)
    {
        destination = new Vector3(transform.position.x - _tarGetPos.x, 0, transform.position.z - _tarGetPos.z).normalized;
        currentTime = Stats.runTime;
        nav.speed = Stats.runSpeed;
        IsWalking = false;
        IsRunning = true;
    }

    public override void Damage(int _dmg, Vector3 _tarGetPos)
    {
        base.Damage(_dmg, _tarGetPos);

        if (!IsDead)
            Run(_tarGetPos);
    }

    protected override void Initialize()
    {
        IsWalking = false;
        IsHappy = false;
        IsRunning = false;
        IsAction = true;
        nav.speed = Stats.walkSpeed;
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
        int randomAction = Random.Range(0, 3);
        switch (randomAction)
        {
            case 0:
                Wait();
                break;

            case 1:
                Walk();
                break;
            case 2:
                Peek();
                break;
        }
    }

    protected virtual void Wait()
    {
        currentTime = Stats.waitTime;
        if(idle_Sound != null)
            PlaySE(idle_Sound);
    }

    protected virtual void Peek()
    {
        currentTime = Stats.waitTime;
        IsHappy = true;
    }

    protected virtual void Walk()
    {
        currentTime = Stats.walkTime;
        nav.speed = Stats.walkSpeed;
        IsWalking = true;
    }

    protected override void OnAnimatorMove()
    {
        animator.SetBool("IsWalking", IsWalking);
        animator.SetBool("IsRunning", IsRunning);
        animator.SetBool("IsHappy", IsHappy);
    }
}
