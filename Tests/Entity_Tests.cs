using NUnit.Framework;
using UnityEngine;

namespace EcsFramework.Tests
{
    public class Entity_Tests : EcsUnitTests
    {
        private struct TestEntityComponent : IComponent
        {
            public bool coolBool;
        }
        private struct TestEntityTag : ITag { }
        private struct TestEntityTag2 : ITag { }
        
        [Test]
        public void IsAlive_AfterCreate()
        {
            Entity entity = m_world.CreateEntity();
            Assert.True(entity.IsAlive());
        }

        [Test]
        public void IsAlive_AfterDelete()
        {
            Entity entity = m_world.CreateEntity();
            m_world.DeleteEntity(entity);
            Assert.False(entity.IsAlive());
        }

        [Test]
        public void Component_Has_True()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddComponent<TestEntityComponent>();
            Assert.True(entity.HasComponent<TestEntityComponent>());
        }

        [Test]
        public void Component_Has_False()
        {
            Entity entity = m_world.CreateEntity();
            Assert.False(entity.HasComponent<TestEntityComponent>());
        }
        
        [Test]
        public void Component_Has_AddRemove()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddComponent<TestEntityComponent>();
            entity.RemoveComponent<TestEntityComponent>();

            Assert.False(entity.HasComponent<TestEntityComponent>());
        }
        
        [Test]
        public void Component_AddGet()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddComponent(new TestEntityComponent() { coolBool = true });
            ref TestEntityComponent testEntityComponent = ref entity.GetComponent<TestEntityComponent>();
            Assert.True(testEntityComponent.coolBool);
        }

        [Test]
        public void Component_ModifiedPersists()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddComponent(new TestEntityComponent() { coolBool = true });
            ref TestEntityComponent testEntityComponent = ref entity.GetComponent<TestEntityComponent>();
            Assert.True(testEntityComponent.coolBool);

            testEntityComponent.coolBool = false;
            entity.MarkComponentModified<TestEntityComponent>();
            Assert.False(testEntityComponent.coolBool);
        }

        [Test]
        public void RuntimeTag_Has_True()
        {
            RuntimeTag runtimeTag = new RuntimeTag("TestRuntimeTag");
            
            Entity entity = m_world.CreateEntity();
            entity.AddTag(runtimeTag);
            Assert.True(entity.HasTag(runtimeTag));
        }

        [Test]
        public void RuntimeTag_Has_False()
        {
            RuntimeTag runtimeTag = new RuntimeTag("TestRuntimeTag");
            
            Entity entity = m_world.CreateEntity();
            Assert.False(entity.HasTag(runtimeTag));
        }
        
        [Test]
        public void RuntimeTag_Has_AddRemove()
        {
            RuntimeTag runtimeTag = new RuntimeTag("TestRuntimeTag");
            
            Entity entity = m_world.CreateEntity();
            entity.AddTag(runtimeTag);
            Assert.True(entity.HasTag(runtimeTag));
            
            entity.RemoveTag(runtimeTag);
            Assert.False(entity.HasTag(runtimeTag));
        }

        [Test]
        public void ManagedComponent_Has_True()
        {
            Entity entity = m_world.CreateEntity();

            GameObject gameobject = new GameObject();
            entity.AddManagedComponent(gameobject.transform);

            Assert.True(entity.HasManagedComponent<Transform>());
        }

        [Test]
        public void ManagedComponent_Has_False()
        {
            Entity entity = m_world.CreateEntity();

            Assert.False(entity.HasManagedComponent<Transform>());
        }

        [Test]
        public void ManagedComponent_Has_AddRemove()
        {
            Entity entity = m_world.CreateEntity();

            GameObject gameobject = new GameObject();
            entity.AddManagedComponent(gameobject.transform);
            Assert.True(entity.HasManagedComponent<Transform>());
            
            entity.RemoveManagedComponent<Transform>();
            Assert.False(entity.HasManagedComponent<Transform>());
        }
        
        [Test]
        public void ManagedComponent_AddGet()
        {
            Entity entity = m_world.CreateEntity();

            GameObject gameobject = new GameObject();
            entity.AddManagedComponent(gameobject.transform);

            Transform transform = entity.GetManagedComponent<Transform>();
            Assert.True(transform == gameobject.transform);
        }
        
        [Test]
        public void ManagedComponent_AddRemoveGet()
        {
            Entity entity = m_world.CreateEntity();

            GameObject gameobject = new GameObject();
            entity.AddManagedComponent(gameobject.transform);
            entity.RemoveManagedComponent<Transform>();
            
            Transform transform = entity.GetManagedComponent<Transform>();
            Assert.True(transform == null);
        }

        [Test]
        public void Pair_TagTag_Has_True()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestEntityTag, TestEntityTag2>();
            Assert.True(entity.HasPair<TestEntityTag, TestEntityTag2>());
        }

        [Test]
        public void Pair_TagTag_Has_False()
        {
            Entity entity = m_world.CreateEntity();
            Assert.False(entity.HasPair<TestEntityTag, TestEntityTag2>());
        }

        [Test]
        public void Pair_TagComponent_Has_True()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestEntityTag, TestEntityComponent>(new TestEntityComponent() { coolBool = true });
            Assert.True(entity.HasPair<TestEntityTag, TestEntityComponent>());
        }

        [Test]
        public void Pair_TagComponent_Has_False()
        {
            Entity entity = m_world.CreateEntity();
            Assert.False(entity.HasPair<TestEntityTag, TestEntityComponent>());
        }
        
        [Test]
        public void Pair_TagComponent_Has_AddRemove()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestEntityTag, TestEntityComponent>(new TestEntityComponent() { coolBool = true });
            Assert.True(entity.HasPair<TestEntityTag, TestEntityComponent>());

            entity.RemovePair<TestEntityTag, TestEntityComponent>();
            Assert.False(entity.HasPair<TestEntityTag, TestEntityComponent>());
        }
        
        [Test]
        public void Pair_TagComponent_AddGet()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestEntityTag, TestEntityComponent>(new TestEntityComponent() { coolBool = true });

            ref TestEntityComponent component = ref entity.GetPair<TestEntityTag, TestEntityComponent>();
            Assert.True(component.coolBool);
        }

        [Test]
        public void Pair_TagComponent_AddSetGet()
        {
            Entity entity = m_world.CreateEntity();
            entity.AddPair<TestEntityTag, TestEntityComponent>(new TestEntityComponent() { coolBool = true });
            
            ref TestEntityComponent gotComponent = ref entity.GetPair<TestEntityTag, TestEntityComponent>();
            Assert.True(gotComponent.coolBool);
            gotComponent.coolBool = false;
            entity.MarkPairModified<TestEntityTag, TestEntityComponent>();
            
            ref TestEntityComponent gotComponentAgain = ref entity.GetPair<TestEntityTag, TestEntityComponent>();
            Assert.False(gotComponentAgain.coolBool);
        }

        [Test]
        public void Pair_TagEntity_Has_True()
        {
            Entity entity = m_world.CreateEntity();
            Entity entity2 = m_world.CreateEntity();
            entity.AddPair<TestEntityTag>(entity2);
            Assert.True(entity.HasPair<TestEntityTag>(entity2));
        }

        [Test]
        public void Pair_TagEntity_Has_False()
        {
            Entity entity = m_world.CreateEntity();
            Entity entity2 = m_world.CreateEntity();
            Assert.False(entity.HasPair<TestEntityTag>(entity2));
        }
        
        [Test]
        public void Pair_TagEntity_Has_AddRemove()
        {
            Entity entity = m_world.CreateEntity();
            Entity entity2 = m_world.CreateEntity();
            entity.AddPair<TestEntityTag>(entity2);
            Assert.True(entity.HasPair<TestEntityTag>(entity2));

            entity.RemovePair<TestEntityTag>(entity2);
            Assert.False(entity.HasPair<TestEntityTag>(entity2));
        }
    }
}