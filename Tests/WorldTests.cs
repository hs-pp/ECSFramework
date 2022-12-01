using NUnit.Framework;

namespace EcsFramework.Tests
{
    public struct WorldTestComponent : IComponent
    {
    }
    
    /// <summary>
    /// These tests should call functions from World.cs and then strictly use flecs wrapper macros
    /// to assert the results!
    ///
    /// We're testing if our custom functions reflect properly in flecs.
    /// Our tests are not responsible for anything happening in flecs.
    /// </summary>
    public unsafe class WorldTests
    {
        [Test]
        public void CreatedWorldExists()
        {
            World world = new World();
            var worldInfo = flecs.ecs_get_world_info(world);
            Assert.True(worldInfo != null);
        }

        [Test]
        public void CreatedEntityExistsAndIsAlive()
        {
            World world = new World();

            Entity entity = world.CreateEntity();

            Assert.True(flecs.ecs_exists(world, entity));
            Assert.True(flecs.ecs_is_alive(world, entity));

            world.Dispose();
        }

        [Test]
        public void DeletedEntityExistsButIsNotAlive()
        {
            World world = new World();

            Entity entity = world.CreateEntity();

            Assert.True(flecs.ecs_exists(world, entity));
            Assert.True(flecs.ecs_is_alive(world, entity));

            world.DeleteEntity(entity);

            Assert.True(flecs.ecs_exists(world, entity));
            Assert.True(!flecs.ecs_is_alive(world, entity));

            world.Dispose();
        }

        [Test]
        public void ComponentIsAddedToEntity()
        {
            World world = new World();

            Entity entity = world.CreateEntity();
            entity.AddComponent<WorldTestComponent>();
            
            world.Tick(0);
            
            Assert.True(entity.HasComponent<WorldTestComponent>());
            world.Dispose();
        }
    }
}