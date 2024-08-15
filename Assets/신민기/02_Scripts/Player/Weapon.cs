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
    PlayerMovement player;
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
    public Transform cannonBulletPos;
    public GameObject cannonBulletPrefab;
    public int maxAmmo;
    public int curAmmo;
    public string cannonSound;

    [Space(10)]
    [Header("Rifle")]
    public Transform rifleBulletPos;
    public GameObject rifleBulletPrefab;
    public string gunSound;
    public int rifleMaxAmmo;
    public int riflecurAmmo;

    private void Awake()
    {
        if(weaponType == WeaponType.Sword)
            rifleBulletPos = GameObject.Find("RiflePos").transform;
        
        if(weaponType == WeaponType.Cannon)
            cannonBulletPos = GameObject.Find("CannonBulletPos").transform;

        meleeArea = GetComponent<BoxCollider>();
        player = GetComponentInParent<PlayerMovement>();
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
            if(weaponType == WeaponType.Cannon)
            {
                curAmmo--;
                StartCoroutine(CannonAttack());
            }
            else
            {
                riflecurAmmo--;
                StartCoroutine(RifleAttack());
            }
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

    IEnumerator CannonAttack()
    {
        player.IsActive = false;
        yield return new WaitForSeconds(1f);
        SoundManager.instance.PlaySFX(cannonSound);
        GameObject bullet = Instantiate(cannonBulletPrefab, cannonBulletPos.position, Quaternion.identity);
        bullet.GetComponent<CannonBullet>().FireBullet(cannonBulletPos.transform);
        yield return new WaitForSeconds(0.5f);
        player.IsActive = true;
    }

    IEnumerator RifleAttack()
    {
        player.IsActive = false;
        yield return new WaitForSeconds(0.3f);
        SoundManager.instance.PlaySFX(gunSound);
        GameObject bullet = Instantiate(rifleBulletPrefab, rifleBulletPos.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().FireBullet(rifleBulletPos.transform);
        yield return new WaitForSeconds(0.5f);
        player.IsActive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ENEMY"))
        {
            other.GetComponent<Enemys>().Damage(monsterDamage, transform.position);
        }
    }
}
