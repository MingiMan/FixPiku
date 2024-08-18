using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("0 : 도끼  1 : 곡괭이  2 : 검 3 : 캐논")]
    Camera cam;
    public GameObject[] weapons;
    PlayerMovement player;
    public bool[] hasWeapon;
    public int activeWeaponIndex = -1;/////////////////////////////////////////
    Weapon equipWeapon;
    Animator animator;
    bool IsSwap;
    bool IsAttack;
    float atkDelay;

    private void Awake()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        hasWeapon = new bool[weapons.Length];

        for (int i = 0; i < weapons.Length; i++)
            hasWeapon[i] = true;

        foreach (var weapon in weapons)
            weapon.gameObject.SetActive(false);
        WeaponLock();// 잠금추가/////////////////////////////////////////
        hasWeapon[0] = true; // 도끼만 잠금해제/////////////////////////////////////////
    }

    public void Initalize()
    {
        foreach (var weapon in weapons)
            weapon.gameObject.SetActive(false);

        if (equipWeapon != null)
            equipWeapon.gameObject.SetActive(false);

    }

    private void Update()
    {
        SwapWeapon();
        Attack();
    }

    void SwapWeapon()
    {
        int weaponIndex = -1;
        if (Input.GetButtonDown("Swap1") && hasWeapon[0] && !IsSwap && !weapons[0].activeSelf)  // 잠긴건 실행 안되도록 수정
        {
            weaponIndex = 0;
            activeWeaponIndex = 0;/////////////////////////////////////////

        }
        else if (Input.GetButtonDown("Swap2") && hasWeapon[1] && !IsSwap && !weapons[1].activeSelf)
        {
            weaponIndex = 1;
            activeWeaponIndex = 1;/////////////////////////////////////////
        }
        else if (Input.GetButtonDown("Swap3") && hasWeapon[2] && !IsSwap && !weapons[2].activeSelf)
        {
            weaponIndex = 2;
            activeWeaponIndex = 2;/////////////////////////////////////////
        }
        else if (Input.GetButtonDown("Swap4") && hasWeapon[3] && !IsSwap && !weapons[3].activeSelf)
        {
            weaponIndex = 3;
            activeWeaponIndex = 3;/////////////////////////////////////////
        }

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
                RotateTowardsMouse();
                WeaponType weaponType = equipWeapon.GetComponent<Weapon>().weaponType;


                animator.SetBool("IsAxe", false);
                animator.SetBool("IsPickAxe", false);
                animator.SetBool("IsSword", false);
                animator.SetBool("IsCannon", false);
                string weaponBool = weaponType switch
                {
                    WeaponType.Axe => "IsAxe",
                    WeaponType.PickAxe => "IsPickAxe",
                    WeaponType.Sword => "IsSword",
                    WeaponType.Cannon => "IsCannon",
                    _ => null
                };

                if (player.IsActive && player.IsGrounded)
                {
                    if (weaponBool != null)
                        animator.SetBool(weaponBool, true);

                    animator.SetTrigger("OnAttack");
                    equipWeapon.GetComponent<Weapon>().Use();
                    atkDelay = 0;
                }
            }
        }

        void RotateTowardsMouse()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Vector3 directionLookPos = hit.point - player.transform.position;
                directionLookPos.y = 0;

                if (directionLookPos != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionLookPos);
                    player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * 100f);
                }
            }
        }

    public void WeaponLock() // 무기 해금 구현 부분/////////////////////////////////////////
    {
        // Weapon weapon = GetComponent<Weapon>();
        // hasWeapon[weapon.weaponNum] = false;
        for (int i = 0; i < weapons.Length; i++)
            hasWeapon[i] = false;
    }
}