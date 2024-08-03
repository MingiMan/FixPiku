using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float v; // 상하 화살표 키값 저장할 변수
    private float h; // 좌우 화살표 키값 저장할 변수
    private float r; // 회전값 저장할 변수

    private string moveAxisName = "Vertical";
    private string rotateAxisName = "Horizontal";
    private string cameraRoteate = "Mouse X";

    [SerializeField]
    private float moveSpeed = 6.0f;  // 이동속도
    [SerializeField]
    private float turnSpeed = 500.0f;// 회전속도


    public int rock = 0;  //보유 돌
    public int wood = 0;  //보유 나무
    public int leather = 0;  //보유 가죽

    public float attackState = 10.0f;  // 공격력


    [SerializeField]
    private Animator animator;




    void Start()
    {
        Debug.Log("Hello World!!!");
    }
    /* 
        화면을 랜더링하는 주기만큼 호출 60fps > 60번 호출
        호출 간격이 불규칙
    */
    void Update()
    {
        v = Input.GetAxis(moveAxisName);
        h = Input.GetAxis(rotateAxisName);
        r = Input.GetAxis(cameraRoteate);

        // Vector3 moveDir = Vector3.forward * v + Vector3.right * h;
        // transform.Translate(moveDir.normalized * Time.deltaTime * moveSpeed);
        // transform.Rotate(Vector3.up * Time.deltaTime * r * turnSpeed);
        Vector3 moveDir = new Vector3(h, 0, v).normalized;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        // transform.LookAt(transform.position + moveDir);
        transform.Rotate(Vector3.up * Time.deltaTime * r * turnSpeed);
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

    }

    void Attack()
    {

        {
            Debug.Log("Attack");

        }


    }

}
