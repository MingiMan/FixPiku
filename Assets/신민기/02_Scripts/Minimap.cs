using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour
{
    GameObject maker;

    private void Start()
    {
        StartCoroutine(FindPlayer());
    }

    private void LateUpdate()
    {
        if (maker != null)
        {
            Vector3 pos = maker.transform.position;
            pos.y = transform.position.y;
            transform.position = pos;
        }

    }

    IEnumerator FindPlayer()
    {
        yield return new WaitForSeconds(1F);
        maker = GameObject.FindGameObjectWithTag("MAKER");
    }
}
