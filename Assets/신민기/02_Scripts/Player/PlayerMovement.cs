using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    Animator animator;
    Camera cam;
    PlayerSpawner playerSpawn;
    WeaponController weaponController;

    Vector3 moveDirection;
    Vector3 desiredMoveDirection;

    float inputX;
    float inputZ;
    public float moveAmount;
    float dempTime;
    float ySpeed;

    public bool IsActive; // 외부스트립트에서도 적용시킬거임
    bool IsRun;
    bool IsFalling;
    public bool IsGrounded;
    bool BlockRotationPlayer;
    bool IsParticularAcitve; // 스테미나가 떨어지면 특정행동을 불가능하도록 만든 bool 값
    bool IsStaminaRecovering;
    bool IsInvincible;
    bool backHpHit;

    [Header("Player Stats")]
    [SerializeField] float moveSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float runCost;
    [SerializeField] float jumpCost;
    float currentSpeed;

    [Space(10)]
    [Header("Player Stamina")]
    [SerializeField] float maxStamina;
    [SerializeField] float staminaTime;
    [SerializeField] float staminaHeal;
    GameObject staminaParent;
    Image staminaBar;
    float staminaCoolTime;
    float currentStamina;

    [Header("Player HP")]
    [SerializeField] int maxHp;
    [SerializeField] float hpTime;
    [SerializeField] int hpHealAmount = 5; // 회복할 정수 단위
    [SerializeField] float healInterval = 0.1f; // 회복 간격 (초)
    private float healTimer;
    float hpCoolTimer;
    float currentHp;
    bool IsHpRecovering;
    Slider hpBar;
    Slider backHpBar;
    TextMeshProUGUI maxHpText;
    TextMeshProUGUI currentHpText;

    private WeaponSlotInventory weaponSlotInventory; //추가~~~~~~~~~~~~~~
    private HouseAttacked houseAttacked; //추가~~~~~~~~~~~~~~

    private void Awake()
    {
        //Debug.Log(GameObject.Find("StaminaBar").transform.name); 
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cam = Camera.main;
        staminaParent = GetComponentInChildren<Canvas>().gameObject;
        staminaBar = GameObject.Find("StaminaBar").transform.GetComponent<Image>(); // 수정
        hpBar = GameObject.Find("PlayerHealth").transform.Find("PlayerHpSlider").GetComponent<Slider>();
        backHpBar = GameObject.Find("PlayerHealth").transform.Find("BackHpSlider").GetComponent<Slider>();
        maxHpText = hpBar.transform.Find("MaxHp").GetComponent<TextMeshProUGUI>();
        currentHpText = hpBar.transform.Find("CurrentHp").GetComponent<TextMeshProUGUI>();
        playerSpawn = GetComponent<PlayerSpawner>();
        weaponController = GetComponent<WeaponController>();
        weaponSlotInventory = FindObjectOfType<WeaponSlotInventory>();
        houseAttacked = FindObjectOfType<HouseAttacked>();
    }

    private void Start()
    {
        Initalize();
    }

    private void OnEnable()
    {
        Initalize();
    }

    public void Initalize()
    {
        animator.SetTrigger("OnActive");
        weaponController.Initalize();
        IsParticularAcitve = true;
        IsInvincible = false;
        IsActive = true;
        currentStamina = maxStamina;
        currentHp = maxHp;
        maxHpText.text = "/ " + $"{maxHp}";
        currentHpText.text = $"{currentHp}";
        transform.gameObject.tag = "Player";
        hpBar.value = 1;
        backHpBar.value = hpBar.value;
        staminaParent.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (IsActive)
        {
            MoveInput();
            Move();
        }
    }

    private void Update()
    {
        if (IsActive)
        {
            JumpInput();
            Turn();
        }
        GroundCheck();
        UpdateStamina();
        UpdateHP();
        Heal();
    }

    void MoveInput()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        moveAmount = new Vector3(inputX, 0, inputZ).sqrMagnitude;
        moveDirection = new Vector3(inputX, 0, inputZ).normalized;

        if (Input.GetButton("Run") && moveAmount > 0 && IsParticularAcitve)
        {
            IsRun = true;
            currentSpeed = runSpeed;
            IsHpRecovering = false;
            hpCoolTimer = 0;
            CostStamina(runCost, true);
            PlayerMoveAndRotation();
        }

        else if (moveAmount > 0.1f)
        {
            IsHpRecovering = false;
            hpCoolTimer = 0;
            IsRun = false;
            dempTime = 0.3f;
            currentSpeed = moveSpeed;
            PlayerMoveAndRotation();
        }
        else
        {
            IsRun = false;
            dempTime = 0.15f;
            currentSpeed = moveSpeed;
            desiredMoveDirection = Vector3.zero;
        }
    }

    void Move()
    {
        moveDirection.y = ySpeed;
        Vector3 move = moveDirection * currentSpeed * Time.deltaTime;
        move.y = moveDirection.y * Time.deltaTime;
        controller.Move(move);
    }

    void PlayerMoveAndRotation()
    {
        var forward = cam.transform.forward;
        var right = cam.transform.right;
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        moveDirection = forward * inputZ + right * inputX;

        if (!BlockRotationPlayer)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 0.1f);
    }

    void JumpInput()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded && IsParticularAcitve)
        {
            ySpeed = jumpForce;
            CostStamina(jumpCost, false);
            animator.SetTrigger("OnJump");
            IsHpRecovering = false;
            hpCoolTimer = 0;
        }
    }


    void GroundCheck()
    {
        if (controller.isGrounded)
        {
            controller.stepOffset = 0f;
            moveDirection.y = -0.5f;
            IsFalling = false;
            IsGrounded = true;
        }
        else
        {
            controller.stepOffset = 0;
            ySpeed -= 9.81f * Time.deltaTime;
            IsFalling = true;
            IsGrounded = false;
        }
    }

    void Turn()
    {
        if (Input.GetMouseButton(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
            {
                Vector3 directionToLook = rayHit.point - transform.position;
                directionToLook.y = 0;

                if (directionToLook != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToLook);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 50f);
                }
            }
        }

        //Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        //{
        //    Vector3 directionLookPos = hit.point - transform.position;
        //    directionLookPos.y = 0;

        //    if (directionLookPos != Vector3.zero)
        //    {
        //        Quaternion targetRotation = Quaternion.LookRotation(directionLookPos);
        //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 100f);
        //    }
        //}
    }
    public void OnDamage(int _atkDamage) // 순서 바꾸지마세요...
    {
        if (IsInvincible)
            return;

        IsHpRecovering = false;
        hpCoolTimer = 0;
        currentHp -= _atkDamage;
        UpdateHpUI();

        if (currentHp <= 0)
        {
            OnDie();
            return;
        }


        IsInvincible = true;
        animator.SetTrigger("OnDamage");

        if (!backHpHit)
        {
            StartCoroutine(BackHpCoroutine());
        }
    }

    public void OnDie()
    {
        IsActive = false;
        animator.SetTrigger("OnDie");
        IsInvincible = true;
        playerSpawn.PlayerSpawn();
        this.gameObject.GetComponent<PlayerState>().rock = 0;
        this.gameObject.GetComponent<PlayerState>().wood = 0;
        this.gameObject.GetComponent<PlayerState>().leather = 0;
        weaponSlotInventory.ResetSlotOutline();
        if (TimeManager.Instance.nightCheck)
        {
            StartCoroutine(WaitDieFadeIn());
            StopCoroutine(WaitDieFadeIn());
        }
    }

    IEnumerator WaitDieFadeIn() // 죽은 후 페이드인아웃 대기시간 후 디펜스 실패 결과 연결
    {
        houseAttacked.LooseHouseItem(); // 먼저 자원 다잃고 거점 데미지 비활성화, 체력/레벨 초기화
        yield return new WaitForSeconds(5.0f); // 페이드인/아웃 시간은 임의로 조정
        houseAttacked.LooseHouseText(); // 페이드인/아웃 끝나면 문구 띄우고 낮으로 전환
    }

    public void UnActive()
    {
        IsActive = false;
    }

    public void Active()
    {
        IsActive = true;
    }

    #region Hp
    private void UpdateHP()
    {
        backHpBar.value = Mathf.Lerp(backHpBar.value, hpBar.value, Time.deltaTime * 10f);

        if (Mathf.Abs(hpBar.value - backHpBar.value) < 0.01f)
        {
            backHpHit = false;
            backHpBar.value = hpBar.value;
        }
    }
    private void UpdateHpUI()
    {
        currentHp = Mathf.Max(0, currentHp);

        // HP 값을 업데이트할 때, 클램프된 값을 사용
        hpBar.value = Mathf.Clamp01(Mathf.RoundToInt(currentHp) / (float)maxHp);
        currentHpText.text = $"{Mathf.RoundToInt(currentHp)}";
    }

    private IEnumerator BackHpCoroutine()
    {
        yield return new WaitForSeconds(1f);
        backHpHit = true;
        yield return new WaitForSeconds(0.5f);
        IsInvincible = false;
    }

    void Heal()
    {
        if (currentHp < maxHp)
        {
            if (!IsHpRecovering)
            {
                hpCoolTimer += Time.deltaTime;
                if (hpCoolTimer >= hpTime && moveAmount == 0)
                {
                    IsHpRecovering = true;
                    hpCoolTimer = 0f;
                }
            }
            else
            {
                healTimer += Time.deltaTime;
                if (healTimer >= healInterval)
                {
                    currentHp += hpHealAmount * Time.deltaTime;
                    currentHp = Mathf.Clamp(currentHp, 0, maxHp);
                    UpdateHpUI();
                }
            }
        }
        else
        {
            IsHpRecovering = false;
            healTimer = 0f;
        }
    }

    #endregion


    #region 스테미나
    void CostStamina(float cost, bool UseDeltaTime = false)
    {
        staminaParent.SetActive(true);

        if (UseDeltaTime)
            currentStamina -= cost * Time.deltaTime;

        else
            currentStamina -= cost;

        if (currentStamina <= maxStamina)
        {
            IsStaminaRecovering = false;
            staminaCoolTime = 0;
        }
    }

    void UpdateStamina()
    {
        if (currentStamina <= maxStamina)
        {
            if (!IsStaminaRecovering)
            {
                staminaCoolTime += Time.deltaTime;
                if (staminaCoolTime >= staminaTime)
                    IsStaminaRecovering = true;
            }
            else
            {
                currentStamina += staminaHeal * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
                if (currentStamina >= maxStamina)
                {
                    staminaParent.SetActive(false);
                    IsStaminaRecovering = false;
                    IsParticularAcitve = true;
                }
            }
        }

        if (currentStamina <= 0)
            IsParticularAcitve = false;
        else if (currentStamina >= jumpCost) // 점프비용이 제일 큰 값이라서 점프비용만큼 채웠더라면 다시 특정행동을 활성화 할 수 있다.
            IsParticularAcitve = true;

        staminaBar.fillAmount = currentStamina / maxStamina;
    }

    #endregion
    private void OnAnimatorMove()
    {
        animator.SetBool("IsRunning", IsRun);
        animator.SetBool("IsGrounded", controller.isGrounded);
        animator.SetBool("IsFalling", IsFalling);
        animator.SetFloat("MoveAmount", moveAmount, dempTime, Time.deltaTime);
    }
}