using System.Collections;
using UnityEngine;

public enum WeaponType
{
    Axe,
    PickAxe,
    Sword
}

[RequireComponent(typeof(BoxCollider))]
public class Weapon : MonoBehaviour
{
    public WeaponType weaponType;
    public int weaponNum;
    public float atkSpeed;
    [SerializeField] int woodDamage;
    [SerializeField] int rockDamage;
    [SerializeField] int monsterDamage;
    [SerializeField] float enableTime;
    BoxCollider meleeArea;

    private void Awake()
    {
        meleeArea = GetComponent<BoxCollider>();
        meleeArea.enabled = false;
    }

    public void Use()
    {
        StopCoroutine(Swing());
        StartCoroutine(Swing());
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(enableTime);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;
    }
}
