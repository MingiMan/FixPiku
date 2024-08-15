using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class UseHouse : MonoBehaviour
{

    public GameObject player;
    public PlayerState playerState;
    public GameObject inventory;

    [SerializeField] private Button LevelUpButton; //거점레벨업 버튼
    [SerializeField] private Button ItemSaveButton; // 거점 아이템 저장버튼
    [SerializeField] int houseLevelLimit;
    [SerializeField] HouseInventory houseInventory;
    [SerializeField] HouseAttacked houseAttacked;

    public TextMeshProUGUI slotText;
    [SerializeField]
    private Canvas houseUI;

    [Serializable]
    public struct levelUpItem
    {
        public int rock;
        public int wood;
        public int leather;

    }

    public int houseLevel = 0;
    [SerializeField] private GameObject[] houseParts;
    public levelUpItem[] useLevelUpItem;


    void Start()
    {

        houseUI.enabled = false;
        player = GameObject.FindWithTag("Player");

        playerState = player.gameObject.GetComponent<PlayerState>();

    }

    // Update is called once per frame
    void Update()
    {
        HousePartsActive();

    }
    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            //Debug.Log("stay");
            if (!TimeManager.Instance.nightCheck) houseUI.enabled = true;
            else houseUI.enabled = false;
        }

    }
    void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            //Debug.Log("out");
            houseUI.enabled = false;
        }

    }
    void OnTriggerStay(Collider coll)
    {
        if (coll.CompareTag("Player") && TimeManager.Instance.nightCheck)
        {
            //Debug.Log("out");
            houseUI.enabled = false;
        }

    }


    void OnEnable()
    {
        LevelUpButton.onClick.AddListener(() => // 거점 레벨업 버튼
       {
           try
           {
               // Debug.Log($"체크{playerState.wood} ,{useLevelUpItem[houseLevel].wood}");
               if (LevelUpItemCheck())
               {
                   Debug.Log("레벨업");
                   HouseLevelUpClick();
               }
           }
           catch (Exception e)
           {
               Debug.Log(e.Message);
           }


       });
        ItemSaveButton.onClick.AddListener(() => // 거점 아이템 저장 버튼
        {
            try
            {
                // Debug.Log($"체크{playerState.wood} ,{useLevelUpItem[houseLevel].wood}");
                if (playerState.rock > 0 || playerState.wood > 0 || playerState.leather > 0)
                {
                    Debug.Log("저장");
                    HouseSaveItem();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }


        });
    }
    void HousePartsActive()
    {
        if (houseLevel == 1)// 집레벨 1
        {
            houseParts[0].gameObject.SetActive(true);
            houseParts[1].gameObject.SetActive(true);
            houseParts[2].gameObject.SetActive(false);
            houseParts[3].gameObject.SetActive(false);
            houseParts[4].gameObject.SetActive(false);
            houseParts[5].gameObject.SetActive(false);
            houseParts[6].gameObject.SetActive(false);
            houseParts[1].gameObject.transform.localPosition = new Vector3(0, 0, 0);
        }
        else if (houseLevel == 2)// 집레벨 2
        {
            houseParts[0].gameObject.SetActive(true);
            houseParts[1].gameObject.SetActive(true);
            houseParts[2].gameObject.SetActive(true);
            houseParts[3].gameObject.SetActive(false);
            houseParts[4].gameObject.SetActive(false);
            houseParts[5].gameObject.SetActive(false);
            houseParts[6].gameObject.SetActive(false);
        }
        else if (houseLevel == 3)// 집레벨 3
        {
            houseParts[0].gameObject.SetActive(true);
            houseParts[1].gameObject.SetActive(true);
            houseParts[2].gameObject.SetActive(true);
            houseParts[3].gameObject.SetActive(true);
            houseParts[4].gameObject.SetActive(false);
            houseParts[5].gameObject.SetActive(false);
            houseParts[6].gameObject.SetActive(false);
        }
        else if (houseLevel == 4)// 집레벨 4
        {
            houseParts[0].gameObject.SetActive(true);
            houseParts[1].gameObject.SetActive(true);
            houseParts[2].gameObject.SetActive(true);
            houseParts[3].gameObject.SetActive(true);
            houseParts[4].gameObject.SetActive(true);
            houseParts[5].gameObject.SetActive(false);
            houseParts[6].gameObject.SetActive(false);
        }
        else if (houseLevel == 5)// 집레벨 5
        {
            houseParts[0].gameObject.SetActive(true);
            houseParts[1].gameObject.SetActive(true);
            houseParts[2].gameObject.SetActive(true);
            houseParts[3].gameObject.SetActive(true);
            houseParts[4].gameObject.SetActive(true);
            houseParts[5].gameObject.SetActive(true);
            houseParts[6].gameObject.SetActive(false);
        }
        else if (houseLevel == 6)// 집레벨 6
        {
            houseParts[0].gameObject.SetActive(true);
            houseParts[1].gameObject.SetActive(true);
            houseParts[2].gameObject.SetActive(true);
            houseParts[3].gameObject.SetActive(true);
            houseParts[4].gameObject.SetActive(true);
            houseParts[5].gameObject.SetActive(true);
            houseParts[6].gameObject.SetActive(true);
        }
        else // 집레벨 0
        {
            houseParts[0].gameObject.SetActive(true);
            houseParts[1].gameObject.SetActive(true);
            houseParts[2].gameObject.SetActive(false);
            houseParts[3].gameObject.SetActive(false);
            houseParts[4].gameObject.SetActive(false);
            houseParts[5].gameObject.SetActive(false);
            houseParts[6].gameObject.SetActive(false);
            houseParts[1].gameObject.transform.localPosition = new Vector3(0, -1.5f, 0);
        }
    }
    public void HouseLevelUpClick()
    {
        if (houseLevel < houseLevelLimit - 1)
        {
            // houseInventory.rock -= useLevelUpItem[houseLevel].rock;
            // houseInventory.wood -= useLevelUpItem[houseLevel].wood;
            // houseInventory.leather -= useLevelUpItem[houseLevel].leather;
            if (playerState.rock > useLevelUpItem[houseLevel].rock)
            {
                playerState.rock -= useLevelUpItem[houseLevel].rock;
            }
            else
            {
                houseInventory.rock = houseInventory.rock + playerState.rock - useLevelUpItem[houseLevel].rock;
                playerState.rock = 0;
            }
            if (playerState.wood > useLevelUpItem[houseLevel].wood)
            {
                playerState.wood -= useLevelUpItem[houseLevel].wood;
            }
            else
            {
                houseInventory.wood = houseInventory.wood + playerState.wood - useLevelUpItem[houseLevel].wood;
                playerState.wood = 0;
            }
            if (playerState.leather > useLevelUpItem[houseLevel].leather)
            {
                playerState.leather -= useLevelUpItem[houseLevel].leather;
            }
            else
            {
                houseInventory.leather = houseInventory.leather + playerState.leather - useLevelUpItem[houseLevel].leather;
                playerState.leather = 0;
            }
            SoundManager.instance.PlaySFX("HouseLevelUp");
            houseLevel += 1;
            houseAttacked.HouseCurrentHpSet();
        }
    }
    public bool LevelUpItemCheck()
    {
        if (playerState.rock + houseInventory.rock >= useLevelUpItem[houseLevel].rock && playerState.wood + houseInventory.wood >= useLevelUpItem[houseLevel].wood && playerState.leather + houseInventory.leather >= useLevelUpItem[houseLevel].leather)
        {
            return true;
        }

        return false;
    }

    public void HouseSaveItem()
    {
        houseInventory.rock += playerState.rock;
        houseInventory.wood += playerState.wood;
        houseInventory.leather += playerState.leather;
        playerState.rock = 0;
        playerState.wood = 0;
        playerState.leather = 0;
        SoundManager.instance.PlaySFX("SaveItem");
    }
}
