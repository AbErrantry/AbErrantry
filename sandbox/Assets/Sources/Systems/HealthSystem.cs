using Entitas;
using System.Collections.Generic;

public sealed class HealthSystem : ReactiveSystem<GameEntity>
{
    public HealthSystem(Contexts contexts) : base(contexts.game)
    {

    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Health);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasHealth;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (entity.health.Value <= 0)
            {
                entity.isDestroyed = true;
            }
        }
    }
}

