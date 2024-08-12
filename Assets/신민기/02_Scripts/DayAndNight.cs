using TMPro;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField] float secondPerRealTimeSecond;
    bool IsNight = false;
    [SerializeField] float fogDensityCale;
    [SerializeField] float nightFogDensity;

    float dayFogDensity;
    float currentFogDensity;

    bool wasNight = false;

    [SerializeField] TextMeshProUGUI dayText;

    int dayCount = 0;

    private void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
    }

    private void Update()
    {
        float rotate = 1f * secondPerRealTimeSecond * Time.deltaTime;

        transform.Rotate(Vector3.right, rotate);

        if (transform.eulerAngles.x >= 170)
            IsNight = true;

        else if (transform.eulerAngles.x <= 340)
            IsNight = false;

        if (IsNight)
        {
            if (currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0.1f * fogDensityCale * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else
        {
            if (currentFogDensity >= dayFogDensity)
            {
                currentFogDensity -= 0.1f * fogDensityCale * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
    }
}
