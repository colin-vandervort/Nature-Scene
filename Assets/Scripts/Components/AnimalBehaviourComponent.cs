using Unity.Entities;

public struct AnimalBehaviour : IComponentData
{
    public enum State
    {
        entering,
        leaving,
        waiting,
        eating,
        foraging,
        calling,
        socializing
    }

    public State state;
}
