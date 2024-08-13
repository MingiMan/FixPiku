using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;
using Unity.VisualScripting;
public class UseWeaponInventory : MonoBehaviour
{
    public List<Item> items;
    public TMP_Text slotText;

    public GameObject player;
    public PlayerState playerState;

    [SerializeField] WeaponInventory weaponInventory;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerState = player.gameObject.GetComponent<PlayerState>();
        FreshSlot();
    }
    void Update()
    {
        HouseSlotUpdate();
    }
    void HouseSlotUpdate()
    {
        string temp;
        for (int i = 0; i < items.Count && i < slots.Length; i++)
        {
            slotText = slots[i].transform.GetComponentInChildren<TMP_Text>(); //slot의 text 가져오기
            switch (slots[i].item.name) // 현재 slot에 할당된 아이템의 이름에 따라 플레이어의 재료 개수를 text에 넣음
            {
                case ("Rock"):
                    temp = $"<color=#000000>{weaponInventory.useWeaponeActiveItem[this.gameObject.transform.parent.gameObject.transform.GetSiblingIndex()].rock}</color>";
                    //temp = $"<color=#ffffff>{rock}/{useHouse.useLevelUpItem[useHouse.houseLevel].rock}</color>";
                    slotText.text = temp;
                    break;
                case ("Wood"):
                    temp = $"<color=#000000>{weaponInventory.useWeaponeActiveItem[this.gameObject.transform.parent.gameObject.transform.GetSiblingIndex()].wood}</color>";
                    //temp = $"<color=#ffffff>{wood}/{useHouse.useLevelUpItem[useHouse.houseLevel].wood}</color>";
                    slotText.text = temp;
                    break;
                case ("Leather"):
                    temp = $"<color=#000000>{weaponInventory.useWeaponeActiveItem[this.gameObject.transform.parent.gameObject.transform.GetSiblingIndex()].leather}</color>";
                    //temp = $"<color=#ffffff>{leather}/{useHouse.useLevelUpItem[useHouse.houseLevel].leather}</color>";
                    slotText.text = temp;
                    break;

                default:
                    break;
            }

        }
    }

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private UseSlot[] slots;

#if UNITY_EDITOR
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<UseSlot>();
    }
#endif

    public int ListCount()
    {
        return items.Count;
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