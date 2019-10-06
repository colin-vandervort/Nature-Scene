using Unity.Entities;

public struct EnvironmentManager : IComponentData
{
    public int month;         //0 = January, 11 = December
    public float timeOfDay;     //Military hours
    public float cloudiness;    //0 = no clouds, 100 = full cloud coverage
    public float raininess;     //0 = no rain, 100 = heavy rain
    public float temperature;   //fahrenheit
    public float windiness;     //in mph
    public float windDirection; //0 = left (N), 100 = right (S)
}
