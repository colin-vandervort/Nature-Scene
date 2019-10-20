using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[UpdateAfter(typeof(AssignTimeAndMonthSystem))]
public class AssignCurrentCurvesSystem : JobComponentSystem
{
    public struct AssignCurrentCurvesJob : IJobForEach<EnvironmentManager>
    {
        public float macroRNG;
        public float microRNG;

        public void Execute(ref EnvironmentManager environmentManager)
        {
            if (environmentManager.intervalMet)
            {
                CloudCoverage cloudCoverage = WeatherDataCurves.GetInstance().cloudCoverageCurves[environmentManager.month];

                environmentManager.currentClearNode = cloudCoverage.clear.Evaluate(environmentManager.timeOfDay);
                environmentManager.currentMostlyClearNode = cloudCoverage.mostlyClear.Evaluate(environmentManager.timeOfDay);
                environmentManager.currentPartlyCloudyNode = cloudCoverage.partlyCloudy.Evaluate(environmentManager.timeOfDay);
                environmentManager.currentMostlyCloudyNode = cloudCoverage.mostlyCloudy.Evaluate(environmentManager.timeOfDay);

                Raininess raininess = WeatherDataCurves.GetInstance().rainfallData[environmentManager.month];

                environmentManager.rainfallChance = raininess.rainfallChance;
                environmentManager.rainfallQuantity = raininess.rainfallHeaviness;

                Temperature temperature = WeatherDataCurves.GetInstance().temperatureData[environmentManager.month];

                environmentManager.currentTemperatureTarget = temperature.averageTemperature.Evaluate(environmentManager.timeOfDay);
                environmentManager.currentTenToNinetyPercentileSpread = temperature.tenToNinetyPercentileSpread.Evaluate(environmentManager.timeOfDay);
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new AssignCurrentCurvesJob();
        return job.Schedule(this, inputDeps);
    }
}
