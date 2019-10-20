using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using Unity.Mathematics;

[UpdateAfter(typeof(AssignRaininessSystem))]
public class AssignTemperatureSystem : JobComponentSystem
{
    [BurstCompile]
    public struct AssignTemperatureJob : IJobForEach<EnvironmentManager>
    {
        public float RNG;

        public float dT;

        public void Execute(ref EnvironmentManager environmentManager)
        {
            if (environmentManager.intervalMet)
            {
                environmentManager.intervalMet = false;
                environmentManager.intervalCounter -= 3f;

                environmentManager.currentTemperatureTarget += environmentManager.currentTenToNinetyPercentileSpread * RNG;
            }
            environmentManager.temperature = math.lerp(environmentManager.temperature, environmentManager.currentTemperatureTarget, dT * (1f / 45f)); //adjust for rate of change of clouds. 1/6th the rate of change of hours.
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new AssignTemperatureJob
        {
            RNG = UnityEngine.Random.Range(-1.12f, 1.12f), //adjust for temperature extremes. Might want to exagerate for giggles.
            dT = Time.deltaTime
        };

        return job.Schedule(this, inputDeps);
    }
}
