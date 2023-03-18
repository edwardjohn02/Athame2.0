using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameCreator;


public class TimeController : MonoBehaviour
{
    [SerializeField]
    private float timeMultiplierBase;

    [SerializeField]
    private GameObject Asleep;

    [SerializeField] 
    private float startHour;

    [SerializeField]
    private TextMeshProUGUI timeText;
   
    [SerializeField]
    private Light sunLight;

    [SerializeField]
    private float sunriseHour;

    [SerializeField]
    private float sunsetHour;

    [SerializeField]
    private Color dayAmbientLight;

    [SerializeField]
    private Color nightAmbientLight;

    [SerializeField]
    private AnimationCurve lightChangeCurve;

    [SerializeField]
    private float maxSunLightIntensity;

    [SerializeField]
    private Light moonLight;

    [SerializeField]
    private float maxMoonLightIntensity;


    private DateTime currentTime;

    private TimeSpan sunriseTime;
    private TimeSpan sunsetTime;
    private float timeMultiplier;
    private bool sleeping;

    // Start is called before the first frame update
    void Start()
    {
        sleeping = Asleep.activeSelf;
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);
        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);
        timeMultiplier = timeMultiplierBase;
    }

    // Update is called once per frame
    void Update()
    {
        sleeping = Asleep.activeSelf;

        if(sleeping)
        {
            timeMultiplier = timeMultiplierBase * 500;
        }

        else
        {
            timeMultiplier = timeMultiplierBase;
        }
        UpdateTimeOfDay();
        RotateSun();
        UpdateLightSettings();
    }

    private void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);
        if(timeText != null)
        {
            timeText.text = currentTime.ToString("HH:mm");
        }
    }

    private void RotateSun()
    {
        float sunLightRotation;
        if (currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime,sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime,currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0,180,(float)percentage);
        }
        else
        {
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime,sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime,currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes/sunsetToSunriseDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(180,360,(float)percentage);
        }
        
        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation,Vector3.right);

    }

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sunLight.transform.forward,Vector3.down);

        sunLight.intensity = Mathf.Lerp(0,maxSunLightIntensity,lightChangeCurve.Evaluate(dotProduct));
        moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity,0,lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight,dayAmbientLight,lightChangeCurve.Evaluate(dotProduct));

    }

    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;
        if (difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }
        return difference;
    }


}
