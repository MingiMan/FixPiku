using UnityEngine;

public class PlayerState : MonoBehaviour
{

    public int rock = 0;  //보유 돌
    public int wood = 0;  //보유 나무
    public int leather = 0;  //보유 가죽

    public float attackState = 50.0f;  // 공격력
    void OnTriggerEnter(Collider coll)
    {

        //Debug.Log(coll.name);
        switch (coll.tag)
        {
            case ("ROCK"):
                Debug.Log("ROCK");
                // coll.gameObject.GetComponent<PlayerController>().rock += 1;
                rock += 1;
                Destroy(coll.gameObject);
                break;
            case ("WOOD"):
                Debug.Log("WOOD");
                // coll.gameObject.GetComponent<PlayerController>().wood += 1;
                wood += 1;

                Destroy(coll.gameObject);
                break;
            case ("LEATHER"):
                Debug.Log("LEATHER");
                // coll.gameObject.GetComponent<PlayerController>().leather += 1;
                leather += 1;
                Destroy(coll.gameObject);
                break;

            default:
                break;

        }



    }
}
