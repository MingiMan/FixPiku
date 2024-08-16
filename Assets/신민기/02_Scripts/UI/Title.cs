using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string titleBgm;
    public string startBtn;
    SoundManager theSound;
    CircleFadeInOutUI circleFadeInOut;
    bool mouseInput;
    public string prologBGM;

    private void Awake()
    {
        theSound = FindObjectOfType<SoundManager>();
        circleFadeInOut = FindObjectOfType<CircleFadeInOutUI>();
    }

    private void OnEnable()
    {
        mouseInput = true;
        theSound.PlayBGM(titleBgm);
    }

    public void GoToProlog()
    {
        if (mouseInput)
        {
            mouseInput = false;
            StartCoroutine(Prolog());
        }
    }

    IEnumerator Prolog()
    {
        theSound.PlaySFX(startBtn);
        yield return new WaitForSeconds(1f);
        circleFadeInOut.FadeOut();
        yield return new WaitForSeconds(1f);
        theSound.FadeOutMusic();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
        theSound.PlayBGM(prologBGM);
        theSound.FadeInMusic();
    }
}
