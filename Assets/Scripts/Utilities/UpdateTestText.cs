using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;

public class UpdateTestText : MonoBehaviour
{
    public Text monthText;
    public Text timeText;
    public Text cloudDensityText;
    public Text rainDensityText;
    public Text temperatureText;
    public Text windSpeedText;
    public Text windDirectionText;

    private Entity seasonManager;
    private EnvironmentManager environmentManager;
    private EntityManager entityManager;

    private void Start()
    {
        entityManager = World.Active.EntityManager;
        seasonManager = Bootstrap.GetInstance().environmentManager;
    }

    void Update()
    {
        environmentManager = entityManager.GetComponentData<EnvironmentManager>(seasonManager);

        monthText.text = "Month: " + environmentManager.displayMonth + " Logical Month: " + environmentManager.month;
        timeText.text = "Time: " + environmentManager.displayTime + " Logical Time: " + environmentManager.timeOfDay;
        cloudDensityText.text = "Cloud Density: " + environmentManager.cloudiness;
        rainDensityText.text = "Rain Density: " + environmentManager.raininess;
        temperatureText.text = "Temperature: " + environmentManager.temperature;
        windSpeedText.text = "Wind Speed: " + environmentManager.windiness;
        windDirectionText.text = "Wind Direction: " + environmentManager.windDirection;
    }
}
