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

    private void Awake()
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;

        offset = transform.position;
        cam = Camera.main;
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
        float horizontalInput = 0f;

        if (Input.GetKey(KeyCode.Q))
            horizontalInput = -rotateSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.E))
            horizontalInput = rotateSpeed * Time.deltaTime;

        transform.RotateAround(target.position, Vector3.up, horizontalInput);

        offset = transform.position - target.position;
    }

    void Zoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float dist = Vector3.Distance(transform.position, cam.transform.position);

        if (dist < minZoomDist && scrollInput > 0.0f)
            return;

        else if (dist > maxZoomDist && scrollInput < 0.0f)
            return;


        cam.transform.position += cam.transform.forward * scrollInput * zoomSpeed;
    }
}
