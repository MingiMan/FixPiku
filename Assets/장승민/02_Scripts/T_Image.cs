using UnityEngine;

public class VerticalOscillation : MonoBehaviour
{
    public float amplitude = 1.0f; // ����
    public float frequency = 1.0f; // ���ļ�

    private Vector3 startPosition;

    void Start()
    {
        // ���� ��ġ ����
        startPosition = transform.position;
    }

    void Update()
    {
        // �ð��� ���� Y��ǥ�� ���� �Լ��� ����
        float yOffset = amplitude * Mathf.Sin(Time.time * frequency);
        transform.position = startPosition + new Vector3(0, yOffset, 0);
    }
}