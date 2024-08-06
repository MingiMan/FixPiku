using UnityEngine;

public class StaminaUI : MonoBehaviour
{
    Transform playerTR;

    private void Awake()
    {
        playerTR = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        Quaternion q_st = Quaternion.LookRotation(transform.transform.position - Camera.main.transform.position);
        Vector3 st_angle = Quaternion.RotateTowards(transform.rotation, q_st, 200).eulerAngles;
        transform.rotation = Quaternion.Euler(0, st_angle.y, 0);
    }
}
