using UnityEngine;
using System.Collections;

public class ItemSystemTreeEx : MonoBehaviour
{

    private float woodHp = 100;
    public GameObject player;
    private float damagePoint; // 무기에서 가지고 올 데미지 포인트

    [SerializeField] private GameObject parentObject;   // position 참조용


    [SerializeField]
    private GameObject item_Prefabs;  // 재료아이템. 오브젝트가 파괴된 이후 생성할 재료.

    [SerializeField] private float force;  // 나무가 땅에 쓰러지도록 밀어줄 힘의 세기(랜덤으로 정할 것) 
    [SerializeField] private GameObject mainObject;  // 쓰러질 오브젝트 전체 부분. 쓰러지고 난 다음에 지연 시간 후 파괴 되야 해서 필요함. 본체가 날아가고 밑부분이 남을 예정

    [SerializeField] private GameObject objectPlace; // 오브젝트 있던자리 표시용 잔해

    [SerializeField] private CapsuleCollider parentCol;  // 전체적인 나무에 붙어있는 캡슐 콜라이더. 나무가 쓰러지면 이걸 비활성화 해주어야 함.
    [SerializeField] private CapsuleCollider childCol;  // 날아가버릴 오브젝트에 캡슐 콜라이더. 나무가 쓰러지면 이걸 활성화 해주어야 함.
    [SerializeField] private Rigidbody childRigid; // 날아가버릴 오브젝트에 붙어있는 Rigidbody를 통해 나무가 쓰러지면 중력을 활성화 해주어야 함.

    [SerializeField] private float destroyTime;  // 나무 제거 시간. 나무 윗 부분이 땅에 쓰러지고 나서 파괴될 시간.
    [SerializeField] private float[] item_count;  // 아이템 생성 최소 최대 개수

    [SerializeField] private string chop_sound;  // 나무 도끼질시 재생시킬 사운드 이름 
    [SerializeField] private string falldown_sound;  // 나무 쓰러질 때 재생시킬 사운드 이름 
    [SerializeField] private string logChange_sound;  // 나무 쓰러져서 통나무로 바뀔 때 재생시킬 사운드 이름
    private bool treeActive = true; // 오브젝트 존재시간. 존재하지 않을 시 && 낮이 될시 오브젝트 리젠





    void Start()
    {
        player = GameObject.FindWithTag("Player");


    }

    void OnTriggerEnter(Collider coll) //나무 피격시
    {
        if (treeActive)
        {
            if (coll.CompareTag("MELEE") && coll.gameObject.GetComponent<Weapon>() != null)
            {
                damagePoint = coll.gameObject.GetComponent<Weapon>().woodDamage;
                woodHp -= damagePoint;
                SoundManager.instance.PlaySFX(chop_sound);
                Debug.Log(woodHp);
                if (woodHp <= 0 && parentCol.enabled)
                {
                    FallDownTree();
                    treeActive = false;
                }
            }
        }

    }

    private void FallDownTree() //나무 파괴시
    {
        SoundManager.instance.PlaySFX(falldown_sound);

        parentCol.enabled = false;
        childCol.enabled = true;
        childRigid.useGravity = true;

        childRigid.AddForce(Random.Range(-force, force), 0f, Random.Range(-force, force));

        StartCoroutine(LogCoroutine());
        StopCoroutine(LogCoroutine());

    }

    IEnumerator LogCoroutine()
    {
        GameObject regenObject = Instantiate(objectPlace) as GameObject;
        regenObject.transform.SetParent(this.gameObject.transform, false);
        for (int i = 0; i < Random.Range(item_count[0], item_count[1] + 1); i++)
        {
            Instantiate(item_Prefabs, this.gameObject.transform.position + new Vector3(Random.Range(1.0f, 1.5f), Random.Range(1.0f, 1.5f), Random.Range(1.0f, 1.5f)), Quaternion.LookRotation(this.transform.parent.up * Random.Range(0.0f, 180.0f)));
        }
        var stumpExp = this.gameObject.AddComponent<Rigidbody>();
        var objectPlaceRigid = this.gameObject.GetComponent<Rigidbody>();
        objectPlaceRigid.isKinematic = true;
        objectPlaceRigid.useGravity = false;
        //Vector3 impactPoint = Random.insideUnitSphere;
        // stumpExp.AddExplosionForce(
        //                     10.0f,            // 횡 폭발력
        //                     transform.position + impactPoint * 2.0f, // 폭발 원점
        //                     2.0f,               // 폭발 반경
        //                     20.0f);           // 총 폭발력

        yield return new WaitForSeconds(destroyTime);

        //SoundManager.instance.PlaySE(logChange_sound);


        Destroy(mainObject.gameObject);
        //this.gameObject.GetComponentInParent<ItemRegen>().checkObject = false;
    }

}
