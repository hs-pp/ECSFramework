using System;
using NUnit.Framework;

namespace EcsFramework.Tests
{
    public class System_Tests : EcsUnitTests
    {
        private struct SystemTestComponent : IComponent { }
        
        private class TestSystem1 : SystemBase
        {
            public bool InitWasCalled = false;
            public Action<EcsIter> OnUpdate;

            public TestSystem1(World world) : base(world) { }
        
            protected override void BuildSystem(SystemBuilder systemBuilder)
            {
                systemBuilder.WithComponent<SystemTestComponent>();
            }
        
            protected override void OnInit()
            {
                InitWasCalled = true;
            }

            protected override void Update(EcsIter ecsIter)
            {
                OnUpdate?.Invoke(ecsIter);
            }
        }
        
        [Test]
        public void InitGetsCalled()
        {
            int trueEntityCount = 50;
            for (int i = 0; i < trueEntityCount; i++)
            {
                Entity entity = m_world.CreateEntity();
                entity.AddComponent<SystemTestComponent>();
            }

            TestSystem1 testSystem1 = m_world.RegisterSystem<TestSystem1>();
            
            Assert.True(testSystem1.InitWasCalled);
        }
        
        [Test]
        public void UpdatesCorrectly()
        {
            int trueEntityCount = 50;
            for (int i = 0; i < trueEntityCount; i++)
            {
                Entity entity = m_world.CreateEntity();
                entity.AddComponent<SystemTestComponent>();
            }

            int entityCount = 0;
            TestSystem1 testSystem1 = m_world.RegisterSystem<TestSystem1>();
            testSystem1.OnUpdate += iter =>
            {
                while (iter.MoveNext())
                {
                    entityCount++;
                }
            };
            
            m_world.Tick(0);

            Assert.True(entityCount == trueEntityCount);
        }
        
        [Test]
        public void DoesNotUpdateIfDisabled()
        {
            int trueEntityCount = 50;
            for (int i = 0; i < trueEntityCount; i++)
            {
                Entity entity = m_world.CreateEntity();
                entity.AddComponent<SystemTestComponent>();
            }

            int entityCount = 0;
            TestSystem1 testSystem1 = m_world.RegisterSystem<TestSystem1>();
            testSystem1.OnUpdate += iter =>
            {
                while (iter.MoveNext())
                {
                    entityCount++;
                }
            };

            testSystem1.IsEnabled = false;
            
            m_world.Tick(0);

            Assert.True(entityCount == 0);
        }
    }
}