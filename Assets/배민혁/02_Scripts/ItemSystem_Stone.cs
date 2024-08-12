using UnityEngine;
using System.Collections;
using System.Linq;

public class ItemSystemStone : MonoBehaviour
{

    private float woodHp = 100;
    public GameObject player;
    private float damagePoint;

    [SerializeField] private GameObject rock_Prefabs;  // 통나무. 나무가 쓰러진 이후 생성할 재료.

    [SerializeField] private float force;  // 나무가 땅에 쓰러지도록 밀어줄 힘의 세기(랜덤으로 정할 것) 
    [SerializeField] private GameObject[] childStone;  // 부서질 돌조각. 부서진 다음에 지연 시간 후 파괴되야 해서 필요함.

    [SerializeField] private GameObject stonePlace; // 바위가 있던 자리 오브젝트

    [SerializeField] private CapsuleCollider parentCol;  // 전체적인 바위에 붙어있는 캡슐 콜라이더. 바위가 부서지면 이걸 비활성화 해주어야 함.
    [SerializeField] private CapsuleCollider[] childCol;  // 부서지는 바위인 바위 윗 부분에 붙어있는 캡슐 콜라이더. 바위가 부서지면 이걸 활성화 해주어야 함.
    [SerializeField] private Rigidbody[] childRigid; // 부서지는 바위인 바위 윗 부분에 붙어있는 Rigidbody를 통해 바위가 부서지면 중력을 활성화 해주어야 함.

    [SerializeField] private float destroyTime;  // 돌조각 제거 시간. 바위 윗 부분이 땅에 흩어지고 나서 파괴될 시간.

    [SerializeField]
    private string chop_sound;  // 바위 공격 시 재생시킬 사운드 이름 
    [SerializeField]
    private string falldown_sound;  // 바위가 부서질 때 재생시킬 사운드 이름 
    [SerializeField]
    private string logChange_sound;  // 바위가 부숴져 돌조각으로 바뀔 때 재생시킬 사운드 이름
    private bool stoneActive = true;



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
        if (stoneActive)
        {
            if (coll.CompareTag("MELEE"))
            {
                damagePoint = coll.gameObject.GetComponent<Weapon>().woodDamage;
                //Hit(_pos);
                woodHp -= damagePoint;
                Debug.Log(woodHp);
                if (woodHp <= 0 && parentCol.enabled)
                {
                    FallDownStone();
                    stoneActive = false;
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

    private void FallDownStone()
    {
        //SoundManager.instance.PlaySE(falldown_sound);

        parentCol.enabled = false;
        for (int i = 0; i < childCol.Count(); i++)
        {
            childCol[i].enabled = true;
        }
        for (int i = 0; i < childRigid.Count(); i++)
        {
            childRigid[i].useGravity = true;

            childRigid[i].AddForce(Random.Range(-force, force), 0f, Random.Range(-force, force));
        }
        StartCoroutine(LogCoroutine());
        StopCoroutine(LogCoroutine());

    }

    IEnumerator LogCoroutine()
    {
        for (int i = 0; i < Random.Range(3, 10); i++)
        {
            Instantiate(rock_Prefabs, stonePlace.gameObject.transform.parent.position * Random.Range(0.9f, 1.1f), Quaternion.LookRotation(stonePlace.transform.parent.up * Random.Range(0.0f, 180.0f)));
        }
        // Instantiate(Log_Prefabs, childTree.gameObject.transform.parent.position * Random.Range(0.9f, 1.1f), Quaternion.LookRotation(stump.transform.parent.up * Random.Range(0.0f, 180.0f)));
        // Instantiate(Log_Prefabs, childTree.gameObject.transform.parent.position * Random.Range(0.9f, 1.1f), Quaternion.LookRotation(stump.transform.parent.up * Random.Range(0.0f, 180.0f)));
        // Instantiate(Log_Prefabs, childTree.gameObject.transform.parent.position * Random.Range(0.9f, 1.1f), Quaternion.LookRotation(stump.transform.parent.up * Random.Range(0.0f, 180.0f)));
        //Debug.Log(childTree.gameObject.transform.parent.name);
        //Instantiate(Log_Prefabs, childTree.gameObject.transform.parent.position, Quaternion.LookRotation(stump.transform.parent.up * Random.Range(0.0f, 180.0f)));

        //Instantiate(Log_Prefabs, stump.transform.position + Random.insideUnitSphere, Quaternion.identity);
        //Instantiate(Log_Prefabs, stump.transform.position + Random.insideUnitSphere, Quaternion.identity);
        //Instantiate(Log_Prefabs, stump.transform.position + Random.insideUnitSphere, Quaternion.identity);
        var stumpExp = stonePlace.gameObject.AddComponent<Rigidbody>();
        var stumpRigid = stonePlace.gameObject.GetComponent<Rigidbody>();
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

        for (int i = 0; i < childStone.Count(); i++)
        {
            Destroy(childStone[i].gameObject);
        }

        //this.gameObject.GetComponentInParent<ItemRegen>().checkObject = false;
    }

}
