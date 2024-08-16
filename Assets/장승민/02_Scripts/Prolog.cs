using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageSwitcher : MonoBehaviour
{
    public CanvasGroup[] canvasGroups; // 전환할 CanvasGroup 배열
    public Button switchButton; // 버튼 컴포넌트
    public float transitionDuration = 0.5f; // 전환 시간 (초)

    [SerializeField] Image fadeOutIn;
    private int currentIndex = 0; // 현재 이미지 인덱스
    private bool isTransitioning = false; // 전환 중인지 여부

    void Start()
    {
        // 버튼 클릭 이벤트 등록
        if (switchButton != null)
        {
            Debug.Log("버튼 눌림");
            switchButton.onClick.AddListener(OnButtonClick);
        }

        // CanvasGroup 배열 초기화
        if (canvasGroups.Length > 0)
        {
            foreach (var canvasGroup in canvasGroups)
            {
                canvasGroup.alpha = 0; // 모든 이미지를 숨김
            }
            canvasGroups[currentIndex].alpha = 1; // 첫 번째 이미지만 보이도록 설정
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

        // 현재 이미지를 서서히 투명하게 만들기
        float elapsedTime = 0f;
        CanvasGroup currentCanvasGroup = canvasGroups[currentIndex];

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / transitionDuration);
            currentCanvasGroup.alpha = alpha;
            yield return null;
        }

        // 현재 이미지를 숨김
        currentCanvasGroup.alpha = 0;

        // 다음 이미지로 전환
        currentIndex = (currentIndex + 1) % canvasGroups.Length;
        CanvasGroup nextCanvasGroup = canvasGroups[currentIndex];
        nextCanvasGroup.alpha = 0;
        nextCanvasGroup.gameObject.SetActive(true);

        // 다음 이미지를 서서히 나타나게 하기
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