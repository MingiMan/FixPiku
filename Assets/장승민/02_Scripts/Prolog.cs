using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageSwitcher : MonoBehaviour
{
    public CanvasGroup[] canvasGroups; // ��ȯ�� CanvasGroup �迭
    public Button switchButton; // ��ư ������Ʈ
    public float transitionDuration = 0.5f; // ��ȯ �ð� (��)

    [SerializeField] Image fadeOutIn;
    private int currentIndex = 0; // ���� �̹��� �ε���
    private bool isTransitioning = false; // ��ȯ ������ ����

    void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ ���
        if (switchButton != null)
        {
            Debug.Log("��ư ����");
            switchButton.onClick.AddListener(OnButtonClick);
        }

        // CanvasGroup �迭 �ʱ�ȭ
        if (canvasGroups.Length > 0)
        {
            foreach (var canvasGroup in canvasGroups)
            {
                canvasGroup.alpha = 0; // ��� �̹����� ����
            }
            canvasGroups[currentIndex].alpha = 1; // ù ��° �̹����� ���̵��� ����
        }
    }

    public void OnButtonClick()
    {
        if (isTransitioning || canvasGroups.Length <= 1) return;
            StartCoroutine(SwitchImage());
    }

    private IEnumerator SwitchImage()
    {
        isTransitioning = true;

        // ���� �̹����� ������ �����ϰ� �����
        float elapsedTime = 0f;
        CanvasGroup currentCanvasGroup = canvasGroups[currentIndex];

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / transitionDuration);
            currentCanvasGroup.alpha = alpha;
            yield return null;
        }

        // ���� �̹����� ����
        currentCanvasGroup.alpha = 0;

        // ���� �̹����� ��ȯ
        currentIndex = (currentIndex + 1) % canvasGroups.Length;
        CanvasGroup nextCanvasGroup = canvasGroups[currentIndex];
        nextCanvasGroup.alpha = 0;
        nextCanvasGroup.gameObject.SetActive(true);

        // ���� �̹����� ������ ��Ÿ���� �ϱ�
        elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / transitionDuration);
           // Color color = fadeOutIn.color;
           // color.a = alpha;
            nextCanvasGroup.alpha = alpha;
            yield return null;
        }

        isTransitioning = false;
    }
}