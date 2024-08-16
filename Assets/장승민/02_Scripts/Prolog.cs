using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwitcher : MonoBehaviour
{
    public CanvasGroup[] canvasGroups; // 전환할 CanvasGroup 배열
    public float transitionDuration = 0.5f; // 전환 시간 (초)

    private int currentIndex = 0; // 현재 이미지 인덱스
    public bool isTransitioning = false; // 전환 중인지 여부

    public Image DialogueBox;

    public CircleFadeInOutUI circleFadeInOutUI;
    public string prologBGM;

    private void Awake()
    {
        circleFadeInOutUI = FindObjectOfType<CircleFadeInOutUI>();
    }

    private void Start()
    {
        foreach (var image in canvasGroups)
            image.gameObject.SetActive(false);

        canvasGroups[0].gameObject.SetActive(true);
        circleFadeInOutUI.FadeIn();
    }

    public void NextImage()
    {
        if (!isTransitioning)
            StartCoroutine(SwitchImage());
    }

    private IEnumerator SwitchImage()
    {
        isTransitioning = true;

        // 현재 이미지를 서서히 투명하게 만들기
        yield return StartCoroutine(FadeOutImage(canvasGroups[currentIndex]));

        // 다음 이미지 설정 및 서서히 나타나게 하기
        currentIndex++;
        if (currentIndex < canvasGroups.Length)
        {
            canvasGroups[currentIndex].gameObject.SetActive(true);
            yield return StartCoroutine(FadeInImage(canvasGroups[currentIndex]));
        }

        yield return new WaitForSeconds(0.5f);
        isTransitioning = false;
    }

    private IEnumerator FadeOutImage(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / transitionDuration);
            canvasGroup.alpha = alpha;

            SetDialogueBoxAlpha(alpha);
            yield return null;
        }

        canvasGroup.alpha = 0;
    }

    private IEnumerator FadeInImage(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / transitionDuration);
            canvasGroup.alpha = alpha;

            SetDialogueBoxAlpha(alpha);
            yield return null;
        }
    }

    private void SetDialogueBoxAlpha(float alpha)
    {
        if (DialogueBox != null)
        {
            Color color = DialogueBox.color;
            color.a = alpha;
            DialogueBox.color = color;
        }
    }

    public void GoToMainGame()
    {
        StartCoroutine(MainGame());
    }

    private IEnumerator MainGame()
    {
        yield return new WaitForSeconds(0.5f);
        circleFadeInOutUI.FadeOut();
        yield return new WaitForSeconds(3f);
    }
}