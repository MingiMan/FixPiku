using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class EnergeSystem : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
{
    public List<Item> items;
    public TMP_Text slotText;
    private GameObject player;
    private PlayerState playerState;
    private HouseInventory houseInventory;
    [Serializable]
    public struct chargeItem
    {
        public int rock;
        public int wood;
        public int leather;

    }
    public chargeItem useChargeItem;
    [SerializeField] private TMP_Text energeText;
    [SerializeField] private Button chargeEnergeButton; //에너지 충전 버튼
    //private WeaponController weaponController;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        houseInventory = GameObject.Find("House").GetComponentInChildren<HouseInventory>();
        playerState = player.gameObject.GetComponent<PlayerState>();
        //weaponController = FindObjectOfType<WeaponController>();
        FreshSlot();

    }
    void Update()
    {
        if (playerState.inHouse) SlotUpdateInHouse();
        else SlotUpdateNNotInHouse();
        energeText.text = $"<color=#000000>Energe :</color> <color=#00ffff>{playerState.energe}</color>";

    }
    void OnEnable()
    {
        chargeEnergeButton.onClick.AddListener(() => // 거점 레벨업 버튼
       {
           try
           {
               // Debug.Log($"체크{playerState.wood} ,{useLevelUpItem[houseLevel].wood}");
               if (ChargeItemCheck())
               {
                   Debug.Log("충전");
                   ChargeEnergeClick();
               }
           }
           catch (Exception e)
           {
               Debug.Log(e.Message);
           }
       });

    }
    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     Debug.Log("Enter");
    //     weaponController.attackActive = false;
    // }
    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     Debug.Log("Exit");
    //     weaponController.attackActive = true;
    // }
    void SlotUpdateInHouse()
    {
        string temp;
        string textColor;
        for (int i = 0; i < items.Count && i < slots.Length; i++)
        {
            slotText = slots[i].transform.GetComponentInChildren<TMP_Text>(); //slot의 text 가져오기

            for (int j = 0; j < items.Count; j++)
            {
                switch (slots[i].item.name) // 현재 slot에 할당된 아이템의 이름에 따라 플레이어의 재료 개수를 text에 넣음
                {
                    case ("Rock"):
                        if (playerState.rock + houseInventory.rock < useChargeItem.rock) textColor = "<color=#ff0000>";
                        else textColor = "<color=#ffffff>";

                        temp = textColor + $"{playerState.rock + houseInventory.rock}</color>/<color=#ffffff>{useChargeItem.rock}</color>";
                        slotText.text = temp;
                        break;
                    case ("Wood"):
                        if (playerState.wood + houseInventory.wood < useChargeItem.wood) textColor = "<color=#ff0000>";
                        else textColor = "<color=#ffffff>";

                        temp = textColor + $"{playerState.wood + houseInventory.wood}</color>/<color=#ffffff>{useChargeItem.wood}</color>";
                        slotText.text = temp;
                        break;
                    case ("Leather"):
                        if (playerState.leather + houseInventory.leather < useChargeItem.leather) textColor = "<color=#ff0000>";
                        else textColor = "<color=#ffffff>";

                        temp = textColor + $"{playerState.leather + houseInventory.leather}</color>/<color=#ffffff>{useChargeItem.leather}</color>";
                        slotText.text = temp;
                        break;

                    default:
                        break;
                }
            }
        }
    }
    void SlotUpdateNNotInHouse()
    {
        string temp;
        string textColor;
        for (int i = 0; i < items.Count && i < slots.Length; i++)
        {
            slotText = slots[i].transform.GetComponentInChildren<TMP_Text>(); //slot의 text 가져오기

            for (int j = 0; j < items.Count; j++)
            {
                switch (slots[i].item.name) // 현재 slot에 할당된 아이템의 이름에 따라 플레이어의 재료 개수를 text에 넣음
                {
                    case ("Rock"):
                        if (playerState.rock < useChargeItem.rock) textColor = "<color=#ff0000>";
                        else textColor = "<color=#ffffff>";

                        temp = textColor + $"{playerState.rock}</color>/<color=#ffffff>{useChargeItem.rock}</color>";
                        slotText.text = temp;
                        break;
                    case ("Wood"):
                        if (playerState.wood < useChargeItem.wood) textColor = "<color=#ff0000>";
                        else textColor = "<color=#ffffff>";

                        temp = textColor + $"{playerState.wood}</color>/<color=#ffffff>{useChargeItem.wood}</color>";
                        slotText.text = temp;
                        break;
                    case ("Leather"):
                        if (playerState.leather < useChargeItem.leather) textColor = "<color=#ff0000>";
                        else textColor = "<color=#ffffff>";

                        temp = textColor + $"{playerState.leather}</color>/<color=#ffffff>{useChargeItem.leather}</color>";
                        slotText.text = temp;
                        break;

                    default:
                        break;
                }
            }
        }
    }

    public void ChargeEnergeClick()
    {
        if (playerState.rock > useChargeItem.rock)
        {
            playerState.rock -= useChargeItem.rock;
        }
        else
        {
            houseInventory.rock = houseInventory.rock + playerState.rock - useChargeItem.rock;
            playerState.rock = 0;
        }
        if (playerState.wood > useChargeItem.wood)
        {
            playerState.wood -= useChargeItem.wood;
        }
        else
        {
            houseInventory.wood = houseInventory.wood + playerState.wood - useChargeItem.wood;
            playerState.wood = 0;
        }
        if (playerState.leather > useChargeItem.leather)
        {
            playerState.leather -= useChargeItem.leather;
        }
        else
        {
            houseInventory.leather = houseInventory.leather + playerState.leather - useChargeItem.leather;
            playerState.leather = 0;
        }
        SoundManager.instance.PlaySFX("HouseLevelUp");
        playerState.energe += 10;

    }
    public bool ChargeItemCheck()
    {
        if (playerState.inHouse)
        {
            if (playerState.rock + houseInventory.rock >= useChargeItem.rock && playerState.wood + houseInventory.wood >= useChargeItem.wood && playerState.leather + houseInventory.leather >= useChargeItem.leather)
            {
                return true;
            }
        }
        else if (!playerState.inHouse)
        {
            if (playerState.rock >= useChargeItem.rock && playerState.wood >= useChargeItem.wood && playerState.leather >= useChargeItem.leather)
            {
                return true;
            }
        }

        return false;
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
