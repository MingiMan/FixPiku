using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    [SerializeField] private Texture2D skyboxNight; // ��
    [SerializeField] private Texture2D skyboxSunrise; // �¾��� ��������.
    [SerializeField] private Texture2D skyboxDay;
    [SerializeField] private Texture2D skyboxSunset; // ����

    [SerializeField] private Gradient graddientNightToSunrise;
    [SerializeField] private Gradient graddientSunriseToDay;
    [SerializeField] private Gradient graddientDayToSunset;
    [SerializeField] private Gradient graddientSunsetToNight;

    [SerializeField] private Light globalLight;

    private int minutes;

    public int Minutes
    { get { return minutes; } set { minutes = value; OnMinutesChange(value); } }

    private int hours = 8;

    public int Hours
    { get { return hours; } set { hours = value; OnHoursChange(value); } }

    private int days;

    public int Days
    { get { return days; } set { days = value; } }

    private float tempSecond;

    public int timer;

    public bool nightCheck = false;//밤낮 확인////////////////////

    public bool timeLock;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(Instance);
    }

    public void Update()
    {
        if (!timeLock)
        {
            tempSecond += Time.deltaTime;

            if (tempSecond >= 1)
            {
                Minutes += 1;
                tempSecond = 0;
            }
        }
    }

    private void OnMinutesChange(int value)
    {
        globalLight.transform.Rotate(Vector3.up, (1f / (1440f / 4f)) * 360f, Space.World);
        if (value >= timer)
        {
            Hours++;
            minutes = 0;
        }
        if (Hours >= 24)
        {
            Hours = 0;
            Days++;
        }
    }

    private void OnHoursChange(int value)
    {
        if (value == 6) 
        {
            GameManager.Instance.monsterSpawner.AllMonsterDeath();
            StartCoroutine(LerpSkybox(skyboxNight, skyboxSunrise, 10f));
            StartCoroutine(LerpLight(graddientNightToSunrise, 10f));
            nightCheck = false;
        }
        else if (value == 8) 
        {
            StartCoroutine(LerpSkybox(skyboxSunrise, skyboxDay, 10f));
            StartCoroutine(LerpLight(graddientSunriseToDay, 10f));
        }
        else if (value == 18) 
        {
            StartCoroutine(LerpSkybox(skyboxDay, skyboxSunset, 10f));
            StartCoroutine(LerpLight(graddientDayToSunset, 10f));
        }

        else if (value == 20)
        {
            GameManager.Instance.WaringUISetAcitve();
        }

        else if (value == 22 && !timeLock) 
        {
            StartCoroutine(LerpSkybox(skyboxSunset, skyboxNight, 10f));
            StartCoroutine(LerpLight(graddientSunsetToNight, 10f));
            GameManager.Instance.OnMonsterSpawnForLevel();
            nightCheck = true;
        }
    }

    private IEnumerator LerpSkybox(Texture2D a, Texture2D b, float time)
    {
        RenderSettings.skybox.SetTexture("_Texture1", a);
        RenderSettings.skybox.SetTexture("_Texture2", b);
        RenderSettings.skybox.SetFloat("_Blend", 0);
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            RenderSettings.skybox.SetFloat("_Blend", i / time);
            yield return null;  
        }
        RenderSettings.skybox.SetTexture("_Texture1", b);
    }

    private IEnumerator LerpLight(Gradient lightGradient, float time)
    {
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            globalLight.color = lightGradient.Evaluate(i / time);
            RenderSettings.fogColor = globalLight.color;
            yield return null;
        }
    }

}
