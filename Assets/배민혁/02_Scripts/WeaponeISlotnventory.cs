using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;
public class WeaponSlotInventory : MonoBehaviour
{
    public List<Item> items;
    public TextMeshProUGUI slotText;

    public GameObject player;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        FreshSlot();
    }

    void Update()
    {

        for (int i = 0; i < items.Count && i < slots.Length; i++)
        {
            slotText = slots[i].transform.GetComponentInChildren<TextMeshProUGUI>(); //slot의 text 가져오기
            for (int j = 0; j < items.Count; j++)
            {
                switch (slots[i].item.name) // 착용중인 무기 테두리표시
                {
                    case ("Axe"):
                        if (player.GetComponent<WeaponController>().activeWeaponIndex == 0)
                        {
                            slots[i].transform.GetComponent<Outline>().enabled = true;
                        }
                        else
                        {
                            slots[i].transform.GetComponent<Outline>().enabled = false;
                        }
                        break;
                    case ("PickAxe"):
                        if (player.GetComponent<WeaponController>().activeWeaponIndex == 1)
                        {
                            slots[i].transform.GetComponent<Outline>().enabled = true;
                        }
                        else
                        {
                            slots[i].transform.GetComponent<Outline>().enabled = false;
                        }
                        break;
                    case ("Gun"):
                        if (player.GetComponent<WeaponController>().activeWeaponIndex == 2)
                        {
                            slots[i].transform.GetComponent<Outline>().enabled = true;
                        }
                        else
                        {
                            slots[i].transform.GetComponent<Outline>().enabled = false;
                        }
                        break;
                    case ("Cannon"):

                        if (player.GetComponent<WeaponController>().activeWeaponIndex == 3)
                        {
                            slots[i].transform.GetComponent<Outline>().enabled = true;
                        }
                        else
                        {
                            slots[i].transform.GetComponent<Outline>().enabled = false;
                        }
                        break;

                    default:
                        break;
                }
            }
        }
        for (int i = 0; i < player.GetComponent<WeaponController>().hasWeapon.Count(); i++)///추가
        {
            if (player.GetComponent<WeaponController>().hasWeapon[i])
            {
                slots[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
            }
            else
            {
                slots[i].transform.GetChild(1).GetComponent<Image>().enabled = true;
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
    public void ResetSlotOutline()
    {
        for (int i = 0; i < items.Count && i < slots.Length; i++)
        {
            player.GetComponent<WeaponController>().activeWeaponIndex = -1;
            slots[i].transform.GetComponent<Outline>().enabled = false;
        }
    }


}