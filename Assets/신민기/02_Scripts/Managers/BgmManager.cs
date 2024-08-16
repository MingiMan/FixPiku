using System.Collections;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    static public BgmManager instance;

    [Header("0 : 타이틀 1 : 프롤로그 2 : 인게임 3 : 밤 전투")]
    public AudioClip[] clips;
    public AudioSource source;

    WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        source = GetComponent<AudioSource>();
    }

    public void BgmPlay(int _playMusicTrack)
    {
        // source.volume = 1;
        source.clip = clips[_playMusicTrack];
        source.Play();
    }

    public void SetVolumn(float _volume)
    {
        source.volume = _volume;
    }

    public void Pause()
    {
        source.Pause();
    }

    public void UnPause()
    {
        source.UnPause();
    }
    public void BgmStop()
    {
        source.Stop();
    }

    public void FadeOutMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutMusicCoroutine());
    }

    public void FadeInMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInMusicCoroutine());
    }

    IEnumerator FadeOutMusicCoroutine()
    {
        for (float i = 1.0f; i >= 0.0f; i -= 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }

    IEnumerator FadeInMusicCoroutine()
    {
        for (float i = 0.1f; i <= 1f; i += 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }
}
