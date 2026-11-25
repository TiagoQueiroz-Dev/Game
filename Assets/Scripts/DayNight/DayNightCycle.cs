using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time")]
    [Tooltip("Day Length in Minutes")]
    [SerializeField] private float _targetDayLength = 0.5f;   // Length of the Day in minutes
    public float targetDayLength
    {
        get { return _targetDayLength; }
    }

    [SerializeField] [Range(0f, 1f)] private float _timeOfDay;
    public float timeOfDay
    {
        get { return _timeOfDay; }
    }

    [SerializeField] private int _dayNumber = 0;    // Tracks the day passed
    public int dayNumber
    {
        get { return _dayNumber; }
    }

    [SerializeField] private int _yearNumber = 0;
    public int yearNumber
    {
        get { return _yearNumber; }
    }

    private float _timeScale = 100f;        //Variable for conversion between Real Time and game Time

    [SerializeField] private int _yearLength = 100;
    public float yearLength
    {
        get { return _yearLength; }
    }

    public bool pause = false;

    [Header("Sun Light")]
    [SerializeField] private Transform dailyRotation;
    [SerializeField] private Light Sun;
    private float intensity;
    [SerializeField] private float sunBasedIntensity = 1f;
    [SerializeField] private float sunVariation = 1.5f;
    [SerializeField] private Gradient sunColor;

    [Header("Seasonal Variables")]
    [SerializeField] private Transform sunSeasonalRotation;
    [SerializeField][Range(-45f, 45f)] private float maxSeasonalTilt;

    [Header("Modules")]
    private List<DN_ModuleBase> moduleList = new List<DN_ModuleBase>();

    private void Update()
    {
        if (!pause)
        {
            UpdateTimeScale();
            UpdateTime();
        }

        AdjustSunRotation();
        SunIntensity();
        UpdateModules();
    }

    private void UpdateTimeScale()
    {
        _timeScale = 24 / (targetDayLength / 60);
    }


    private void UpdateTime()
    {
        _timeOfDay += Time.deltaTime * _timeScale / 86400; // Seconds in a day

        if (_timeOfDay > 1)         // new Day
        {
            _dayNumber++;
            _timeOfDay -= 1;

            if (_dayNumber > _yearLength)           //New Year
            {
                _yearNumber++;
                _dayNumber = 0;
            }
        }
    }

    //Rotates the Sun daily
    private void AdjustSunRotation()
    {
        float sunAngle = timeOfDay * 360f;
        dailyRotation.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, sunAngle));

        float seasonalAngle = -maxSeasonalTilt * Mathf.Cos(dayNumber / _yearLength * 2f * Mathf.PI);
        sunSeasonalRotation.localRotation = Quaternion.Euler(new Vector3(seasonalAngle, 0f, 0f));
    }

    private void SunIntensity()
    {
        intensity = Vector3.Dot(Sun.transform.forward, Vector3.down);
        intensity = Mathf.Clamp01(intensity);

        Sun.intensity = intensity * sunVariation * sunBasedIntensity;
    }

    private void AdjustSunColor()
    {
        Sun.color = sunColor.Evaluate(intensity);
    }

    public void AddModule(DN_ModuleBase module)
    {
        moduleList.Add(module);
    }

    public void RemoveModule(DN_ModuleBase module)
    {
        moduleList.Remove(module);
    }

    private void UpdateModules()
    {
        foreach (DN_ModuleBase module in moduleList)
        {
            module.UpdateModule(intensity);
        }
    }
}
