using Entitas;

public sealed class CreatePlayerSystem : IInitializeSystem
{
    private readonly Contexts _contexts;

    public CreatePlayerSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Initialize()
    {
        var gameEntity = _contexts.game.CreateEntity();
        gameEntity.AddHealth(100);
    }
}
