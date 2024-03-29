﻿using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using Unity.Mathematics;

[UpdateAfter(typeof(AssignCurrentCurvesSystem))]
public class AssignCloudinessSystem : JobComponentSystem
{
    [BurstCompile]
    public struct AssignCloudinessJob : IJobForEach<EnvironmentManager>
    {
        public float macroRNG;
        public float microRNG;

        public float dT;

        public void Execute(ref EnvironmentManager environmentManager)
        {
            if (environmentManager.intervalMet)
            {
                float currentRandomCloudIndex = macroRNG;

                if (environmentManager.currentClearNode >= currentRandomCloudIndex)
                    environmentManager.currentCloudinessTarget = 0f;
                else if (environmentManager.currentMostlyClearNode >= currentRandomCloudIndex)
                    environmentManager.currentCloudinessTarget = microRNG;
                else if (environmentManager.currentPartlyCloudyNode >= currentRandomCloudIndex)
                    environmentManager.currentCloudinessTarget = 25f + microRNG;
                else if (environmentManager.currentMostlyCloudyNode >= currentRandomCloudIndex)
                    environmentManager.currentCloudinessTarget = 50f + microRNG;
                else
                    environmentManager.currentCloudinessTarget = 75f + microRNG;
            }
            environmentManager.cloudiness = math.lerp(environmentManager.cloudiness, environmentManager.currentCloudinessTarget, dT * (1f / 45f)); //adjust for rate of change of clouds. 1/6th the rate of change of hours.
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new AssignCloudinessJob
        {
            macroRNG = UnityEngine.Random.Range(0f, 100f),
            microRNG = UnityEngine.Random.Range(0f, 25f),
            dT = Time.deltaTime
        };

        return job.Schedule(this, inputDeps);
    }
}
