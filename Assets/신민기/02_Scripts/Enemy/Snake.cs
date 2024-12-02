using System.Collections;
using UnityEngine;

public class Snake : Animals
{
    [SerializeField] BoxCollider meleeArea;
    bool IsTracking;
    bool IsAttack;


    //아이템 드롭용 구현//////////////////
    [SerializeField]
    private float[] dropItem_count;  // 아이템 생성 최소 최대 개수
    [SerializeField] private GameObject dropItem_Prefabs;  // 재료아이템. 오브젝트가 파괴된 이후 생성할 재료.

    //아이템 드롭용 구현//////////////////
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        rigid.isKinematic = false;
        meleeArea.enabled = false;
        nav.speed = Stats.runSpeed;
    }

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
        if (!IsDead && !Invisible)
        {
            Invisible = true;
            currentHp -= _dmg;
            if (currentHp <= 0)
            {
                Dead();
                ////////////////////죽으면 아이템 드롭///////////////////
                if (dropItem_count[1] - dropItem_count[0] <= 1) Instantiate(dropItem_Prefabs, this.gameObject.transform.position + new Vector3(Random.Range(1.0f, 1.5f), Random.Range(1.0f, 1.5f), Random.Range(1.0f, 1.5f)), Quaternion.LookRotation(this.transform.parent.up * Random.Range(0.0f, 180.0f)));
                else
                {
                    for (int i = 0; i < Random.Range(dropItem_count[0], dropItem_count[1] + 1); i++)
                    {
                        Instantiate(dropItem_Prefabs, this.gameObject.transform.position + new Vector3(Random.Range(1.0f, 1.5f), Random.Range(1.0f, 1.5f), Random.Range(1.0f, 1.5f)), Quaternion.LookRotation(this.transform.parent.up * Random.Range(0.0f, 180.0f)));
                    }
                }
                ////////////////////죽으면 아이템 드롭///////////////////
                return;
            }
            if (hurt_Sound != null)
                PlaySE(hurt_Sound);
            StartCoroutine(ColorDamage());
            animator.SetTrigger("OnDamage");
            IsTracking = true;
            PlayerTracking();
        }
    }
    IEnumerator ColorDamage()
    {
        float duration = 0.2f;
        float elapsed = 0f;
        Color originalColor = mat.color;
        Color damagedColor = Color.red;

        while (elapsed < duration)
        {
            mat.color = Color.Lerp(originalColor, damagedColor, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        mat.color = damagedColor;

        elapsed = 0f;
        while (elapsed < duration)
        {
            mat.color = Color.Lerp(damagedColor, originalColor, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Invisible = false;
        mat.color = originalColor;
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
            float distanceToPlayer = Vector3.Distance(transform.position, playerTr.transform.position);
            nav.SetDestination(playerTr.transform.position);

            if (!IsAttack && distanceToPlayer <= nav.stoppingDistance)
            {
                Quaternion targetRotation = Quaternion.LookRotation(playerTr.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20f);
                StartCoroutine(CoroutineAttack());
            }
            else if (distanceToPlayer > 15f)
            {
                Initialize();
            }
        }
        else if (IsRunning)
        {
            nav.SetDestination(destination);
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
        meleeArea.enabled = false;
        IsRunning = true;
        IsAttack = false;
    }


    public void EnableCollider()
    {
        meleeArea.enabled = true;
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
    public override void Dead()
    {
        StopAllCoroutines();
        mat.color = Color.red;
        IsWalking = false;
        IsRunning = false;
        IsDead = true;

        if (dead_Sound != null)
            PlaySE(dead_Sound);

        animator.SetTrigger("OnDie");
        rigid.isKinematic = false;
        gameObject.layer = 6;
        gameObject.tag = "Untagged";
        StartCoroutine(SnakeDeath());
    }

    IEnumerator SnakeDeath()
    {
        yield return new WaitForSeconds(3f);
        Instantiate(deadDustParticle, transform.position, transform.rotation);
        GameManager.Instance.wildMonsterSpawner.SnakeDeath(this.gameObject);
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
