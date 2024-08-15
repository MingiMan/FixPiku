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
    public PlayerState playerState;

    [SerializeField] private Button[] weaponeActiveButton; // 무기 제작 버튼
    private bool weaponeWindowOff;
    private bool waeponeClearOff;
    [SerializeField] private GameObject weaponeWindow;

    [SerializeField] private Button weaponeWindowButton; // 무기 제작 버튼
    [SerializeField] private GameObject waeponeClear; // 무기 전부 제작
    [Serializable]
    public struct weaponeActiveItem
    {
        public int rock;
        public int wood;
        public int leather;

    }
    public weaponeActiveItem[] useWeaponeActiveItem;
    [SerializeField] HouseInventory houseInventory;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerState = player.gameObject.GetComponent<PlayerState>();
                
        weaponeWindow.SetActive(false);
        weaponeWindowOff = true;
        waeponeClearOff = true;
        FreshSlot();
    }

    void OnEnable()
    {
        weaponeWindowButton.onClick.AddListener(() => // 무기 창 활성화 버튼
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

        weaponeActiveButton[0].onClick.AddListener(() => // 무기0
        {
            // try
            // {
            //     if (player.GetComponent<WeaponController>().hasWeapon[0] == false)
            //         player.GetComponent<WeaponController>().hasWeapon[0] = true;
            // }
            try
            {
                if (player.GetComponent<WeaponController>().hasWeapon[0] == false)
                {
                    if (weaponeActiveItemCheck(0))
                    {
                        weaponeActiveClick(0);
                    }

                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }


        });
        weaponeActiveButton[1].onClick.AddListener(() => // 무기1
       {
           //    try
           //    {
           //        if (player.GetComponent<WeaponController>().hasWeapon[1] == false)
           //            player.GetComponent<WeaponController>().hasWeapon[1] = true;
           //    }
           //    catch (Exception e)
           try
           {
               if (player.GetComponent<WeaponController>().hasWeapon[1] == false)
               {
                   if (weaponeActiveItemCheck(1))
                   {
                       weaponeActiveClick(1);
                   }

               }
           }
           catch (Exception e)
           {
               Debug.Log(e.Message);
           }


       });
        weaponeActiveButton[2].onClick.AddListener(() => // 무기2
        {
            // try
            // {
            //     if (player.GetComponent<WeaponController>().hasWeapon[2] == false)
            //         player.GetComponent<WeaponController>().hasWeapon[2] = true;
            // }
            try
            {
                if (player.GetComponent<WeaponController>().hasWeapon[2] == false)
                {
                    if (weaponeActiveItemCheck(2))
                    {
                        weaponeActiveClick(2);
                    }

                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }


        });
        weaponeActiveButton[3].onClick.AddListener(() => // 무기2=3
       {
           //    try
           //    {
           //        if (player.GetComponent<WeaponController>().hasWeapon[3] == false)
           //            player.GetComponent<WeaponController>().hasWeapon[3] = true;
           //    }
           try
           {
               if (player.GetComponent<WeaponController>().hasWeapon[3] == false)
               {
                   if (weaponeActiveItemCheck(3))
                   {
                       weaponeActiveClick(3);
                   }

               }
           }
           catch (Exception e)
           {
               Debug.Log(e.Message);
           }


       });


    }
    void Update()
    {

        for (int i = 0; i < player.GetComponent<WeaponController>().hasWeapon.Count(); i++)
        {
            if (player.GetComponent<WeaponController>().hasWeapon[i])
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
    #region 무기 해금 부분
    public void weaponeActiveClick(int waponeNumber)
    {

        if (playerState.rock > useWeaponeActiveItem[waponeNumber].rock)
        {
            playerState.rock -= useWeaponeActiveItem[waponeNumber].rock;
        }
        else
        {
            houseInventory.rock = houseInventory.rock + playerState.rock - useWeaponeActiveItem[waponeNumber].rock;
            playerState.rock = 0;
        }
        if (playerState.wood > useWeaponeActiveItem[waponeNumber].wood)
        {
            playerState.wood -= useWeaponeActiveItem[waponeNumber].wood;
        }
        else
        {
            houseInventory.wood = houseInventory.wood + playerState.wood - useWeaponeActiveItem[waponeNumber].wood;
            playerState.wood = 0;
        }
        if (playerState.leather > useWeaponeActiveItem[waponeNumber].leather)
        {
            playerState.leather -= useWeaponeActiveItem[waponeNumber].leather;
        }
        else
        {
            houseInventory.leather = houseInventory.leather + playerState.leather - useWeaponeActiveItem[waponeNumber].leather;
            playerState.leather = 0;
        }
        SoundManager.instance.PlaySFX("HouseLevelUp");
        player.GetComponent<WeaponController>().hasWeapon[waponeNumber] = true;

    }
    public bool weaponeActiveItemCheck(int waponeNumber)
    {
        if (playerState.rock + houseInventory.rock >= useWeaponeActiveItem[waponeNumber].rock && playerState.wood + houseInventory.wood >= useWeaponeActiveItem[waponeNumber].wood && playerState.leather + houseInventory.leather >= useWeaponeActiveItem[waponeNumber].leather)
        {
            return true;
        }

        return false;
    }
    #endregion
}