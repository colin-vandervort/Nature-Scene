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
    private CloudCoverage[] cloudCoverageCurves;
}
