using UnityEngine;

public class VerticalOscillation : MonoBehaviour
{
    public float amplitude = 1.0f; // 진폭
    public float frequency = 1.0f; // 주파수

    private Vector3 startPosition;

    void Start()
    {
        // 시작 위치 저장
        startPosition = transform.position;
    }

    void Update()
    {
        // 시간에 따라 Y좌표를 사인 함수로 변형
        float yOffset = amplitude * Mathf.Sin(Time.time * frequency);
        transform.position = startPosition + new Vector3(0, yOffset, 0);
    }
}