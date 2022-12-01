using NUnit.Framework;
using UnityEngine;

namespace EcsFramework.Tests
{
    public class Filter_Tests : EcsUnitTests
    {
        private struct TestFilterTag : ITag { }
        private struct TestFilterTag2 : ITag { }
        private struct TestFilterComponent : IComponent
        {
            public bool CoolBool;
        }

        [Test]
        public void Get_Count_Component_One()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddComponent<TestFilterComponent>();

            Filter filter = new FilterBuilder(m_world)
                .WithComponent<TestFilterComponent>()
                .Build();
            
            Assert.True(filter.GetCount() == 1);
        }
        
        [Test]
        public void Get_Count_Component_Fifty()
        {
            int trueEntityCount = 50;
            for (int i = 0; i < trueEntityCount; i++)
            {
                Entity entity = m_world.CreateEntity();
                entity.AddComponent<TestFilterComponent>();
            }

            Filter filter = new FilterBuilder(m_world)
                .WithComponent<TestFilterComponent>()
                .Build();
            
            Assert.True(filter.GetCount() == trueEntityCount);
        }
        
        [Test]
        public void Get_Count_Tag_One()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddTag<TestFilterTag>();

            Filter filter = new FilterBuilder(m_world)
                .WithTag<TestFilterTag>()
                .Build();
            
