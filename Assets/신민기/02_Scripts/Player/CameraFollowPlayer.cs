using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    [SerializeField] float zoomSpeed;
    [SerializeField] float minZoomDist;
    [SerializeField] float maxZoomDist;
    public Transform target;
    public Vector3 offset;

    Camera cam;

    [SerializeField] float minVerticalAngle = -30f; 
    [SerializeField] float maxVerticalAngle = 60f;
    float currentVerticalAngle = 0f;
    Vector3 velocity = Vector3.zero;
    Vector3 desiredCameraPosition;

    private void Awake()
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;

        offset = transform.position;
        cam = Camera.main;
        desiredCameraPosition = cam.transform.position;
    }

    private void Update()
    {
        Vector3 newPos = target.position + offset;
        transform.position = newPos;
        Rotate();
        Zoom();
    }

    void Rotate()
    {
        //float horizontalInput = 0f;

        //if (Input.GetKey(KeyCode.Q))
        //    horizontalInput = -rotateSpeed * Time.deltaTime;

        //if (Input.GetKey(KeyCode.E))
        //    horizontalInput = rotateSpeed * Time.deltaTime;

        //transform.RotateAround(target.position, Vector3.up, horizontalInput);

        //offset = transform.position - target.position;

        if (Input.GetMouseButton(2)) // 마우스 휠 버튼을 눌렀을 때
        {
            float horizontalInput = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            float verticalInput = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;

            transform.RotateAround(target.position, Vector3.up, horizontalInput);

            float desiredVerticalAngle = currentVerticalAngle - verticalInput;
            desiredVerticalAngle = Mathf.Clamp(desiredVerticalAngle, minVerticalAngle, maxVerticalAngle);
            float angleDifference = desiredVerticalAngle - currentVerticalAngle;

            Vector3 right = transform.right;
            transform.RotateAround(target.position, right, angleDifference);

            currentVerticalAngle = desiredVerticalAngle;

            offset = transform.position - target.position;
        }

    }

    void Zoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float dist = Vector3.Distance(transform.position, cam.transform.position);

        if (dist < minZoomDist && scrollInput > 0.0f)
            return;

        else if (dist > maxZoomDist && scrollInput < 0.0f)
            return;

        // 목표 카메라 위치 계산
        desiredCameraPosition = cam.transform.position + cam.transform.forward * scrollInput * zoomSpeed;

        // SmoothDamp를 사용해 부드럽게 카메라 위치 이동
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, desiredCameraPosition, ref velocity, 0.3f);
    }
}
