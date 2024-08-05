using UnityEngine;

public class HouseLevelUp : MonoBehaviour
{
    [SerializeField] UseHouse houseLevelCheck;
    [SerializeField] int houseLevelLimit = 4;

    // Update is called once per frame
    void Update()
    {

    }
    public void HouseLevelUpClick()
    {
        if (houseLevelCheck.houseLevel < 3)
        {
            houseLevelCheck.houseLevel += 1;
        }
    }
}
