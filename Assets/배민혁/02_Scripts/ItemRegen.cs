using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
public class ItemRegen : MonoBehaviour
{
    public GameObject parentObject;
    public GameObject regenPrefab;
    //private float regenTime = 5.0f;  // 나무 재생성시간
    Transform[] childList;
    public bool checkObject = false;// object 있으면 true 없으면 false
    public bool oneCycle = false;// 낮밤 한사이클이 돌면 true, 리젠 대기용
    void Start()
    {
        GameObject regenObject = Instantiate(regenPrefab) as GameObject;
        regenObject.transform.SetParent(parentObject.gameObject.transform, false);
        //checkObject = true;
        oneCycle = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!oneCycle && TimeManager.Instance.nightCheck) oneCycle = true;
        if (oneCycle && !TimeManager.Instance.nightCheck)
        {
            childList = gameObject.GetComponentsInChildren<Transform>();
            //Debug.Log("regen");
            if (childList != null)
            {
                for (int i = 0; i < parentObject.transform.childCount; i++)
                {
                    Destroy(parentObject.transform.GetChild(i).gameObject);
                }

            }
            //Instantiate(regenPrefab, itemPosition, itemRotation);
            GameObject regenObject = Instantiate(regenPrefab) as GameObject;
            regenObject.transform.SetParent(parentObject.gameObject.transform, false);
            //checkObject = true;
            oneCycle = false;
        }
        // if (!checkObject)
        // {
        //     childList = gameObject.GetComponentsInChildren<Transform>();
        //     Debug.Log("regen");
        //     if (childList != null)
        //     {
        //         for (int i = 0; i < parentObject.transform.childCount; i++)
        //         {
        //             Destroy(parentObject.transform.GetChild(i).gameObject);
        //         }

        //     }
        //     //Instantiate(regenPrefab, itemPosition, itemRotation);
        //     GameObject regenObject = Instantiate(regenPrefab) as GameObject;
        //     regenObject.transform.SetParent(parentObject.gameObject.transform, false);
        //     checkObject = true;
        // }

    }
}
