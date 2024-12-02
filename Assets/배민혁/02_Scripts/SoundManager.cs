using System.Collections;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] Sound[] sfx = null;
    [SerializeField] Sound[] bgm = null;

    [SerializeField] AudioSource bgmPlayer = null;
    [SerializeField] AudioSource[] sfxPlayer = null;
    WaitForSeconds waitTime = new WaitForSeconds(0.05f);

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBGM(string p_bgmName)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (p_bgmName == bgm[i].name)
            {
                bgmPlayer.clip = bgm[i].clip;
                bgmPlayer.loop = true;
                bgmPlayer.Play();
            }
        }
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void PlaySFX(string p_sfxName)
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == sfx[i].name)
            {
                for (int j = 0; j < sfxPlayer.Length; j++)
                {
                    // SFXPlayer에서 재생 중이지 않은 Audio Source를 발견했다면 
                    if (!sfxPlayer[j].isPlaying)
                    {
                        sfxPlayer[j].clip = sfx[i].clip;
                        sfxPlayer[j].Play();
                        // Debug.Log("효과음 재생");
                        return;
                    }
                }
                // Debug.Log("모든 오디오 플레이어가 재생중입니다.");
                return;
            }
        }
        // Debug.Log(p_sfxName + " 이름의 효과음이 없습니다.");
        return;
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
        for (float i = 0.3f; i >= 0.0f; i -= 0.01f)
        {
            bgmPlayer.volume = i;
            yield return waitTime;
        }
    }

    IEnumerator FadeInMusicCoroutine()
    {
        for (float i = 0.1f; i <= 0.3f; i += 0.01f)
        {
            bgmPlayer.volume = i;
            yield return waitTime;
        }
    }


    #region 간단버전
    // private SoundManager _instance;

    // public AudioSource bgSound;
    // public AudioClip[] bgList;
    // public enum Sound
    // {
    //     Bgm,
    //     Effect,
    //     MaxCount,  // 아무것도 아님. 그냥 Sound enum의 개수 세기 위해 추가. (0, 1, '2' 이렇게 2개) 
    // }
    // void Awake()
    // {
    //     if (_instance == null)
    //     {
    //         _instance = this;
    //         SceneManager.sceneLoaded += OnSceneLoaded;
    //     }
    //     else if (_instance != this)
    //     {
    //         Destroy(gameObject);
    //     }
    //     DontDestroyOnLoad(gameObject);
    // }

    // private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    // {
    //     for (int i = 0; i < bgList.Length; i++)
    //     {
    //         if (arg0.name == bgList[i].name)
    //         {
    //             BgSoundPlay(bgList[i]);
    //         }

    //     }
    // }
    // public void BgSoundPlay(AudioClip clip)
    // {
    //     bgSound.clip = clip;
    //     bgSound.loop = true;
    //     bgSound.volume = 0.1f;
    //     bgSound.Play();
    // }
    #endregion
}

