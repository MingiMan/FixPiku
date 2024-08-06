using System.Collections;
using UnityEngine;

public enum WeaponType
{
    Axe,
    PickAxe,
    Sword,
    Cannon,
}
public enum AtkType
{
    Melee, Range
}

[RequireComponent(typeof(BoxCollider))]
public class Weapon : MonoBehaviour
{
    public WeaponType weaponType;
    public AtkType atkType;
    public int weaponNum;
    public float atkSpeed;
<<<<<<< HEAD
    [SerializeField] int woodDamage;
    [SerializeField] int rockDamage;
    [SerializeField] int monsterDamage;
    [SerializeField] float enableTime;
=======

    public int woodDamage;
    public int rockDamage;
    public int monsterDamage;
    [SerializeField] float enableTime;

>>>>>>> dd014889354572f0abb1d3a42768d4434be9d3a4
    BoxCollider meleeArea;

    [Space(10)]
    [Header("Range")]
    public Transform bulletPos;
    public GameObject cannonBulletPrefab;
    public int maxAmmo;
    public int curAmmo;

    private void Awake()
    {
        meleeArea = GetComponent<BoxCollider>();
        meleeArea.enabled = false;
    }

    public void Use()
    {
<<<<<<< HEAD
        if(atkType == AtkType.Melee)
=======
        if (atkType == AtkType.Melee)
>>>>>>> dd014889354572f0abb1d3a42768d4434be9d3a4
        {
            StopCoroutine(Swing());
            StartCoroutine(Swing());
        }
        else if (atkType == AtkType.Range)
        {
            curAmmo--;
            StartCoroutine(RangeAttack());
        }
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(enableTime);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;
    }

    public void EnableCollider()
    {
        meleeArea.enabled = true;
    }

    public void DisableCollider()
    {
        meleeArea.enabled = false;
    }

    IEnumerator RangeAttack()
    {
        yield return new WaitForSeconds(1f);
<<<<<<< HEAD
        GameObject bullet = Instantiate(cannonBulletPrefab, bulletPos.position,Quaternion.identity);
=======
        GameObject bullet = Instantiate(cannonBulletPrefab, bulletPos.position, Quaternion.identity);
>>>>>>> dd014889354572f0abb1d3a42768d4434be9d3a4
        bullet.GetComponent<CannonBullet>().FireBullet(bulletPos.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ENEMY"))
        {
            other.GetComponent<Enemys>().Damage(monsterDamage, transform.position);
        }
    }
}
