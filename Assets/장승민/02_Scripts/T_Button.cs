using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInButton : MonoBehaviour
{
    public CanvasGroup canvasGroup; // ��ư�� CanvasGroup ������Ʈ
    public float fadeDuration = 1.0f; // ������ ���̰� �� �ð�
    public float delay = 2.0f; // ��ư�� ���̱������ ��� �ð�

    void Start()
    {
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup is not assigned.");
            return;
        }

        // ��ư�� ó���� �����ϰ� ����
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // �ڷ�ƾ ����
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        // ��� �ð�
        yield return new WaitForSeconds(delay);

        float elapsedTime = 0f;

        // ������ ���̰� �ϱ�
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        // �Ϸ� �� ���� ����
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}