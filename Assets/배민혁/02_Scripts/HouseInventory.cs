using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.Rendering;

public class HouseInventory : MonoBehaviour
{
    public List<Item> items;
<<<<<<< HEAD
    public TextMeshProUGUI slotText;
=======
    public TMP_Text slotText;

>>>>>>> dd014889354572f0abb1d3a42768d4434be9d3a4

    public GameObject player;
    public GameObject Inventory;

    public int rock = 3;
    public int wood = 3;
    public int leather = 3;
<<<<<<< HEAD
    void Update()
    {
        string temp;
        Debug.Log($"{rock}, {wood}, {leather}");
        for (int i = 0; i < items.Count && i < slots.Length; i++)
        {
            slotText = slots[i].transform.GetComponentInChildren<TextMeshProUGUI>(); //slot의 text 가져오기
=======

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

>>>>>>> dd014889354572f0abb1d3a42768d4434be9d3a4
            for (int j = 0; j < items.Count; j++)
            {
                switch (slots[i].item.name) // 현재 slot에 할당된 아이템의 이름에 따라 플레이어의 재료 개수를 text에 넣음
                {
                    case ("Rock"):
                        //temp = $"<color=#ffffff>{rock + player.GetComponent<PlayerController>().rock}</color>";
<<<<<<< HEAD
                        temp = $"<color=#ffffff>{rock}";
=======
                        temp = $"<color=#ffffff>{rock}/{000}</color>";
>>>>>>> dd014889354572f0abb1d3a42768d4434be9d3a4
                        slotText.text = temp;
                        break;
                    case ("Wood"):
                        //temp = $"<color=#ffffff>{wood + player.GetComponent<PlayerController>().wood}</color>";
<<<<<<< HEAD
                        temp = $"<color=#ffffff>{wood}";
=======
                        temp = $"<color=#ffffff>{wood}/{000}</color>";
>>>>>>> dd014889354572f0abb1d3a42768d4434be9d3a4
                        slotText.text = temp;
                        break;
                    case ("Leather"):
                        //temp = $"<color=#ffffff>{leather + player.GetComponent<PlayerController>().leather}</color>";
<<<<<<< HEAD
                        temp = $"<color=#ffffff>{leather}";
=======
                        temp = $"<color=#ffffff>{leather}/{000}</color>";
>>>>>>> dd014889354572f0abb1d3a42768d4434be9d3a4
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
        player = GameObject.FindGameObjectWithTag("PLAYER");
        FreshSlot();
<<<<<<< HEAD
=======
        slotUpdate();
>>>>>>> dd014889354572f0abb1d3a42768d4434be9d3a4
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