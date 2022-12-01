using NUnit.Framework;
using UnityEngine;

namespace EcsFramework.Tests
{
    public class Observer_Tests : EcsUnitTests
    {
        private struct TestObserverTag : ITag { }
        private struct TestObserverTag2 : ITag { }
        private struct TestObserverComponent : IComponent
        {
            public bool CoolBool;
        }

        [Test]
        public void Get_Count_Component_One()
        {
            int matchCount = 0;
            void ObserverCallback(EcsIter ecsIter)
            {
                matchCount++;
            }
            
            var observer = new ObserverBuilder(m_world)
                .WithComponent<TestObserverComponent>()
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();
            
            Entity entity = m_world.CreateEntity();
            entity.AddComponent<TestObserverComponent>();
            
            Assert.True(matchCount == 1);
        }
        
        [Test]
        public void Get_Count_Component_Fifty()
        {
            int matchCount = 0;
            void ObserverCallback(EcsIter ecsIter)
            {
                matchCount++;
            }
            
            var observer = new ObserverBuilder(m_world)
                .WithComponent<TestObserverComponent>()
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();

            int trueEntityCount = 50;
            for (int i = 0; i < trueEntityCount; i++)
            {
                Entity entity = m_world.CreateEntity();
                entity.AddComponent<TestObserverComponent>();
            }

            Assert.True(matchCount == trueEntityCount);
        }
        
        [Test]
        public void Get_Count_Tag_One()
        {
            int matchCount = 0;
            void ObserverCallback(EcsIter ecsIter)
            {
                matchCount++;
            }
            
            var observer = new ObserverBuilder(m_world)
                .WithTag<TestObserverTag>()
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();
            
            Entity entity = m_world.CreateEntity();
            entity.AddTag<TestObserverTag>();
            
            Assert.True(matchCount == 1);
        }
        
        [Test]
        public void Get_Count_Tag_Fifty()
        {
            int matchCount = 0;
            void ObserverCallback(EcsIter ecsIter)
            {
                matchCount++;
            }
            
            var observer = new ObserverBuilder(m_world)
                .WithTag<TestObserverTag>()
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();

            int trueEntityCount = 50;
            for (int i = 0; i < trueEntityCount; i++)
            {
                Entity entity = m_world.CreateEntity();
                entity.AddTag<TestObserverTag>();
            }

            Assert.True(matchCount == trueEntityCount);
        }
        
        [Test]
        public void Get_Count_RuntimeTag_One()
        {
            RuntimeTag runtimeTag = new RuntimeTag("testTag");

            int matchCount = 0;
            void ObserverCallback(EcsIter ecsIter)
            {
                matchCount++;
            }
            
            var observer = new ObserverBuilder(m_world)
                .WithTag(runtimeTag)
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();

            Entity entity = m_world.CreateEntity();
            entity.AddTag(runtimeTag);

            Assert.True(matchCount == 1);
        }
        
        [Test]
        public void Get_Count_RuntimeTag_Fifty()
        {
            RuntimeTag runtimeTag = new RuntimeTag("testTag");

            int matchCount = 0;
            void ObserverCallback(EcsIter ecsIter)
            {
                matchCount++;
            }
            
            var observer = new ObserverBuilder(m_world)
                .WithTag(runtimeTag)
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();

            int trueEntityCount = 50;
            for (int i = 0; i < trueEntityCount; i++)
            {
                Entity entity = m_world.CreateEntity();
                entity.AddTag(runtimeTag);
            }

            Assert.True(matchCount == trueEntityCount);
        }
        
        [Test]
        public void Get_Count_ManagedComponent_One()
        {
            GameObject gameObject = new GameObject();
            
            int matchCount = 0;
            void ObserverCallback(EcsIter ecsIter)
            {
                matchCount++;
            }
            
            var observer = new ObserverBuilder(m_world)
                .WithManagedComponent<Transform>()
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();

            Entity entity = m_world.CreateEntity();
            entity.AddManagedComponent(gameObject.transform);

            Assert.True(matchCount == 1);
        }
        
