using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("0 : 도끼  1 : 곡괭이  2 : 검")]  // 검은 아직 구현안햇음!
    public GameObject[] weapons;
    bool[] hasWeapon;
    Weapon equipWeapon;
    Animator animator;
    bool IsSwap;
    bool IsAttack;
    float atkDelay;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        hasWeapon = new bool[weapons.Length];

        for (int i = 0; i < weapons.Length; i++)
            hasWeapon[i] = true;

        foreach (var weapon in weapons)
            weapon.gameObject.SetActive(false);
    }

    private void Update()
    {
        SwapWeapon();
        Attack();
    }

    void SwapWeapon()
    {
        int weaponIndex = -1;

        // 무기 교체 조건 확인
        if (Input.GetButtonDown("Swap1") && hasWeapon[0] && !IsSwap)
            weaponIndex = 0;
        else if (Input.GetButtonDown("Swap2") && hasWeapon[1] && !IsSwap)
            weaponIndex = 1;
        else if (Input.GetButtonDown("Swap3") && hasWeapon[2] && !IsSwap)
            weaponIndex = 2;

        if (weaponIndex >= 0 && weaponIndex < weapons.Length)
        {
            if (equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);

            IsSwap = true;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            StartCoroutine(WeaponSwap());
        }
    }

    IEnumerator WeaponSwap()
    {
        animator.SetTrigger("OnSwap");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;
        yield return new WaitForSeconds(animationLength / 2);
        equipWeapon.gameObject.SetActive(true);
        IsSwap = false;
    }

    void Attack()
    {
        if (equipWeapon == null)
            return;

        atkDelay += Time.deltaTime;
        IsAttack = equipWeapon.GetComponent<Weapon>().atkSpeed < atkDelay;

        if (Input.GetMouseButtonDown(0) && IsAttack && !IsSwap)
        {
            WeaponType weaponType = equipWeapon.GetComponent<Weapon>().weaponType;
            switch (weaponType)
            {
                case WeaponType.Axe:         
                    animator.SetBool("IsAxe", IsAttack);
                    break;
                case WeaponType.PickAxe:
                    animator.SetBool("IsPickAxe", IsAttack);
                    break;
                case WeaponType.Sword:
                    animator.SetBool("IsSword", IsAttack);
                    break;
                default:
                    break;
            }
            animator.SetTrigger("OnAttack");
            equipWeapon.GetComponent<Weapon>().Use();
            atkDelay = 0;
            IsAttack = false;
        }
    }


    public void WeaponLock() // 무기 해금 아직 구현 안함
    {
        Weapon weapon = GetComponent<Weapon>();
        hasWeapon[weapon.weaponNum] = true;
    }
}
