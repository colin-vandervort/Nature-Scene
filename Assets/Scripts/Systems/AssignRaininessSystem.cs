using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using Unity.Mathematics;

[UpdateAfter(typeof(AssignCloudinessSystem))]
public class AssignRaininessSystem : JobComponentSystem
{
    [BurstCompile]
    public struct AssignRaininessJob : IJobForEach<EnvironmentManager>
    {
        public float RNG;

        public float dT;

        public void Execute(ref EnvironmentManager environmentManager)
        {
            if (environmentManager.intervalMet)
            {
                if (environmentManager.currentCloudinessTarget > 0f)
                {
                    //If we have clouds
                    if (environmentManager.cloudiness > 0f)
                        environmentManager.currentRaininessTarget = environmentManager.rainfallChance / (100f - environmentManager.currentClearNode) / environmentManager.rainfallQuantity * RNG * 20f; //23.5 is the highest density we've seen.
                    else //Shouldn't really need this, might cause issues, but should never really be hit without raininess being close to low already.
                        environmentManager.raininess = 0f;
                }
                else
                    environmentManager.currentRaininessTarget = 0f;

                if (environmentManager.currentRaininessTarget > 100f)
                    environmentManager.currentRaininessTarget = 100f;
                else if (environmentManager.currentRaininessTarget < 0f)
                    environmentManager.currentRaininessTarget = 0f;
            }
            environmentManager.raininess = math.lerp(environmentManager.raininess, environmentManager.currentRaininessTarget, dT * (1f / 45f)); //adjust for rate of change of clouds. 1/6th the rate of change of hours.
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new AssignRaininessJob
        {
            RNG = UnityEngine.Random.Range(0f, 35f), //Tune this to fit. 20 is probably the "clean" number, but 35 might be a better average. If we're too rainy on average, go back to 20.
            dT = Time.deltaTime
        };

        return job.Schedule(this, inputDeps);
    }
}