        [Test]
        public void Get_Count_ManagedComponent_Fifty()
        {
            GameObject gameObject = new GameObject();

            int matchCount = 0;
            void ObserverCallback(EcsIter ecsIter)
            {
                matchCount++;
            }
            
            var observer = new ObserverBuilder(m_world)
                .WithManagedComponent<Transform>()
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();

            int trueEntityCount = 50;
            for (int i = 0; i < trueEntityCount; i++)
            {
                Entity entity = m_world.CreateEntity();
                entity.AddManagedComponent(gameObject.transform);
            }

            Assert.True(matchCount == trueEntityCount);
        }
        
        [Test]
        public void Get_Count_Pair_TagTag_One()
        {
            int matchCount = 0;
            void ObserverCallback(EcsIter ecsIter)
            {
                matchCount++;
            }
            
            var observer = new ObserverBuilder(m_world)
                .WithPair<TestObserverTag, TestObserverTag2>()
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();

            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestObserverTag, TestObserverTag2>();

            Assert.True(matchCount == 1);
        }
        
        [Test]
        public void Get_Count_Pair_TagTag_Fifty()
        {
            int matchCount = 0;
            void ObserverCallback(EcsIter ecsIter)
            {
                matchCount++;
            }
            
            var observer = new ObserverBuilder(m_world)
                .WithPair<TestObserverTag, TestObserverTag2>()
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();

            int trueEntityCount = 50;
            for (int i = 0; i < trueEntityCount; i++)
            {
                Entity entity = m_world.CreateEntity();
                entity.AddPair<TestObserverTag, TestObserverTag2>();
            }

            Assert.True(matchCount == trueEntityCount);
        }

        [Test]
        public void Get_Count_Pair_TagComponent_One()
        {
            int matchCount = 0;
            void ObserverCallback(EcsIter ecsIter)
            {
                matchCount++;
            }
            
            var observer = new ObserverBuilder(m_world)
                .WithPair<TestObserverTag, TestObserverComponent>()
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();

            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestObserverTag, TestObserverComponent>(new TestObserverComponent() { CoolBool = true });
            Assert.True(matchCount == 1);
        }
        
        [Test]
        public void Get_Count_Pair_TagComponent_Fifty()
        {
            int matchCount = 0;
            void ObserverCallback(EcsIter ecsIter)
            {
                matchCount++;
            }
            
            var observer = new ObserverBuilder(m_world)
                .WithPair<TestObserverTag, TestObserverComponent>()
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();

            int trueEntityCount = 50;
            for (int i = 0; i < trueEntityCount; i++)
            {
                Entity entity = m_world.CreateEntity();
                entity.AddPair<TestObserverTag, TestObserverComponent>(new TestObserverComponent() { CoolBool = true });
            }

            Assert.True(matchCount == trueEntityCount);
        }

        [Test]
        public void Get_Counter_Pair_TagEntity_One()
        {
            Entity entity2 = m_world.CreateEntity();
            
            int matchCount = 0;
            void ObserverCallback(EcsIter ecsIter)
            {
                matchCount++;
            }
            
            var observer = new ObserverBuilder(m_world)
                .WithPair<TestObserverTag>(entity2)
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();

            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestObserverTag>(entity2);
            Assert.True(matchCount == 1);
        }

        [Test]
        public void Get_Counter_Pair_TagEntity_Fifty()
        {
            Entity entity2 = m_world.CreateEntity();
            
            int matchCount = 0;
            void ObserverCallback(EcsIter ecsIter)
            {
                matchCount++;
            }
            
            var observer = new ObserverBuilder(m_world)
                .WithPair<TestObserverTag>(entity2)
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();

            int trueEntityCount = 50;
            for (int i = 0; i < trueEntityCount; i++)
            {
                Entity entity = m_world.CreateEntity();
                entity.AddPair<TestObserverTag>(entity2);
            }

            Assert.True(matchCount == trueEntityCount);
        }

        [Test]
        public void Get_Count_NoTerms()
        {
            int matchCount = 0;
            void ObserverCallback(EcsIter ecsIter)
            {
                matchCount++;
            }
            
            var observer = new ObserverBuilder(m_world)
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();
            
            Assert.True(matchCount == 0);
        }
        
