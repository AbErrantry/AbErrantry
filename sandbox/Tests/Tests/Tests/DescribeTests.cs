using NSpec;

class describe_HealthSystem : nspec
{
    void when_executing()
    {
        it["flags entities as destroyed when health is 0"] = () =>
        {
            // arrange
            var contexts = new Contexts();
            var system = new HealthSystem(contexts);
            var entity = contexts.game.CreateEntity();
            entity.AddHealth(0);

            // act
            system.Execute();

            // assert
            entity.isDestroyed.should_be_true();
        };
    }
}