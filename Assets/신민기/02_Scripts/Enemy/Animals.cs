using UnityEngine;

public class Animals : Enemys
{
<<<<<<< HEAD
    bool IsHappy;
=======
    protected bool IsHappy;
>>>>>>> dd014889354572f0abb1d3a42768d4434be9d3a4

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

<<<<<<< HEAD
    protected void Wait()
    {
        currentTime = Stats.waitTime;
        PlaySE(idle_Sound);
    }

    protected void Peek()
=======
    protected virtual void Wait()
    {
        currentTime = Stats.waitTime;
        if(idle_Sound != null)
            PlaySE(idle_Sound);
    }

    protected virtual void Peek()
>>>>>>> dd014889354572f0abb1d3a42768d4434be9d3a4
    {
        currentTime = Stats.waitTime;
        IsHappy = true;
    }

<<<<<<< HEAD
    protected void Walk()
=======
    protected virtual void Walk()
>>>>>>> dd014889354572f0abb1d3a42768d4434be9d3a4
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
