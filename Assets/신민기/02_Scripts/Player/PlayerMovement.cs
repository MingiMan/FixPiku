using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public bool IsActive;
    bool IsRun;
    bool IsFalling;
    bool IsGrounded;
    bool BlockRotationPlayer;
    bool IsSpecificAcitve; // ���׹̳��� �������� Ư���ൿ�� �Ұ����ϵ��� ���� bool ��
    bool IsRecovering;

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
    Slider hpBar;
    Slider backHpBar;
    TextMeshProUGUI maxHpText;
    TextMeshProUGUI currentHpText;


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
        IsActive = true;
        IsSpecificAcitve = true;
        currentStamina = maxStamina;
        currentHp = maxHp;
        maxHpText.text = "/ " + $"{maxHp}";
        currentHpText.text = $"{currentHp}";
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
        UpdateHpUI();
    }

    void MoveInput()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        moveAmount = new Vector3(inputX, 0, inputZ).sqrMagnitude;
        moveDirection = new Vector3(inputX, 0, inputZ).normalized;

        if (Input.GetButton("Run") && moveAmount > 0 && IsSpecificAcitve)
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
        if (Input.GetButtonDown("Jump") && IsGrounded && IsSpecificAcitve)
        {
            ySpeed = jumpForce;
            CostStamina(jumpCost, false);
            animator.SetTrigger("OnJump");
        }
    }

    #region ���׹̳�
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
                    IsSpecificAcitve = true;
                }
            }
        }

        if (currentStamina <= 0)
            IsSpecificAcitve = false;
        else if (currentStamina >= jumpCost) // ��������� ���� ū ���̶� ������븸ŭ ä������� �ٽ� Ư���ൿ�� Ȱ��ȭ �� �� �ִ�.
            IsSpecificAcitve = true;

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

    void UpdateHpUI()
    {
        hpBar.value = Mathf.Clamp01(currentHp / (float)maxHp);
        currentHpText.text = $"{currentHp}";
    }

    // �ִϸ��̼ǿ� Ŭ������ ���
    public void UnActive()
    {
        IsActive = false;
    }

    public void Active()
    {
        IsActive = true;
    }

    private void OnAnimatorMove()
    {
        animator.SetBool("IsRunning", IsRun);
        animator.SetBool("IsGrounded", controller.isGrounded);
        animator.SetBool("IsFalling", IsFalling);
        animator.SetFloat("MoveAmount", moveAmount, dempTime, Time.deltaTime);
    }
}
