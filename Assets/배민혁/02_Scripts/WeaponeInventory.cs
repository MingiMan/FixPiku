using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;
using Unity.VisualScripting;
public class WeaponInventory : MonoBehaviour
{
    public List<Item> items;
    public TextMeshProUGUI slotText;

    public GameObject player;

    [SerializeField] private Button[] weaponeActiveButton; // 무기 제작 버튼
    private bool weaponeWindowOff;
    private bool waeponeClearOff;
    [SerializeField] private GameObject weaponeWindow;

    [SerializeField] private Button weaponeWindowButton; // 무기 제작 버튼
    [SerializeField] private GameObject waeponeClear; // 무기 전부 제작

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        weaponeWindow.SetActive(false);
        weaponeWindowOff = true;
        waeponeClearOff = true;
        FreshSlot();
    }

    void OnEnable()
    {
        weaponeWindowButton.onClick.AddListener(() => // 거점 레벨업 버튼
       {
           try
           {
               // Debug.Log($"체크{playerState.wood} ,{useLevelUpItem[houseLevel].wood}");
               if (weaponeWindowOff)
               {
                   weaponeWindow.SetActive(true);
                   waeponeClear.SetActive(true);
                   weaponeWindowOff = false;
                   waeponeClearOff = false;
               }
               else
               {
                   weaponeWindow.SetActive(false);
                   waeponeClear.SetActive(false);
                   weaponeWindowOff = true;
                   waeponeClearOff = true;
               }
           }
           catch (Exception e)
           {
               Debug.Log(e.Message);
           }


       });

        weaponeActiveButton[0].onClick.AddListener(() => // 무기1
        {
            try
            {
                if (player.GetComponent<WeaponControllerBMH>().hasWeapon[0] == false)
                    player.GetComponent<WeaponControllerBMH>().hasWeapon[0] = true;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }


        });
        weaponeActiveButton[1].onClick.AddListener(() => // 무기1
       {
           try
           {
               if (player.GetComponent<WeaponControllerBMH>().hasWeapon[1] == false)
                   player.GetComponent<WeaponControllerBMH>().hasWeapon[1] = true;
           }
           catch (Exception e)
           {
               Debug.Log(e.Message);
           }


       });
        weaponeActiveButton[2].onClick.AddListener(() => // 무기1
        {
            try
            {
                if (player.GetComponent<WeaponControllerBMH>().hasWeapon[2] == false)
                    player.GetComponent<WeaponControllerBMH>().hasWeapon[2] = true;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }


        });
        weaponeActiveButton[3].onClick.AddListener(() => // 무기1
       {
           try
           {
               if (player.GetComponent<WeaponControllerBMH>().hasWeapon[3] == false)
                   player.GetComponent<WeaponControllerBMH>().hasWeapon[3] = true;
           }
           catch (Exception e)
           {
               Debug.Log(e.Message);
           }


       });


    }
    void Update()
    {

        for (int i = 0; i < player.GetComponent<WeaponControllerBMH>().hasWeapon.Count(); i++)///추가////////////////////////////////
        {
            if (player.GetComponent<WeaponControllerBMH>().hasWeapon[i])
            {
                slots[i].transform.GetChild(3).GetComponent<Image>().enabled = true;
                weaponeActiveButton[i].GetComponent<Button>().enabled = false;
            }
            else
            {
                slots[i].transform.GetChild(3).GetComponent<Image>().enabled = false;
                weaponeActiveButton[i].GetComponent<Button>().enabled = true;
            }
        }
        if (!waeponeClearOff && !weaponeActiveButton[0].GetComponent<Button>().enabled && !weaponeActiveButton[1].GetComponent<Button>().enabled && !weaponeActiveButton[2].GetComponent<Button>().enabled && !weaponeActiveButton[3].GetComponent<Button>().enabled)
        {
            waeponeClear.SetActive(true);
        }
        else
        {
            waeponeClear.SetActive(false);
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
}