using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HouseInventory : MonoBehaviour
{
    public List<Item> items;
    public TMP_Text slotText;


    private GameObject player;
    [SerializeField] UseHouse useHouse;

    public int rock = 0;
    public int wood = 0;
    public int leather = 0;

    void Update()
    {
        QuestSlotUpdate();
        HouseSlotUpdate();
        //Debug.Log($"{rock}, {wood}, {leather}");

    }
    void QuestSlotUpdate()
    {
        string temp;
        string textColor;
        for (int i = 0; i < items.Count && i < QuestSlots.Length; i++)
        {
            slotText = QuestSlots[i].transform.GetComponentInChildren<TMP_Text>(); //slot의 text 가져오기

            for (int j = 0; j < items.Count; j++)
            {
                switch (QuestSlots[i].item.name) // 현재 slot에 할당된 아이템의 이름에 따라 플레이어의 재료 개수를 text에 넣음
                {
                    case ("Rock"):
                        if (rock + player.gameObject.GetComponent<PlayerState>().rock < useHouse.useLevelUpItem[useHouse.houseLevel].rock) textColor = "<color=#ff0000>";
                        else textColor = "<color=#ffffff>";

                        temp = textColor + $"{rock + player.gameObject.GetComponent<PlayerState>().rock}</color>/<color=#ffffff>{useHouse.useLevelUpItem[useHouse.houseLevel].rock}</color>";
                        //temp = $"<color=#ffffff>{rock + player.gameObject.GetComponent<PlayerState>().rock}/{useHouse.useLevelUpItem[useHouse.houseLevel].rock}</color>";
                        //temp = $"<color=#ffffff>{rock}/{useHouse.useLevelUpItem[useHouse.houseLevel].rock}</color>";
                        slotText.text = temp;
                        break;
                    case ("Wood"):
                        if (wood + player.gameObject.GetComponent<PlayerState>().wood < useHouse.useLevelUpItem[useHouse.houseLevel].wood) textColor = "<color=#ff0000>";
                        else textColor = "<color=#ffffff>";

                        temp = textColor + $"{wood + player.gameObject.GetComponent<PlayerState>().wood}</color>/<color=#ffffff>{useHouse.useLevelUpItem[useHouse.houseLevel].wood}</color>";
                        //temp = $"<color=#ffffff>{wood + player.gameObject.GetComponent<PlayerState>().wood}/{useHouse.useLevelUpItem[useHouse.houseLevel].wood}</color>";
                        //temp = $"<color=#ffffff>{wood}/{useHouse.useLevelUpItem[useHouse.houseLevel].wood}</color>";

                        slotText.text = temp;
                        break;
                    case ("Leather"):
                        if (leather + player.gameObject.GetComponent<PlayerState>().leather < useHouse.useLevelUpItem[useHouse.houseLevel].leather) textColor = "<color=#ff0000>";
                        else textColor = "<color=#ffffff>";

                        temp = textColor + $"{leather + player.gameObject.GetComponent<PlayerState>().leather}</color>/<color=#ffffff>{useHouse.useLevelUpItem[useHouse.houseLevel].leather}</color>";
                        //temp = $"<color=#ffffff>{leather + player.gameObject.GetComponent<PlayerState>().leather}/{useHouse.useLevelUpItem[useHouse.houseLevel].leather}</color>";
                        //temp = $"<color=#ffffff>{leather}/{useHouse.useLevelUpItem[useHouse.houseLevel].leather}</color>";
                        slotText.text = temp;
                        break;

                    default:
                        break;
                }
            }
        }
    }
    void HouseSlotUpdate()
    {
        string temp;
        for (int i = 0; i < items.Count && i < HouseSlots.Length; i++)
        {
            slotText = HouseSlots[i].transform.GetComponentInChildren<TMP_Text>(); //slot의 text 가져오기

            for (int j = 0; j < items.Count; j++)
            {
                switch (HouseSlots[i].item.name) // 현재 slot에 할당된 아이템의 이름에 따라 플레이어의 재료 개수를 text에 넣음
                {
                    case ("Rock"):
                        temp = $"<color=#000000>{rock}</color>";
                        //temp = $"<color=#ffffff>{rock}/{useHouse.useLevelUpItem[useHouse.houseLevel].rock}</color>";
                        slotText.text = temp;
                        break;
                    case ("Wood"):
                        temp = $"<color=#000000>{wood}</color>";
                        //temp = $"<color=#ffffff>{wood}/{useHouse.useLevelUpItem[useHouse.houseLevel].wood}</color>";

                        slotText.text = temp;
                        break;
                    case ("Leather"):
                        temp = $"<color=#000000>{leather}</color>";
                        //temp = $"<color=#ffffff>{leather}/{useHouse.useLevelUpItem[useHouse.houseLevel].leather}</color>";
                        slotText.text = temp;
                        break;

                    default:
                        break;
                }
            }
        }
    }
    #region item slot 기능
    [SerializeField]
    private Transform QuestSlotParent;
    [SerializeField]
    private Transform HouseSlotParent;
    [SerializeField]
    private Slot[] QuestSlots;
    [SerializeField]
    private Slot[] HouseSlots;

#if UNITY_EDITOR
    private void OnValidate()
    {
        QuestSlots = QuestSlotParent.GetComponentsInChildren<Slot>();
        HouseSlots = HouseSlotParent.GetComponentsInChildren<Slot>();
    }
#endif

    public int ListCount()
    {
        return items.Count;
    }
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        QuestFreshSlot();
        HouseFreshSlot();
        QuestSlotUpdate();
        HouseSlotUpdate();
    }

    public void QuestFreshSlot()
    {
        int i = 0;
        for (; i < items.Count && i < QuestSlots.Length; i++)
        {
            QuestSlots[i].item = items[i];
        }
        for (; i < QuestSlots.Length; i++)
        {
            QuestSlots[i].item = null;
        }
    }
    public void HouseFreshSlot()
    {
        int i = 0;
        for (; i < items.Count && i < HouseSlots.Length; i++)
        {
            HouseSlots[i].item = items[i];
        }
        for (; i < HouseSlots.Length; i++)
        {
            HouseSlots[i].item = null;
        }
    }

    public void AddItem(Item _item)
    {
        if (items.Count < HouseSlots.Length)
        {
            items.Add(_item);
            QuestFreshSlot();
        }
        else
        {
            print("슬롯이 가득 차 있습니다.");
        }
        if (items.Count < HouseSlots.Length)
        {
            items.Add(_item);
            HouseFreshSlot();
        }
        else
        {
            print("슬롯이 가득 차 있습니다.");
        }
    }
    #endregion
}