using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class ItemFunction : MonoBehaviour
{
    void Awake()
    {
        Debug.Log(this.gameObject.tag);
        if (this.gameObject.tag == "Rock") this.gameObject.GetComponent<SphereCollider>().enabled = false;
        else if (this.gameObject.tag == "Wood") this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        else if (this.gameObject.tag == "Leather") this.gameObject.GetComponent<BoxCollider>().enabled = false;

        //this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        StartCoroutine(ActiveItem());
    }
    IEnumerator ActiveItem()
    {
        Debug.Log(this.gameObject.tag + "실행");
        yield return new WaitForSeconds(1.0f);
        if (this.gameObject.tag == "Rock") this.gameObject.GetComponent<SphereCollider>().enabled = true;
        else if (this.gameObject.tag == "Wood") this.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        else if (this.gameObject.tag == "Leather") this.gameObject.GetComponent<BoxCollider>().enabled = true;
        //this.gameObject.GetComponent<CapsuleCollider>().enabled = true;
    }



}
