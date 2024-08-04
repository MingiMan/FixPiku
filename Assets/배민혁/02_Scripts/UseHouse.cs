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
    private Canvas HouseUI;
    // public int rock = player.GetComponent<PlayerController>().rock;  // 필요돌
    //public int wood = 0;  // 필요나무
    //public int leather = 0;  // 필요가죽

    void Start()
    {

        HouseUI.enabled = false;
        player = GameObject.FindWithTag("Player");
        inevetotyScript = inventory.GetComponent<Inventory>();

    }

    // Update is called once per frame
    void Update()
    {
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
    }
    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            Debug.Log("stay");
            HouseUI.enabled = true;
        }


    }
    void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            Debug.Log("out");
            HouseUI.enabled = false;
        }

    }
}
