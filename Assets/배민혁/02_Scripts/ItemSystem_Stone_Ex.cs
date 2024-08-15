using UnityEngine;
using System.Collections;
using System.Linq;

public class ItemSystemStoneEx : MonoBehaviour
{

    private float rockHp = 100;
    public GameObject player;
    private float damagePoint;

    [SerializeField] private GameObject parentObject;   // position 참조용

    [SerializeField] private GameObject item_Prefabs;  // 통나무. 나무가 쓰러진 이후 생성할 재료.

    [SerializeField] private float force;  // 나무가 땅에 쓰러지도록 밀어줄 힘의 세기(랜덤으로 정할 것) 
    [SerializeField] private GameObject mainObject;  // 쓰러질 오브젝트 전체 부분. 쓰러지고 난 다음에 지연 시간 후 파괴 되야 해서 필요함. 본체가 날아가고 밑부분이 남을 예정

    [SerializeField] private GameObject[] childStone;  // 부서질 돌조각. 부서진 다음에 지연 시간 후 파괴되야 해서 필요함.
    private GameObject[] CreateChildStone;  //생성된 돌조각들 저장
    [SerializeField] private GameObject objectPlace; // 바위가 있던 자리 오브젝트

    [SerializeField] private CapsuleCollider parentCol;  // 전체적인 바위에 붙어있는 캡슐 콜라이더. 바위가 부서지면 이걸 비활성화 해주어야 함.
    //[SerializeField] private CapsuleCollider[] childCol;  // 부서지는 바위인 바위 윗 부분에 붙어있는 캡슐 콜라이더. 바위가 부서지면 이걸 활성화 해주어야 함.
    //[SerializeField] private Rigidbody[] childRigid; // 부서지는 바위인 바위 윗 부분에 붙어있는 Rigidbody를 통해 바위가 부서지면 중력을 활성화 해주어야 함.

    [SerializeField] private float[] item_count;  // 아이템 생성 최소 최대 개수

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
        CreateChildStone = new GameObject[childStone.Count()];


    }

    void OnTriggerEnter(Collider coll)
    {
        if (stoneActive)
        {
            if (coll.CompareTag("MELEE"))
            {
                damagePoint = coll.gameObject.GetComponent<Weapon>().rockDamage;
                //Hit(_pos);
                rockHp -= damagePoint;
                SoundManager.instance.PlaySFX(chop_sound);
                Debug.Log(rockHp);
                if (rockHp <= 0 && parentCol.enabled)
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
        SoundManager.instance.PlaySFX(falldown_sound);
        Destroy(mainObject.gameObject);
        parentCol.enabled = false;
        for (int i = 0; i < childStone.Count(); i++)
        {
            parentCol.enabled = false;

            GameObject regenObject = Instantiate(childStone[i], this.gameObject.transform.up + new Vector3(Random.Range(1.0f, 1.5f), Random.Range(1.0f, 1.5f), Random.Range(1.0f, 1.5f)), this.gameObject.transform.rotation) as GameObject;
            regenObject.transform.SetParent(this.gameObject.transform, false);
            CreateChildStone[i] = regenObject;
            regenObject.GetComponent<CapsuleCollider>().enabled = true;
            regenObject.GetComponent<Rigidbody>().useGravity = true;
            regenObject.GetComponent<Rigidbody>().AddForce(Random.Range(-force, force), Random.Range(-force, force), Random.Range(-force, force));
        }
        StartCoroutine(LogCoroutine());
        StopCoroutine(LogCoroutine());

    }

    IEnumerator LogCoroutine()
    {
        GameObject regenObject = Instantiate(objectPlace) as GameObject;
        regenObject.transform.SetParent(this.gameObject.transform, false);
        for (int i = 0; i < Random.Range(item_count[0], item_count[1] + 1); i++)
        {
            Instantiate(item_Prefabs, this.gameObject.transform.parent.position + new Vector3(Random.Range(1.0f, 1.5f), Random.Range(1.0f, 1.5f), Random.Range(1.0f, 1.5f)), Quaternion.LookRotation(this.transform.parent.up * Random.Range(0.0f, 180.0f)));
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

        for (int i = 0; i < CreateChildStone.Count(); i++)
        {
            Destroy(CreateChildStone[i].gameObject);
        }

        //this.gameObject.GetComponentInParent<ItemRegen>().checkObject = false;
    }

}
