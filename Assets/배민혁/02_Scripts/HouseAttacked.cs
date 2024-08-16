using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HouseAttacked : MonoBehaviour
{

    [SerializeField] private UseHouse useHouse;             //집 레벨 확인용
    [SerializeField] private HouseInventory houseInventory; //집 가진 자원 확인용

    //[SerializeField] private float MaxHouseHP = 1000.0f;
    [SerializeField] private float[] maxHouseHP;
    public float currentHouseHP;

    [SerializeField] private GameObject houseHpBar;
    [SerializeField] private TMP_Text houseHpBarText;
    [SerializeField] private GameObject looseText;


    [SerializeField] private float houseHPRecovery = 10.0f;

    [SerializeField] private float houseHPRecoveryTime;

    bool recoveryStart = false;

    void Awake()
    {
        HouseCurrentHpSet();
        looseText.SetActive(false);
    }
    void Update()
    {
        HpBarSet();
        if (currentHouseHP > maxHouseHP[useHouse.houseLevel])
        {
            HouseCurrentHpSet();
        }
        if (currentHouseHP >= maxHouseHP[useHouse.houseLevel] && currentHouseHP <= 0.0f && recoveryStart)
        {
            recoveryStart = false;
        }
        else if (currentHouseHP < maxHouseHP[useHouse.houseLevel] && currentHouseHP > 0.0f && !recoveryStart)
        {
            StartCoroutine(HouseHPRecovery());
            recoveryStart = true;
        }
    }
    public void OnTriggerEnter(Collider coll) // 적이 공격할 때 데미지 받음
    {
        if (coll.name == "Melee" || coll.name == "MeleeArea")
        {
            float damgePoint = coll.gameObject.GetComponentInParent<Monsters>().Stats.atkDamage;
            Debug.Log(damgePoint);
            currentHouseHP -= damgePoint;
            //currentHouseHP += -10.0f;
            //coll. 적 데미지 수치 확인필요
            if (currentHouseHP <= 0.0f)
            {
                LooseHouseItem();
                LooseHouseText();
            }


        }
    }
    public void LooseHouseItem()
    {
        if (useHouse.houseLevel > 0) useHouse.houseLevel -= 1; // 레벨 0이상일 경우 1레벨 다운
        houseInventory.rock = 0;        // 집의 자원 없어짐
        houseInventory.wood = 0;
        houseInventory.leather = 0;
        HouseCurrentHpSet();            //hp최대치로 변경
        this.gameObject.GetComponent<BoxCollider>().enabled = false; // 적이 죽은 후 타격 방지를 위한 박스콜리더 비활성화

    }
    public void LooseHouseText()
    {
        TimeManager.Instance.Hours = 6; //강제로 다음 아침으로 넘어가기
        GameManager.Instance.level = 1; // 웨이브 난이도 1로 돌아감
        StartCoroutine(LooseDefense()); //집 무너짐 문구 및 박스콜리더 재활성화
    }
    public void HouseCurrentHpSet()// 하우스 현재체력 최대체력으로 초기화
    {
        currentHouseHP = maxHouseHP[useHouse.houseLevel];
    }

    void HpBarSet() // hp바 게이지와 숫자 초기화
    {
        houseHpBar.GetComponent<Slider>().value = (currentHouseHP / maxHouseHP[useHouse.houseLevel]);

        string temp = $"<color=#ffffff>{(int)currentHouseHP}</color>/<color=#ffffff>{maxHouseHP[useHouse.houseLevel]}</color>";
        houseHpBarText.text = temp;
    }
    IEnumerator HouseHPRecovery() // 하우스 체력 자동회복
    {
        if (houseInventory.rock > 0 && houseInventory.wood > 0 && houseInventory.leather > 0)
        {
            if (currentHouseHP < maxHouseHP[useHouse.houseLevel] && currentHouseHP > 0.0f && recoveryStart)
            {
                houseInventory.rock -= 1;
                houseInventory.wood -= 1;
                houseInventory.leather -= 1;
                currentHouseHP += 10.0f * (useHouse.houseLevel + 1);
                Debug.Log(currentHouseHP);
            }
        }
        yield return new WaitForSeconds(houseHPRecoveryTime);
        if (recoveryStart) StartCoroutine(HouseHPRecovery());
        else recoveryStart = false;



    }
    IEnumerator LooseDefense() // 집 지키기 실패시 문구 출력 및 비활성화된 박스 콜리더 활성화
    {
        looseText.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        looseText.SetActive(false);
        this.gameObject.GetComponent<BoxCollider>().enabled = true;


    }


}
