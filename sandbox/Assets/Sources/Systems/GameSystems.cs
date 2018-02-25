public sealed class GameSystems : Feature
{
    public GameSystems(Contexts contexts)
    {
        // Initialization
        Add(new CreatePlayerSystem(contexts));



        // Input



        // Update
        Add(new LogHealthSystem(contexts));
        Add(new HealthSystem(contexts));



        // View / Render



        // Cleanup
        Add(new DestroyEntitySystem(contexts));
    }
}
