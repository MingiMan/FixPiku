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
    public int woodDamage;
    public int rockDamage;
    public int monsterDamage;
    [SerializeField] float enableTime;
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
        if (atkType == AtkType.Melee)
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
        GameObject bullet = Instantiate(cannonBulletPrefab, bulletPos.position, Quaternion.identity);
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