            Assert.True(filter.GetCount() == 1);
        }
        
        [Test]
        public void Get_Count_Tag_Fifty()
        {
            int trueEntityCount = 50;
            for (int i = 0; i < trueEntityCount; i++)
            {
                Entity entity = m_world.CreateEntity();
                entity.AddTag<TestFilterTag>();
            }

            Filter filter = new FilterBuilder(m_world)
                .WithTag<TestFilterTag>()
                .Build();
            
            Assert.True(filter.GetCount() == trueEntityCount);
        }
        
        [Test]
        public void Get_Count_RuntimeTag_One()
        {
            RuntimeTag runtimeTag = new RuntimeTag("testTag");
            
            Entity entity = m_world.CreateEntity();
            entity.AddTag(runtimeTag);

            Filter filter = new FilterBuilder(m_world)
                .WithTag(runtimeTag)
                .Build();
            
            Assert.True(filter.GetCount() == 1);
        }
        
        [Test]
        public void Get_Count_RuntimeTag_Fifty()
        {
            RuntimeTag runtimeTag = new RuntimeTag("testTag");

            int trueEntityCount = 50;
            for (int i = 0; i < trueEntityCount; i++)
            {
                Entity entity = m_world.CreateEntity();
                entity.AddTag(runtimeTag);
            }

            Filter filter = new FilterBuilder(m_world)
                .WithTag(runtimeTag)
                .Build();
            
            Assert.True(filter.GetCount() == trueEntityCount);
        }

        [Test]
        public void Get_Count_ManagedComponent_One()
        {
            GameObject gameObject = new GameObject();

            Entity entity = m_world.CreateEntity();
            entity.AddManagedComponent<Transform>(gameObject.transform);
            
            Filter filter = new FilterBuilder(m_world)
                .WithManagedComponent<Transform>()
                .Build();

            Assert.True(filter.GetCount() == 1);
        }
        
        [Test]
        public void Get_Count_ManagedComponent_Fifty()
        {
            GameObject gameObject = new GameObject();

            int trueEntityCount = 50;
            for (int i = 0; i < trueEntityCount; i++)
            {
                Entity entity = m_world.CreateEntity();
                entity.AddManagedComponent<Transform>(gameObject.transform);
            }
            
            Filter filter = new FilterBuilder(m_world)
                .WithManagedComponent<Transform>()
                .Build();

            Assert.True(filter.GetCount() == trueEntityCount);
        }

        [Test]
        public void Get_Count_Pair_TagTag_One()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestFilterTag, TestFilterTag2>();

            Filter filter = new FilterBuilder(m_world)
                .WithPair<TestFilterTag, TestFilterTag2>()
                .Build();
            
            Assert.True(filter.GetCount() == 1);
        }
        
        [Test]
        public void Get_Count_Pair_TagTag_Fifty()
        {
            int trueEntityCount = 50;
            for (int i = 0; i < trueEntityCount; i++)
            {
                Entity entity = m_world.CreateEntity();
                entity.AddPair<TestFilterTag, TestFilterTag2>();
            }

            Filter filter = new FilterBuilder(m_world)
                .WithPair<TestFilterTag, TestFilterTag2>()
                .Build();
            
            Assert.True(filter.GetCount() == trueEntityCount);
        }
        
        [Test]
        public void Get_Count_Pair_TagComponent_One()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestFilterTag, TestFilterComponent>(new TestFilterComponent() { CoolBool = true });

            Filter filter = new FilterBuilder(m_world)
                .WithPair<TestFilterTag, TestFilterComponent>()
                .Build();
            
            Assert.True(filter.GetCount() == 1);
        }
        
        [Test]
        public void Get_Count_Pair_TagComponent_Fifty()
        {
            int trueEntityCount = 50;
            for (int i = 0; i < trueEntityCount; i++)
            {
                Entity entity = m_world.CreateEntity();
                entity.AddPair<TestFilterTag, TestFilterComponent>(new TestFilterComponent() { CoolBool = true });
            }

            Filter filter = new FilterBuilder(m_world)
                .WithPair<TestFilterTag, TestFilterComponent>()
                .Build();
            
            Assert.True(filter.GetCount() == trueEntityCount);
        }

        [Test]
        public void Get_Count_Pair_TagEntity_One()
        {
            Entity entity = m_world.CreateEntity();
            
            Entity entity2 = m_world.CreateEntity();
            entity2.AddPair<TestFilterTag>(entity);

            Filter filter = new FilterBuilder(m_world)
                .WithPair<TestFilterTag>(entity)
                .Build();

            Assert.True(filter.GetCount() == 1);
        }
        
        [Test]
        public void Get_Count_Pair_TagEntity_Fifty()
        {
            Entity entity2 = m_world.CreateEntity();
            
            int trueEntityCount = 50;
            for (int i = 0; i < trueEntityCount; i++)
            {
                Entity entity = m_world.CreateEntity();
                entity.AddPair<TestFilterTag>(entity2);
            }

            Filter filter = new FilterBuilder(m_world)
                .WithPair<TestFilterTag>(entity2)
                .Build();
            
            Assert.True(filter.GetCount() == trueEntityCount);
        }
        
        [Test]
        public void Get_Count_NoTerms()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddComponent<TestFilterComponent>();

            Filter filter = new FilterBuilder(m_world)
                .Build();

            Assert.True(filter.GetCount() == 0);
        }

        [Test]
        public void Get_Entity()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddComponent<TestFilterComponent>();

            Filter filter = new FilterBuilder(m_world)
                .WithComponent<TestFilterComponent>()
                .Build();

            var iter = filter.GetIter();
            iter.MoveNext();
            
            Assert.True(iter.GetEntity().Equals(entity));
        }

        [Test]
        public void Get_Component()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddComponent(new TestFilterComponent() { CoolBool = true });

            Filter filter = new FilterBuilder(m_world)
                .WithComponent<TestFilterComponent>()
                .Build();

            var iter = filter.GetIter();
            iter.MoveNext();

            ref TestFilterComponent testFilterComponent = ref iter.GetComponent<TestFilterComponent>(0);
            Assert.True(testFilterComponent.CoolBool);
        }
        
        [Test]
        public void Get_ManagedComponent()
        {
            Entity entity = m_world.CreateEntity();
            GameObject gameObject = new GameObject();
            
            entity.AddManagedComponent<Transform>(gameObject.transform);

            Filter filter = new FilterBuilder(m_world)
                .WithManagedComponent<Transform>()
                .Build();

            var iter = filter.GetIter();
            iter.MoveNext();

            Transform transform = iter.GetManagedComponent<Transform>();
            Assert.True(transform == gameObject.transform);
        }
        
        [Test]
        public void Get_Pair_SecondComponent()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestFilterTag, TestFilterComponent>(new TestFilterComponent() { CoolBool = true });

            Filter filter = new FilterBuilder(m_world)
                .WithPair<TestFilterTag, TestFilterComponent>()
                .Build();

            var iter = filter.GetIter();
            iter.MoveNext();

            ref TestFilterComponent testFilterComponent = ref iter.GetPairSecondAsComponent<TestFilterComponent>(0);
            Assert.True(testFilterComponent.CoolBool);
        }
        
        [Test]
        public void Get_Pair_SecondEntity()
        {
            Entity entity = m_world.CreateEntity();
            Entity entity2 = m_world.CreateEntity();
            entity.AddPair<TestFilterTag>(entity2);

            Filter filter = new FilterBuilder(m_world)
                .WithPair<TestFilterTag>(entity2)
                .Build();

            var iter = filter.GetIter();
            iter.MoveNext();

            Assert.True(iter.GetPairSecondAsEntity(0).Equals(entity2));
        }
        
        [Test]
        public void Pair_TypeIsCorrect_First()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestFilterTag, TestFilterTag2>();

            Filter filter = new FilterBuilder(m_world)
                .WithPair<TestFilterTag, TestFilterTag2>()
                .Build();

            var iter = filter.GetIter();
            iter.MoveNext();
            
            Assert.True(iter.PairFirstIsType<TestFilterTag>(0));
            Assert.False(iter.PairFirstIsType<TestFilterTag2>(0));
        }
        
        [Test]
        public void Pair_TypeIsCorrect_Second_Tag()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestFilterTag, TestFilterTag2>();

            Filter filter = new FilterBuilder(m_world)
                .WithPair<TestFilterTag, TestFilterTag2>()
                .Build();

            var iter = filter.GetIter();
            iter.MoveNext();
            
            Assert.False(iter.PairSecondIsType<TestFilterTag>(0));
            Assert.True(iter.PairSecondIsType<TestFilterTag2>(0));
        }
        
        [Test]
        public void Pair_TypeIsCorrect_Second_Component()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestFilterTag, TestFilterComponent>(new TestFilterComponent() { CoolBool = true });

            Filter filter = new FilterBuilder(m_world)
                .WithPair<TestFilterTag, TestFilterComponent>()
                .Build();

            var iter = filter.GetIter();
            iter.MoveNext();
            
            Assert.False(iter.PairSecondIsType<TestFilterTag>(0));
            Assert.True(iter.PairSecondIsType<TestFilterComponent>(0));
        }
    }
}