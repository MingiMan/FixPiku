using UnityEngine;

public class Animals : Enemys
{
    //아이템 드롭용 구현//////////////////
    [SerializeField] private float[] item_count;  // 아이템 생성 최소 최대 개수
    [SerializeField] private GameObject item_Prefabs;  // 재료아이템. 오브젝트가 파괴된 이후 생성할 재료.

    //아이템 드롭용 구현//////////////////
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
        else
        {
            ////////////////////죽으면 아이템 드롭///////////////////
            for (int i = 0; i < Random.Range(item_count[0], item_count[1] + 1); i++)
            {
                Instantiate(item_Prefabs, this.gameObject.transform.position + new Vector3(Random.Range(1.0f, 1.5f), Random.Range(1.0f, 1.5f), Random.Range(1.0f, 1.5f)), Quaternion.LookRotation(this.transform.parent.up * Random.Range(0.0f, 180.0f)));
            }
            ////////////////////죽으면 아이템 드롭///////////////////
        }
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
        if (idle_Sound != null)
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

    protected override void OnEnable()
    {
        base.OnEnable();
    }
}
