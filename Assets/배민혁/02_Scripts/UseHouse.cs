using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Mono.Cecil.Cil;
using UnityEngine.UI;
using System;

public class UseHouse : MonoBehaviour
{

    public GameObject player;
    public GameObject inventory;
    public List<Item> items;
    [SerializeField] private Transform slotParent;
    [SerializeField] private Slot[] slots;

    [SerializeField] private Button LevelUpButton; //거점레벨업 버튼
    [SerializeField] private Button ItemSaveButton; // 거점 아이템 저장버튼
    [SerializeField] int houseLevelLimit;
    [SerializeField] HouseInventory houseInventory;
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
            houseUI.enabled = true;
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

    void OnEnable()
    {
        LevelUpButton.onClick.AddListener(() => // 거점 레벨업 버튼
       {
           try
           {
               Debug.Log($"체크{player.gameObject.GetComponent<PlayerState>().wood} ,{useLevelUpItem[houseLevel].wood}");
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
            houseParts[1].gameObject.SetActive(false);
            houseParts[2].gameObject.SetActive(false);
            houseParts[3].gameObject.SetActive(false);
            houseParts[4].gameObject.SetActive(false);
            houseParts[5].gameObject.SetActive(false);
            houseParts[6].gameObject.SetActive(false);

        }
    }
    public void HouseLevelUpClick()
    {
        if (houseLevel < houseLevelLimit - 1)
        {
            houseLevel += 1;
        }
    }
    public bool LevelUpItemCheck()
    {
        if (player.gameObject.GetComponent<PlayerState>().wood >= useLevelUpItem[houseLevel].wood)
        {
            return true;
        }

        return false;
    }
}
