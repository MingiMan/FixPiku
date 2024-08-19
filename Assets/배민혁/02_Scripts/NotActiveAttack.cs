using UnityEngine;
using UnityEngine.EventSystems;

public class NotActiveAttack : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private WeaponController weaponController;

    void Awake()
    {
        weaponController = FindObjectOfType<WeaponController>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
        weaponController.attackActive = false;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit");
        weaponController.attackActive = true;
    }
}
