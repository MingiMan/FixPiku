using UnityEngine;

public class Spike : Enemys
{
    [SerializeField] BoxCollider meleeArea;
    [SerializeField] float targetRadius = 0.5f;
    [SerializeField] float targetRange = 1f;
    private RaycastHit[] sphereCastHits;
    bool IsAttack;
    bool IsActive;

    protected override void Awake()
    {
        base.Awake();
        meleeArea = GetComponentInChildren<BoxCollider>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        rigid.isKinematic = false;
        meleeArea.enabled = false;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected void PlayerDetection()
    {
        //sphereCastHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("PLAYER"));

        //foreach (var hit in sphereCastHits)
        //{
        //    if (hit.collider.CompareTag("Player"))
        //    {
        //        destination = playerTr.transform.position;
        //        IsActive = true;
        //        IsRunning = true;
        //    }
        //}

        Collider[] colliders = Physics.OverlapSphere(transform.position, 4f, LayerMask.GetMask("PLAYER"));
        if (colliders.Length > 0)
        {
            IsActive = true;
            IsRunning = true;
        }
    }

    protected override void OnAnimatorMove()
    {
        animator.SetBool("IsRunning", IsRunning);
        animator.SetBool("IsActive", IsActive);
    }
}
