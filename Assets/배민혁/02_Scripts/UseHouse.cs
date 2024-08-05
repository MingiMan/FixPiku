using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Mono.Cecil.Cil;

public class UseHouse : MonoBehaviour
{

    public GameObject player;
    public GameObject inventory;
    public List<Item> items;
    public Inventory inevetotyScript;
    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots;


    public TextMeshProUGUI slotText;
    [SerializeField]
    private Canvas houseUI;
    // public int rock = player.GetComponent<PlayerController>().rock;  // 필요돌
    //public int wood = 0;  // 필요나무
    //public int leather = 0;  // 필요가죽

    public int houseLevel = 0;
    [SerializeField] private GameObject[] houseParts;

    void Start()
    {

        houseUI.enabled = false;
        player = GameObject.FindWithTag("Player");
        inevetotyScript = inventory.GetComponent<Inventory>();

    }

    // Update is called once per frame
    void Update()
    {
        //HouseSlot();
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
    void HouseSlot()
    {
        for (int i = 0; i < items.Count && i < slots.Length; i++)
        {
            slotText = slots[i].transform.GetComponentInChildren<TextMeshProUGUI>(); //slot의 text 가져오기
            for (int j = 0; j < items.Count; j++)
            {
                switch (slots[i].item.name) // 현재 slot에 할당된 아이템의 이름에 따라 플레이어의 재료 개수를 text에 넣음
                {
                    case ("Rock"):
                        slotText.text = $"<color=#ffffff>{player.GetComponent<PlayerState>().rock}</color>";
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
    }
    void HousePartsActive()
    {
        if (houseLevel == 1)// 집레벨 1
        {
            houseParts[0].gameObject.SetActive(true);
            houseParts[1].gameObject.SetActive(true);
            houseParts[2].gameObject.SetActive(false);
            houseParts[3].gameObject.SetActive(false);
        }
        else if (houseLevel == 2)// 집레벨 2
        {
            houseParts[0].gameObject.SetActive(true);
            houseParts[1].gameObject.SetActive(true);
            houseParts[2].gameObject.SetActive(true);
            houseParts[3].gameObject.SetActive(false);
        }
        else if (houseLevel == 3)// 집레벨 3
        {
            houseParts[0].gameObject.SetActive(true);
            houseParts[1].gameObject.SetActive(true);
            houseParts[2].gameObject.SetActive(true);
            houseParts[3].gameObject.SetActive(true);
        }
        else // 집레벨 0
        {
            houseParts[0].gameObject.SetActive(true);
            houseParts[1].gameObject.SetActive(false);
            houseParts[2].gameObject.SetActive(false);
            houseParts[3].gameObject.SetActive(false);
        }
    }
}
