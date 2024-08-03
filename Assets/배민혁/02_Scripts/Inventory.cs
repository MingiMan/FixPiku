using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<Item> items;
    public TextMeshProUGUI slotText;

    public GameObject player;


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
                        slotText.text = $"<color=#ffffff>{player.GetComponent<PlayerController>().rock}</color>";
                        break;
                    case ("Wood"):
                        slotText.text = $"<color=#ffffff>{player.GetComponent<PlayerController>().wood}</color>";
                        break;
                    case ("Leather"):
                        slotText.text = $"<color=#ffffff>{player.GetComponent<PlayerController>().leather}</color>";
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