using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class ItemFunction : MonoBehaviour
{
    public GameObject player;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }



    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            switch (this.gameObject.tag)
            {
                case ("ROCK"):
                    Debug.Log("ROCK");
                    // coll.gameObject.GetComponent<PlayerController>().rock += 1;
                    player.GetComponent<PlayerState>().rock += 1;
                    Destroy(this.gameObject);
                    break;
                case ("WOOD"):
                    Debug.Log("WOOD");
                    // coll.gameObject.GetComponent<PlayerController>().wood += 1;
                    player.GetComponent<PlayerState>().wood += 1;

                    Destroy(this.gameObject);
                    break;
                case ("LEATHER"):
                    Debug.Log("LEATHER");
                    // coll.gameObject.GetComponent<PlayerController>().leather += 1;
                    player.GetComponent<PlayerState>().leather += 1;
                    Destroy(this.gameObject);
                    break;


                default:
                    break;

            }
        }


    }
    // void OnTriggerStay(Collider coll)
    // {
    //     if (coll.CompareTag("PLAYER"))
    //     {
    //         Debug.Log("rock2");

    //         coll.gameObject.GetComponent<PlayerController>().rock += 1;
    //         Destroy(this.gameObject);
    //     }

    // }
}
