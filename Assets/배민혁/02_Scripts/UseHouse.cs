using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Mono.Cecil.Cil;
<<<<<<< HEAD
=======
using UnityEngine.UI;
using System;
>>>>>>> dd014889354572f0abb1d3a42768d4434be9d3a4

public class UseHouse : MonoBehaviour
{

    public GameObject player;
    public GameObject inventory;
    public List<Item> items;
    public Inventory inevetotyScript;
<<<<<<< HEAD
    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots;
=======
    [SerializeField] private Transform slotParent;
    [SerializeField] private Slot[] slots;

    [SerializeField] private Button LevelUpButton; //거점레벨업 버튼
    [SerializeField] private Button ItemSaveButton; // 거점 아이템 저장버튼
    [SerializeField] int houseLevelLimit;

>>>>>>> dd014889354572f0abb1d3a42768d4434be9d3a4


    public TextMeshProUGUI slotText;
    [SerializeField]
<<<<<<< HEAD
    private Canvas HouseUI;
    // public int rock = player.GetComponent<PlayerController>().rock;  // 필요돌
    //public int wood = 0;  // 필요나무
    //public int leather = 0;  // 필요가죽

    void Start()
    {

        HouseUI.enabled = false;
=======
    private Canvas houseUI;
    //public int rock = 0;  // 필요돌
    //public int wood = 0;  // 필요나무
    //public int leather = 0;  // 필요가죽

    [Serializable]
    public struct levelUpItem
    {
        public int rock;
        public int wood;
        public int leather;

    }

    public int houseLevel = 0;
    [SerializeField] private GameObject[] houseParts;
    [SerializeField] private levelUpItem[] useLevelUpItem;

    void Start()
    {

        houseUI.enabled = false;
>>>>>>> dd014889354572f0abb1d3a42768d4434be9d3a4
        player = GameObject.FindWithTag("Player");
        inevetotyScript = inventory.GetComponent<Inventory>();

    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        for (int i = 0; i < items.Count && i < slots.Length; i++)
        {
            slotText = slots[i].transform.GetComponentInChildren<TextMeshProUGUI>(); //slot의 text 가져오기
            for (int j = 0; j < items.Count; j++)
            {
                switch (slots[i].item.name) // 현재 slot에 할당된 아이템의 이름에 따라 플레이어의 재료 개수를 text에 넣음
                {
                    case ("Rock"):
                        slotText.text = $"<color=#000000>{player.GetComponent<PlayerState>().rock}</color>";
                        break;
                    case ("Wood"):
                        slotText.text = $"<color=#ffffff>{player.GetComponent<PlayerState>().wood}</color>";
                        break;
                    case ("Leather"):
                        slotText.text = $"<color=#ffffff>{player.GetComponent<PlayerState>().leather}</color>";
                        break;

                    default:
                        break;
                }
            }
        }
=======
        HousePartsActive();

>>>>>>> dd014889354572f0abb1d3a42768d4434be9d3a4
    }
    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
<<<<<<< HEAD
            Debug.Log("stay");
            HouseUI.enabled = true;
=======
            //Debug.Log("stay");
            houseUI.enabled = true;
>>>>>>> dd014889354572f0abb1d3a42768d4434be9d3a4
        }


    }
    void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
<<<<<<< HEAD
            Debug.Log("out");
            HouseUI.enabled = false;
        }

    }
=======
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
            houseParts[4].gameObject.SetActive(false);
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
            houseParts[5].gameObject.SetActive(false);
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
            houseParts[6].gameObject.SetActive(false);
        }
        else if (houseLevel == 7)// 집레벨 7
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
        if (houseLevel < houseLevelLimit)
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
>>>>>>> dd014889354572f0abb1d3a42768d4434be9d3a4
}
