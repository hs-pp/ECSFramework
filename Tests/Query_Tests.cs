using NUnit.Framework;
using UnityEngine;

namespace EcsFramework.Tests
{
    public class Query_Tests : EcsUnitTests
    {
        private struct TestQueryTag : ITag { }
        private struct TestQueryTag2 : ITag { }
        private struct TestQueryComponent : IComponent
        {
            public bool CoolBool;
        }

         [Test]
        public void Get_Count_Component_One()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddComponent<TestQueryComponent>();

            Query query = new QueryBuilder(m_world)
                .WithComponent<TestQueryComponent>()
                .Build();
            
            Assert.True(query.GetCount() == 1);
        }
        
        [Test]
        public void Get_Count_Component_Fifty()
        {
            int trueEntityCount = 50;
            for (int i = 0; i < trueEntityCount; i++)
            {
                Entity entity = m_world.CreateEntity();
                entity.AddComponent<TestQueryComponent>();
            }

            Query query = new QueryBuilder(m_world)
                .WithComponent<TestQueryComponent>()
                .Build();
            
            Assert.True(query.GetCount() == trueEntityCount);
        }
        
        [Test]
        public void Get_Count_Tag_One()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddTag<TestQueryTag>();

            Query query = new QueryBuilder(m_world)
                .WithTag<TestQueryTag>()
                .Build();
            
