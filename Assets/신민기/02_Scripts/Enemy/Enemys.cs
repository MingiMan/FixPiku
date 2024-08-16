using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Animal,
    Monster,
}
[RequireComponent(typeof(NavMeshAgent))]
public class Enemys : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody rigid;
    protected AudioSource theAudio;
    protected NavMeshAgent nav;
    protected Transform playerTr;
    protected Material mat;
    protected SkinnedMeshRenderer skinnedMesh;
    [SerializeField] protected ParticleSystem deadDustParticle;

    public EnemyType enemyType;

    protected int currentHp;
    protected float currentTime;

    protected bool IsAction;
    protected bool IsWalking;
    protected bool IsRunning;
    protected bool IsDead;
    protected bool Invisible;
    protected Vector3 destination;

    [Header("Monster Stats")]
    [SerializeField] public BasicMonsters.Base Stats;

    [Header("SoundClip(사운드가 없는 몬스터도 있음)")]
    [SerializeField] protected AudioClip dead_Sound;
    [SerializeField] protected AudioClip hurt_Sound;
    [SerializeField] protected AudioClip idle_Sound;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        theAudio = GetComponent<AudioSource>();
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        mat = skinnedMesh.material;
    }

    protected virtual void OnEnable()
    {
        currentHp = Stats.health;
        currentTime = Stats.waitTime;
        IsAction = true;
        nav.autoTraverseOffMeshLink = false;
        mat.color = Color.white;
        Invisible = false;
        IsDead = false;
        rigid.isKinematic = false;
        gameObject.layer = 8;
        gameObject.tag = "ENEMY";
        StartCoroutine(CheckOffMeshLink());
    }

    //protected virtual void Start()
    //{
    //    currentHp = Stats.health;
    //    currentTime = Stats.waitTime;
    //    IsAction = true;
    //    nav.autoTraverseOffMeshLink = false;
    //    StartCoroutine(CheckOffMeshLink());
    //}

    protected virtual void Update()
    {
        if (!IsDead)
        {
            ElapseTime();
            FreezeRotation();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (!IsDead)
            Move();
    }

    protected virtual void Initialize()
    {
        Invisible = false;
        IsWalking = false;
        IsRunning = false;
        IsAction = true;
        nav.speed = Stats.walkSpeed;
        nav.ResetPath();
        FindRandomPoint(transform.position, 10f, out destination);
    }

    protected virtual void Move()
    {
        if (IsWalking)
        {
            nav.SetDestination(destination);

            if (Vector3.Distance(transform.position, destination) <= nav.stoppingDistance)
            {
                IsWalking = false;
                nav.ResetPath();
                RandomAction();
            }
        }
        else if (IsRunning)
        {
            nav.SetDestination(transform.position + destination);
        }
    }

    protected virtual void ElapseTime()
    {
        if (IsAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
                Initialize();
        }
    }
    protected virtual void RandomAction()
    {
        // 상속받은 스크립트에서 작성하시길..
    }

    protected bool FindRandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    protected IEnumerator CheckOffMeshLink()
    {
        while (true)
        {
            if (nav.isOnOffMeshLink)
            {
                animator.SetTrigger("OnJump");
                StartCoroutine(Jump(nav, 2.0f, 1.0f));
                yield return new WaitForSeconds(1.0f);
            }
            yield return null;
        }
    }

    protected IEnumerator Jump(NavMeshAgent agent, float height, float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos;

        float time = 0f;

        while (time < 1f)
        {
            float upDist = height * (time - time * time);
            agent.transform.position = Vector3.Lerp(startPos, endPos, time) + upDist * Vector3.up;

            time += Time.deltaTime / duration;
            yield return null;
        }

        agent.CompleteOffMeshLink();
    }

    public virtual void Run(Vector3 _tarGetPos)
    {
        destination = new Vector3(transform.position.x - _tarGetPos.x, 0, transform.position.z - _tarGetPos.z).normalized;
        currentTime = Stats.runTime;
        nav.speed = Stats.runSpeed;
        IsWalking = false;
        IsRunning = true;
    }

    public virtual void Damage(int _dmg, Vector3 _tarGetPos)
    {
        if (!IsDead && !Invisible)
        {
            Invisible = true;
            currentHp -= _dmg;

            if (currentHp <= 0)
            {
                Dead();
                return;
            }
            if (hurt_Sound != null)
                PlaySE(hurt_Sound);
            StartCoroutine(ColorDamage());
            animator.SetTrigger("OnDamage");
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

    public virtual void Dead()
    {
        StopAllCoroutines();
        mat.color = Color.red;
        IsWalking = false;
        IsRunning = false;
        nav.isStopped = true;

        if (dead_Sound != null)
            PlaySE(dead_Sound);
        gameObject.layer = 6;
        gameObject.tag = "Untagged";
        animator.SetTrigger("OnDie");
        rigid.isKinematic = false;
        OnEnemyDeath();
    }


    protected void OnEnemyDeath()
    {
        IsDead = true;
        switch (enemyType)
        {
            case EnemyType.Animal:
                StartCoroutine(AnimalDeath());
                break;
            case EnemyType.Monster:
                StartCoroutine(MonsterDeath());
                break;
        }
    }

    protected void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
        rigid.linearVelocity = Vector3.zero;
    }

    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }

    public void HitByCannon(int damage, Vector3 explosionPos)
    {
        Vector3 reactVec = transform.position - explosionPos;
        Damage(damage, reactVec);
    }

    IEnumerator MonsterDeath()
    {
        yield return new WaitForSeconds(3f);
        Instantiate(deadDustParticle, transform.position, transform.rotation);
        GameManager.Instance.monsterSpawner.OnMonsterDeath(this.gameObject);
    }

    IEnumerator AnimalDeath()
    {
        yield return new WaitForSeconds(3f);
        Instantiate(deadDustParticle, transform.position, transform.rotation);
        GameManager.Instance.animalSpawner.AnimalDeath(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().OnDamage(Stats.atkDamage);
        }
    }


    protected virtual void OnAnimatorMove()
    {
        animator.SetBool("IsRunning", IsRunning);
    }
}
