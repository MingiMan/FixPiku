using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    Animator animator;
    Camera cam;

    Vector3 moveDirection;
    Vector3 desiredMoveDirection;

    float inputX;
    float inputZ;
    float moveAmount;
    float dempTime;
    float ySpeed;

    bool IsRun;
    bool IsFalling;
    bool IsGrounded;
    bool BlockRotationPlayer;
    bool IsParticularAcitve; // 스테미나가 떨어지면 특정행동을 불가능하도록 만든 bool 값
    bool IsRecovering;
    bool IsActive;
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
    int currentHp;
    // Slider hpBar;
    // Slider backHpBar;
    // TextMeshProUGUI maxHpText;
    // TextMeshProUGUI currentHpText;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cam = Camera.main;
        staminaParent = GetComponentInChildren<Canvas>().gameObject;
        staminaBar = GameObject.Find("Canvas").transform.Find("StaminaBar").GetComponent<Image>();
        hpBar = GameObject.Find("PlayerHealth").transform.Find("PlayerHpSlider").GetComponent<Slider>();
        backHpBar = GameObject.Find("PlayerHealth").transform.Find("BackHpSlider").GetComponent<Slider>();
        maxHpText = hpBar.transform.Find("MaxHp").GetComponent<TextMeshProUGUI>();
        currentHpText = hpBar.transform.Find("CurrentHp").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Initalize();
    }

    void Initalize()
    {
        IsParticularAcitve = true;
        IsActive = true;
        currentStamina = maxStamina;
        // currentHp = maxHp;
        // maxHpText.text = "/ " + $"{maxHp}";
        // currentHpText.text = $"{currentHp}";
        // staminaParent.SetActive(false);
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
            CostStamina(runCost, true);
            PlayerMoveAndRotation();
        }

        else if (moveAmount > 0.1f)
        {
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
        }
    }

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
            IsRecovering = false;
            staminaCoolTime = 0;
        }
    }

    void UpdateStamina()
    {
        if (currentStamina <= maxStamina)
        {
            if (!IsRecovering)
            {
                staminaCoolTime += Time.deltaTime;
                if (staminaCoolTime >= staminaTime)
                    IsRecovering = true;
            }
            else
            {
                currentStamina += staminaHeal * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
                if (currentStamina >= maxStamina)
                {
                    staminaParent.SetActive(false);
                    IsRecovering = false;
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
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
                }
            }
        }
    }


    public void OnDamage(int _atkDamage)
    {
        if (IsInvincible || currentHp <= 0) return; 

        IsInvincible = true;
        animator.SetTrigger("OnDamage");

        currentHp -= _atkDamage;
        UpdateHpUI();

        if (!backHpHit)
        {
            StartCoroutine(BackHpCoroutine());
        }
    }

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
        hpBar.value = Mathf.Clamp01(currentHp / (float)maxHp);
        currentHpText.text = $"{currentHp}";
    }

    private IEnumerator BackHpCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        backHpHit = true;
        yield return new WaitForSeconds(0.5f);
        IsInvincible = false;
    }


    #region 애니메이션 이벤트들
    public void UnActive()
    {
        IsActive = false;
    }

    public void Active()
    {
        IsActive = true;
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