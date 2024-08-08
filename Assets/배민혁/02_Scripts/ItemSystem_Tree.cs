using UnityEngine;
using System.Collections;

public class ItemSystemTree : MonoBehaviour
{

    private float woodHp = 100;
    public GameObject player;
    private float damagePoint;

    [SerializeField]
    private GameObject Log_Prefabs;  // 통나무. 나무가 쓰러진 이후 생성할 재료.

    [SerializeField]
    private float force;  // 나무가 땅에 쓰러지도록 밀어줄 힘의 세기(랜덤으로 정할 것) 
    [SerializeField]
    private GameObject childTree;  // 쓰러질 나무 윗부분. 쓰러지고 난 다음에 지연 시간 후 파괴 되야 해서 필요함.

    [SerializeField]
    private GameObject stump;

    [SerializeField]
    private CapsuleCollider parentCol;  // 전체적인 나무에 붙어있는 캡슐 콜라이더. 나무가 쓰러지면 이걸 비활성화 해주어야 함.
    [SerializeField]
    private CapsuleCollider childCol;  // 쓰러질 나무인 나무 윗부분에 붙어있는 캡슐 콜라이더. 나무가 쓰러지면 이걸 활성화 해주어야 함.
    [SerializeField]
    private Rigidbody childRigid; // 쓰러질 나무인 나무 윗부분에 붙어있는 Rigidbody를 통해 나무가 쓰러지면 중력을 활성화 해주어야 함.

    [SerializeField]
    private float destroyTime;  // 나무 제거 시간. 나무 윗 부분이 땅에 쓰러지고 나서 파괴될 시간.

    [SerializeField]
    private string chop_sound;  // 나무 도끼질시 재생시킬 사운드 이름 
    [SerializeField]
    private string falldown_sound;  // 나무 쓰러질 때 재생시킬 사운드 이름 
    [SerializeField]
    private string logChange_sound;  // 나무 쓰러져서 통나무로 바뀔 때 재생시킬 사운드 이름
    private bool treeActive = true;



    void Start()
    {
        player = GameObject.FindWithTag("Player");


    }
    void Update()
    {
        //damagePoint = player.GetComponent<PlayerState>().attackState;

    }
    void OnTriggerEnter(Collider coll)
    {
        if (treeActive)
        {
            if (coll.CompareTag("MELEE"))
            {
                damagePoint = coll.gameObject.GetComponent<Weapon>().woodDamage;
                //Hit(_pos);
                woodHp -= damagePoint;
                Debug.Log(woodHp);
                if (woodHp <= 0 && parentCol.enabled)
                {
                    FallDownTree();
                    treeActive = false;
                }
            }
        }

    }

    /*private void Hit(Vector3 _pos)
    {
        //SoundManager.instance.PlaySE(chop_sound);

        GameObject clone = Instantiate(go_hit_effect_prefab, _pos, Quaternion.Euler(Vector3.zero));
        Destroy(clone, debrisDestroyTime);
    }*/

    private void FallDownTree()
    {
        //SoundManager.instance.PlaySE(falldown_sound);

        parentCol.enabled = false;
        childCol.enabled = true;
        childRigid.useGravity = true;

        childRigid.AddForce(Random.Range(-force, force), 0f, Random.Range(-force, force));

        StartCoroutine(LogCoroutine());
        StopCoroutine(LogCoroutine());

    }

    IEnumerator LogCoroutine()
    {
        for (int i = 0; i < Random.Range(3, 10); i++)
        {
            Instantiate(Log_Prefabs, childTree.gameObject.transform.parent.position * Random.Range(0.9f, 1.1f), Quaternion.LookRotation(stump.transform.parent.up * Random.Range(0.0f, 180.0f)));
        }
        // Instantiate(Log_Prefabs, childTree.gameObject.transform.parent.position * Random.Range(0.9f, 1.1f), Quaternion.LookRotation(stump.transform.parent.up * Random.Range(0.0f, 180.0f)));
        // Instantiate(Log_Prefabs, childTree.gameObject.transform.parent.position * Random.Range(0.9f, 1.1f), Quaternion.LookRotation(stump.transform.parent.up * Random.Range(0.0f, 180.0f)));
        // Instantiate(Log_Prefabs, childTree.gameObject.transform.parent.position * Random.Range(0.9f, 1.1f), Quaternion.LookRotation(stump.transform.parent.up * Random.Range(0.0f, 180.0f)));
        //Debug.Log(childTree.gameObject.transform.parent.name);
        //Instantiate(Log_Prefabs, childTree.gameObject.transform.parent.position, Quaternion.LookRotation(stump.transform.parent.up * Random.Range(0.0f, 180.0f)));

        //Instantiate(Log_Prefabs, stump.transform.position + Random.insideUnitSphere, Quaternion.identity);
        //Instantiate(Log_Prefabs, stump.transform.position + Random.insideUnitSphere, Quaternion.identity);
        //Instantiate(Log_Prefabs, stump.transform.position + Random.insideUnitSphere, Quaternion.identity);
        var stumpExp = stump.gameObject.AddComponent<Rigidbody>();
        var stumpRigid = stump.gameObject.GetComponent<Rigidbody>();
        stumpRigid.isKinematic = true;
        stumpRigid.useGravity = false;
        //Vector3 impactPoint = Random.insideUnitSphere;
        // stumpExp.AddExplosionForce(
        //                     10.0f,            // 횡 폭발력
        //                     transform.position + impactPoint * 2.0f, // 폭발 원점
        //                     2.0f,               // 폭발 반경
        //                     20.0f);           // 총 폭발력

        yield return new WaitForSeconds(destroyTime);

        //SoundManager.instance.PlaySE(logChange_sound);

        Destroy(childTree.gameObject);
        this.gameObject.GetComponentInParent<ItemRegen>().checkObject = false;
    }

}
