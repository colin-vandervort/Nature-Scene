using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CloudCoverage
{
    public string month;
    public AnimationCurve clear;
    public AnimationCurve mostlyClear;
    public AnimationCurve partlyCloudy;
    public AnimationCurve mostlyCloudy;
    //If none of these, overcast.
}

[System.Serializable]
public class Raininess
{
    public string month;
    public float rainfallChance;
    public float rainfallHeaviness;
}

[System.Serializable]
public class Temperature
{
    public string month;
    public AnimationCurve averageTemperature;
    public AnimationCurve tenToNinetyPercentileSpread;
}


public class WeatherData
{
    public CloudCoverage cloudCoverage;

    public WeatherData
        (
        CloudCoverage cloudCoverage
        )
    {
        this.cloudCoverage = cloudCoverage;
    }
}

public class WeatherDataCurves : MonoBehaviour
{
    [SerializeField]
    public CloudCoverage[] cloudCoverageCurves;

    [SerializeField]
    public Raininess[] rainfallData;

    [SerializeField]
    public Temperature[] temperatureData;

    private static WeatherDataCurves instance;
    public static WeatherDataCurves GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }
}
