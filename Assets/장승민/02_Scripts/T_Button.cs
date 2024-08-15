using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInButton : MonoBehaviour
{
    public CanvasGroup canvasGroup; // 버튼의 CanvasGroup 컴포넌트
    public float fadeDuration = 1.0f; // 서서히 보이게 할 시간
    public float delay = 2.0f; // 버튼이 보이기까지의 대기 시간

    void Start()
    {
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup is not assigned.");
            return;
        }

        // 버튼을 처음에 투명하게 설정
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // 코루틴 시작
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        // 대기 시간
        yield return new WaitForSeconds(delay);

        float elapsedTime = 0f;

        // 서서히 보이게 하기
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        // 완료 후 상태 설정
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}