using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.Rendering;

public class HouseInventory : MonoBehaviour
{
    public List<Item> items;
    public TMP_Text slotText;


    private GameObject player;
    public GameObject Inventory;
    [SerializeField] UseHouse useHouse;

    public int rock = 0;
    public int wood = 0;
    public int leather = 0;

    void Update()
    {
        slotUpdate();
        //Debug.Log($"{rock}, {wood}, {leather}");

    }
    void slotUpdate()
    {
        string temp;
        for (int i = 0; i < items.Count && i < slots.Length; i++)
        {
            slotText = slots[i].transform.GetComponentInChildren<TMP_Text>(); //slot의 text 가져오기

            for (int j = 0; j < items.Count; j++)
            {
                switch (slots[i].item.name) // 현재 slot에 할당된 아이템의 이름에 따라 플레이어의 재료 개수를 text에 넣음
                {
                    case ("Rock"):
                        temp = $"<color=#ffffff>{rock + player.gameObject.GetComponent<PlayerState>().rock}/{useHouse.useLevelUpItem[useHouse.houseLevel].rock}</color>";
                        //temp = $"<color=#ffffff>{rock}/{useHouse.useLevelUpItem[useHouse.houseLevel].rock}</color>";
                        slotText.text = temp;
                        break;
                    case ("Wood"):
                        temp = $"<color=#ffffff>{wood + player.gameObject.GetComponent<PlayerState>().wood}/{useHouse.useLevelUpItem[useHouse.houseLevel].wood}</color>";
                        //temp = $"<color=#ffffff>{wood}/{useHouse.useLevelUpItem[useHouse.houseLevel].wood}</color>";

                        slotText.text = temp;
                        break;
                    case ("Leather"):
                        temp = $"<color=#ffffff>{leather + player.gameObject.GetComponent<PlayerState>().leather}/{useHouse.useLevelUpItem[useHouse.houseLevel].leather}</color>";
                        //temp = $"<color=#ffffff>{leather}/{useHouse.useLevelUpItem[useHouse.houseLevel].leather}</color>";
                        slotText.text = temp;
                        break;

                    default:
                        break;
                }
            }
        }
    }

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots;

#if UNITY_EDITOR
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }
#endif

    public int ListCount()
    {
        return items.Count;
    }
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        FreshSlot();
        slotUpdate();
    }

    public void FreshSlot()
    {
        int i = 0;
        for (; i < items.Count && i < slots.Length; i++)
        {
            slots[i].item = items[i];
        }
        for (; i < slots.Length; i++)
        {
            slots[i].item = null;
        }
    }

    public void AddItem(Item _item)
    {
        if (items.Count < slots.Length)
        {
            items.Add(_item);
            FreshSlot();
        }
        else
        {
            print("슬롯이 가득 차 있습니다.");
        }
    }
}