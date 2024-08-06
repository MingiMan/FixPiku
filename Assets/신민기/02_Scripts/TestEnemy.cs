using UnityEngine;
using UnityEngine.AI;

public class TestEnemy : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody rigid;
    protected AudioSource theAudio;
    protected NavMeshAgent nav;
    protected Transform playerTr;
    protected Material mat;
    protected SkinnedMeshRenderer skinnedMesh;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        theAudio = GetComponent<AudioSource>();
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        mat = skinnedMesh.material;
    }

}
