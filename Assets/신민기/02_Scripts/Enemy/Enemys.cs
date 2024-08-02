using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemys : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody rigid;
    protected AudioSource theAudio;
    protected NavMeshAgent nav; 
    protected Transform playerTr;

    [SerializeField] protected BoxCollider meleeArea;

    [Header("Monster Stats")]
    [SerializeField] public BasicMonsters.Base Stats;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        theAudio = GetComponent<AudioSource>();
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Initalize()
    {

    }
}