        [Test]
        public void Get_Entity()
        {
            void ObserverCallback(EcsIter ecsIter)
            {
                Assert.True(ecsIter.GetEntity().Id != 0);
            }
            
            var observer = new ObserverBuilder(m_world)
                .WithComponent<TestObserverComponent>()
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();
            
            Entity entity = m_world.CreateEntity();
            entity.AddComponent<TestObserverComponent>();
        }

        [Test]
        public void Get_Component()
        {
            void ObserverCallback(EcsIter ecsIter)
            {
                while (ecsIter.MoveNext())
                {
                    ref TestObserverComponent testObserverComponent = ref ecsIter.GetComponent<TestObserverComponent>(0);
                    Assert.True(true);
                    if (testObserverComponent.CoolBool)
                    {
                        Assert.True(true);
                    }
                    else
                    {
                        Assert.True(false);
                    }
                }
            }
            
            var observer = new ObserverBuilder(m_world)
                .WithComponent<TestObserverComponent>()
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();
            
            Entity entity = m_world.CreateEntity();

            entity.AddComponent(new TestObserverComponent() { CoolBool = true });
        }
        
        [Test]
        public void Get_ManagedComponent()
        {
            GameObject gameObject = new GameObject();
            void ObserverCallback(EcsIter ecsIter)
            {
                while (ecsIter.MoveNext())
                {
                    Assert.True(ecsIter.GetManagedComponent<Transform>() != null);
                }
            }
            
            var observer = new ObserverBuilder(m_world)
                .WithManagedComponent<Transform>()
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();

            Entity entity = m_world.CreateEntity();
            entity.AddManagedComponent(gameObject.transform);
        }
        
        [Test]
        public void Get_Pair_SecondComponent()
        {
            void ObserverCallback(EcsIter ecsIter)
            {
                while (ecsIter.MoveNext())
                {
                    if (ecsIter.GetPairSecondAsComponent<TestObserverComponent>(0).CoolBool)
                    {
                        Assert.True(true);
                    }
                    else
                    {
                        Assert.True(false);
                    }
                }
            }
            
            var observer = new ObserverBuilder(m_world)
                .WithPair<TestObserverTag, TestObserverComponent>()
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();

            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestObserverTag, TestObserverComponent>(new TestObserverComponent() { CoolBool = true });
        }
        
        [Test]
        public void Get_Pair_SecondEntity()
        {
            Entity entity2 = m_world.CreateEntity();
            void ObserverCallback(EcsIter ecsIter)
            {
                while (ecsIter.MoveNext())
                {
                    Assert.True(ecsIter.GetPairSecondAsEntity(0).Equals(entity2));
                }
            }
            
            var observer = new ObserverBuilder(m_world)
                .WithPair<TestObserverTag>(entity2)
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();

            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestObserverTag>(entity2);
        }
        
        [Test]
        public void Pair_TypeIsCorrect_First()
        {
            void ObserverCallback(EcsIter ecsIter)
            {
                Assert.True(ecsIter.PairFirstIsType<TestObserverTag>(0));
            }
            
            var observer = new ObserverBuilder(m_world)
                .WithPair<TestObserverTag, TestObserverTag2>()
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();

            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestObserverTag, TestObserverTag2>();
        }
        
        [Test]
        public void Pair_TypeIsCorrect_Second_Tag()
        {
            void ObserverCallback(EcsIter ecsIter)
            {
                Assert.True(ecsIter.PairSecondIsType<TestObserverTag2>(0));
            }
            
            var observer = new ObserverBuilder(m_world)
                .WithPair<TestObserverTag, TestObserverTag2>()
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();

            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestObserverTag, TestObserverTag2>();
        }
        
        [Test]
        public void Pair_TypeIsCorrect_Second_Component()
        {
            void ObserverCallback(EcsIter ecsIter)
            {
                while (ecsIter.MoveNext())
                {
                    Assert.True(ecsIter.PairSecondIsType<TestObserverComponent>(0));
                }
            }
            
            var observer = new ObserverBuilder(m_world)
                .WithPair<TestObserverTag, TestObserverComponent>()
                .OnEvent(ObserverEvent.EcsOnAdd)
                .WithCallback(ObserverCallback)
                .Build();

            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestObserverTag, TestObserverComponent>(new TestObserverComponent() { CoolBool = true });
        }
    }
}