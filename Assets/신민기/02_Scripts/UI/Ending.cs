using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Ending : MonoBehaviour
{
    public CanvasGroup[] canvasGroups;
    public float transitionDuration = 0.5f;

    private int currentIndex = 0;
    public bool isTransitioning = false;

    public Image DialogueBox;

    public CircleFadeInOutUI circleFadeInOutUI;
    public SoundManager theSound;
    public VideoPlayer lastCutScene;
    public string endingBGM;

    public Image fadeOut;
    bool musicFaded;

    private void Awake()
    {
        circleFadeInOutUI = FindObjectOfType<CircleFadeInOutUI>();
        theSound = FindObjectOfType<SoundManager>();
        lastCutScene.enabled = true;
    }

    private void Start()
    {
        foreach (var image in canvasGroups)
            image.gameObject.SetActive(false);

        canvasGroups[0].gameObject.SetActive(true);
        circleFadeInOutUI.FadeIn();
        fadeOut.gameObject.SetActive(false);
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
        if (currentIndex < canvasGroups.Length - 1)
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

    private IEnumerator FadeInImage(CanvasGroup canvasGroup,float transitionDuration = 0.5f)
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

    public void LastEndingScene()
    {
        StartCoroutine(LastFadeInImage(canvasGroups[3]));
    }

    private IEnumerator LastFadeInImage(CanvasGroup canvasGroup)
    {
        canvasGroup.gameObject.SetActive(true);
        lastCutScene.Play();
        yield return new WaitForSeconds(6f);
        fadeOut.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        SoundManager.instance.FadeOutMusic();
        yield return new WaitForSeconds(3f);
        Application.Quit();
    }
}
