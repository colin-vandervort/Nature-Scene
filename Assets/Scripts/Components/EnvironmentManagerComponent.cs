using Unity.Entities;

public struct EnvironmentManager : IComponentData
{
    public int month;           //0 = January, 11 = December
    public float timeOfDay;     //Military hours - Note, this is currently 3 hours off. If we want to display 6:00 stats, this clock needs to be at 9:00.
    public float cloudiness;    //0 = no clouds, 100 = full cloud coverage
    public float raininess;     //0 = no rain, 100 = heavy rain
    public float temperature;   //fahrenheit
    public float windiness;     //in mph
    public float windDirection; //0 = left (N), 100 = right (S)

    public int displayMonth;
    public float displayTime;

    public float intervalCounter; //Used to track weather patern interval adjustments
    public bool intervalMet;      //Used to mark a ready interval
    public float currentCloudinessTarget; //Used to track the current float value for the cloudiness target

    //Used to store frame by frame cloudiness node reference points
    public float currentClearNode;
    public float currentMostlyClearNode;
    public float currentPartlyCloudyNode;
    public float currentMostlyCloudyNode;
}
