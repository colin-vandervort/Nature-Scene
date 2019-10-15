using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class SeasonManagementSystem : JobComponentSystem
{
    [BurstCompile]
    public struct SeasonManagementJob : IJobForEach<EnvironmentManager>
    {
        public float dT;
        public void Execute(ref EnvironmentManager environmentManager)
        {
            environmentManager.timeOfDay += dT * (2f / 15f); //How fast a day ticks. 180 second days. 7.5 second hours.

            if (environmentManager.timeOfDay >= 24f)
            {
                environmentManager.timeOfDay -= 24f; //Reset day
                environmentManager.month += 1;       //increment month
                if (environmentManager.month > 11)
                {
                    environmentManager.month -= 12;  //Reset month
                }
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new SeasonManagementJob
        {
            dT = Time.deltaTime
        };
        return job.Schedule(this, inputDeps);
    }
}