            Assert.True(query.GetCount() == 1);
        }
        
        [Test]
        public void Get_Count_Tag_Fifty()
        {
            int trueEntityCount = 50;
            for (int i = 0; i < trueEntityCount; i++)
            {
                Entity entity = m_world.CreateEntity();
                entity.AddTag<TestQueryTag>();
            }

            Query query = new QueryBuilder(m_world)
                .WithTag<TestQueryTag>()
                .Build();
            
            Assert.True(query.GetCount() == trueEntityCount);
        }
        
        [Test]
        public void Get_Count_RuntimeTag_One()
        {
            RuntimeTag runtimeTag = new RuntimeTag("testTag");
            
            Entity entity = m_world.CreateEntity();
            entity.AddTag(runtimeTag);

            Query query = new QueryBuilder(m_world)
                .WithTag(runtimeTag)
                .Build();
            
            Assert.True(query.GetCount() == 1);
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

            Query query = new QueryBuilder(m_world)
                .WithTag(runtimeTag)
                .Build();
            
            Assert.True(query.GetCount() == trueEntityCount);
        }

        [Test]
        public void Get_Count_ManagedComponent_One()
        {
            GameObject gameObject = new GameObject();

            Entity entity = m_world.CreateEntity();
            entity.AddManagedComponent<Transform>(gameObject.transform);
            
            Query query = new QueryBuilder(m_world)
                .WithManagedComponent<Transform>()
                .Build();

            Assert.True(query.GetCount() == 1);
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
            
            Query query = new QueryBuilder(m_world)
                .WithManagedComponent<Transform>()
                .Build();

            Assert.True(query.GetCount() == trueEntityCount);
        }

        [Test]
        public void Get_Count_Pair_TagTag_One()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestQueryTag, TestQueryTag2>();

            Query query = new QueryBuilder(m_world)
                .WithPair<TestQueryTag, TestQueryTag2>()
                .Build();
            
            Assert.True(query.GetCount() == 1);
        }
        
        [Test]
        public void Get_Count_Pair_TagTag_Fifty()
        {
            int trueEntityCount = 50;
            for (int i = 0; i < trueEntityCount; i++)
            {
                Entity entity = m_world.CreateEntity();
                entity.AddPair<TestQueryTag, TestQueryTag2>();
            }

            Query query = new QueryBuilder(m_world)
                .WithPair<TestQueryTag, TestQueryTag2>()
                .Build();
            
            Assert.True(query.GetCount() == trueEntityCount);
        }
        
        [Test]
        public void Get_Count_Pair_TagComponent_One()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestQueryTag, TestQueryComponent>(new TestQueryComponent() { CoolBool = true });

            Query query = new QueryBuilder(m_world)
                .WithPair<TestQueryTag, TestQueryComponent>()
                .Build();
            
            Assert.True(query.GetCount() == 1);
        }
        
        [Test]
        public void Get_Count_Pair_TagComponent_Fifty()
        {
            int trueEntityCount = 50;
            for (int i = 0; i < trueEntityCount; i++)
            {
                Entity entity = m_world.CreateEntity();
                entity.AddPair<TestQueryTag, TestQueryComponent>(new TestQueryComponent() { CoolBool = true });
            }

            Query query = new QueryBuilder(m_world)
                .WithPair<TestQueryTag, TestQueryComponent>()
                .Build();
            
            Assert.True(query.GetCount() == trueEntityCount);
        }

        [Test]
        public void Get_Count_Pair_TagEntity_One()
        {
            Entity entity = m_world.CreateEntity();
            
            Entity entity2 = m_world.CreateEntity();
            entity2.AddPair<TestQueryTag>(entity);

            Query query = new QueryBuilder(m_world)
                .WithPair<TestQueryTag>(entity)
                .Build();

            Assert.True(query.GetCount() == 1);
        }
        
        [Test]
        public void Get_Count_Pair_TagEntity_Fifty()
        {
            Entity entity2 = m_world.CreateEntity();
            
            int trueEntityCount = 50;
            for (int i = 0; i < trueEntityCount; i++)
            {
                Entity entity = m_world.CreateEntity();
                entity.AddPair<TestQueryTag>(entity2);
            }

            Query query = new QueryBuilder(m_world)
                .WithPair<TestQueryTag>(entity2)
                .Build();
            
            Assert.True(query.GetCount() == trueEntityCount);
        }
        
        [Test]
        public void Get_Count_NoTerms()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddComponent<TestQueryComponent>();

            Query query = new QueryBuilder(m_world)
                .Build();

            Assert.True(query.GetCount() == 0);
        }

        [Test]
        public void Get_Entity()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddComponent<TestQueryComponent>();

            Query query = new QueryBuilder(m_world)
                .WithComponent<TestQueryComponent>()
                .Build();

            var iter = query.GetIter();
            iter.MoveNext();
            
            Assert.True(iter.GetEntity().Equals(entity));
        }

        [Test]
        public void Get_Component()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddComponent(new TestQueryComponent() { CoolBool = true });

            Query query = new QueryBuilder(m_world)
                .WithComponent<TestQueryComponent>()
                .Build();

            var iter = query.GetIter();
            iter.MoveNext();

            ref TestQueryComponent TestQueryComponent = ref iter.GetComponent<TestQueryComponent>(0);
            Assert.True(TestQueryComponent.CoolBool);
        }
        
        [Test]
        public void Get_ManagedComponent()
        {
            Entity entity = m_world.CreateEntity();
            GameObject gameObject = new GameObject();
            
            entity.AddManagedComponent<Transform>(gameObject.transform);

            Query query = new QueryBuilder(m_world)
                .WithManagedComponent<Transform>()
                .Build();

            var iter = query.GetIter();
            iter.MoveNext();

            Transform transform = iter.GetManagedComponent<Transform>();
            Assert.True(transform == gameObject.transform);
        }
        
        [Test]
        public void Get_Pair_SecondComponent()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestQueryTag, TestQueryComponent>(new TestQueryComponent() { CoolBool = true });

            Query query = new QueryBuilder(m_world)
                .WithPair<TestQueryTag, TestQueryComponent>()
                .Build();

            var iter = query.GetIter();
            iter.MoveNext();

            ref TestQueryComponent TestQueryComponent = ref iter.GetPairSecondAsComponent<TestQueryComponent>(0);
            Assert.True(TestQueryComponent.CoolBool);
        }
        
        [Test]
        public void Get_Pair_SecondEntity()
        {
            Entity entity = m_world.CreateEntity();
            Entity entity2 = m_world.CreateEntity();
            entity.AddPair<TestQueryTag>(entity2);

            Query query = new QueryBuilder(m_world)
                .WithPair<TestQueryTag>(entity2)
                .Build();

            var iter = query.GetIter();
            iter.MoveNext();

            Assert.True(iter.GetPairSecondAsEntity(0).Equals(entity2));
        }
        
        [Test]
        public void Pair_TypeIsCorrect_First()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestQueryTag, TestQueryTag2>();

            Query query = new QueryBuilder(m_world)
                .WithPair<TestQueryTag, TestQueryTag2>()
                .Build();

            var iter = query.GetIter();
            iter.MoveNext();
            
            Assert.True(iter.PairFirstIsType<TestQueryTag>(0));
            Assert.False(iter.PairFirstIsType<TestQueryTag2>(0));
        }
        
        [Test]
        public void Pair_TypeIsCorrect_Second_Tag()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestQueryTag, TestQueryTag2>();

            Query query = new QueryBuilder(m_world)
                .WithPair<TestQueryTag, TestQueryTag2>()
                .Build();

            var iter = query.GetIter();
            iter.MoveNext();
            
            Assert.False(iter.PairSecondIsType<TestQueryTag>(0));
            Assert.True(iter.PairSecondIsType<TestQueryTag2>(0));
        }
        
        [Test]
        public void Pair_TypeIsCorrect_Second_Component()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestQueryTag, TestQueryComponent>(new TestQueryComponent() { CoolBool = true });

            Query query = new QueryBuilder(m_world)
                .WithPair<TestQueryTag, TestQueryComponent>()
                .Build();

            var iter = query.GetIter();
            iter.MoveNext();
            
            Assert.False(iter.PairSecondIsType<TestQueryTag>(0));
            Assert.True(iter.PairSecondIsType<TestQueryComponent>(0));
        }
    }
}
