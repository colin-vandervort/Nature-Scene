using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public int testAnimalCount;

    public Mesh testAnimalMesh;
    public Material testAnimalMaterial;

    public Entity environmentManager;

    private static Bootstrap instance;
    public static Bootstrap GetInstance()
    {
        return instance;
    }

    //Kick off the systems
    private void Awake()
    {
        instance = this;

        EntityManager entityManager = World.Active.EntityManager;

        EntityArchetype seasonManagerArchetype = entityManager.CreateArchetype(
            typeof(EnvironmentManager) //Might split this up later, but holds all the data for the world environment
            );

        EntityArchetype testAnimalArchetype = entityManager.CreateArchetype(
            typeof(Translation),    //The animal needs a position
            typeof(LocalToWorld),   //The sprite needs a world position
            typeof(RenderMesh),     //The animal needs a sprite
            typeof(AnimalBehaviour) //The animal can be doing different things
            );

        NativeArray<Entity> seasonManagerArray = new NativeArray<Entity>(1, Allocator.Temp);
        NativeArray<Entity> testAnimalArray = new NativeArray<Entity>(testAnimalCount, Allocator.Temp);

        entityManager.CreateEntity(seasonManagerArchetype, seasonManagerArray);
        entityManager.CreateEntity(testAnimalArchetype, testAnimalArray);

        for (int i = 0; i < seasonManagerArray.Length; i++)
        {
            Entity entity = seasonManagerArray[i];
            entityManager.SetComponentData(entity, new EnvironmentManager
            {
                cloudiness = 71f,     //71% cloud coverage -- average on Jan 1st according to weatherspark
                month = 0,            //January
                raininess = 48f,      //The heaviness of rain. Jan = 48, Feb = 44, Mar = 36, Apr = 32, May = 34, Jun = 28, Jul = 24, Aug = 28, Sep = 40, Oct = 48, Nov = 56, Dec = 52
                temperature = 37f,    //Temperature at the start of the day.
                timeOfDay = 0f,       //Midnight in hours
                windDirection = 85f,  //85% chance of blowing south
                windiness = 5f,       //5mph wind averages in January
                intervalCounter = 3f, //3f = one complete interval
                intervalMet = false,  //Doesn't matter which way we start because the counter should set this if it starts from 3f
                currentCloudinessTarget = 71f,
                currentRaininessTarget = 56f,
                displayMonth = 11,
                displayTime = 21f,
                currentTemperatureTarget = 40f,
                currentTenToNinetyPercentileSpread = 9f
            });
        }

        if (seasonManagerArray.Length > 0)
            environmentManager = seasonManagerArray[0];

        for(int i = 0; i < testAnimalArray.Length; i++)
        {
            Entity entity = testAnimalArray[i];
            entityManager.SetComponentData(entity, new Translation { Value = new float3(0f, 0f, 0f) });
            entityManager.SetComponentData(entity, new AnimalBehaviour { state = AnimalBehaviour.State.waiting });

            entityManager.SetSharedComponentData(entity, new RenderMesh
            {
                mesh = testAnimalMesh,
                material = testAnimalMaterial,
            });
        }
    }
}
