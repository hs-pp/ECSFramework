using NUnit.Framework;
using UnityEngine;

namespace EcsFramework.Tests
{
    public class BaseQueryBuilder_Tests : EcsUnitTests
    {
        private struct TestBuilderComponent : IComponent { }
        private struct TestBuilderTag : ITag { }
        private struct TestBuilderTag2 : ITag { }

        private class TestBuilder : BaseQueryBuilder<TestBuilder>
        {
            public TestBuilder(World world) : base(world) { }

            public flecs.ecs_filter_desc_t GetFilterDesc()
            {
                return m_filterDesc;
            }
        }
        
        [Test]
        public void AddTerm_Component_With()
        {
            flecs.ecs_filter_desc_t filterDesc = new TestBuilder(m_world)
                .WithComponent<TestBuilderComponent>()
                .GetFilterDesc();
            
            Assert.True(filterDesc.terms[0].id == m_world.GetComponentId<TestBuilderComponent>());
            Assert.True(filterDesc.terms[0].oper == flecs.ecs_oper_kind_t.EcsAnd);
        }
        
        [Test]
        public void AddTerm_Component_Without()
        {
            flecs.ecs_filter_desc_t filterDesc = new TestBuilder(m_world)
                .WithoutComponent<TestBuilderComponent>()
                .GetFilterDesc();
            
            Assert.True(filterDesc.terms[0].id == m_world.GetComponentId<TestBuilderComponent>());
            Assert.True(filterDesc.terms[0].oper == flecs.ecs_oper_kind_t.EcsNot);
        }
        
        [Test]
        public void AddTerm_Tag_With()
        {
            flecs.ecs_filter_desc_t filterDesc = new TestBuilder(m_world)
                .WithTag<TestBuilderTag>()
                .GetFilterDesc();
            
            Assert.True(filterDesc.terms[0].id == m_world.GetComponentId<TestBuilderTag>());
            Assert.True(filterDesc.terms[0].oper == flecs.ecs_oper_kind_t.EcsAnd);
        }
        
        [Test]
        public void AddTerm_Tag_Without()
        {
            flecs.ecs_filter_desc_t filterDesc = new TestBuilder(m_world)
                .WithoutTag<TestBuilderTag>()
                .GetFilterDesc();
            
            Assert.True(filterDesc.terms[0].id == m_world.GetComponentId<TestBuilderTag>());
            Assert.True(filterDesc.terms[0].oper == flecs.ecs_oper_kind_t.EcsNot);
        }
        
        [Test]
        public void AddTerm_RuntimeTag_With()
        {
            RuntimeTag runtimeTag = new RuntimeTag("TestBuilderRuntimeTag");
            
            flecs.ecs_filter_desc_t filterDesc = new TestBuilder(m_world)
                .WithTag(runtimeTag)
                .GetFilterDesc();
            
            Assert.True(filterDesc.terms[0].id == m_world.GetRuntimeTagId(runtimeTag));
            Assert.True(filterDesc.terms[0].oper == flecs.ecs_oper_kind_t.EcsAnd);
        }
        
        [Test]
        public void AddTerm_RuntimeTag_Without()
        {
            RuntimeTag runtimeTag = new RuntimeTag("TestBuilderRuntimeTag");
            
            flecs.ecs_filter_desc_t filterDesc = new TestBuilder(m_world)
                .WithoutTag(runtimeTag)
                .GetFilterDesc();
            
            Assert.True(filterDesc.terms[0].id == m_world.GetRuntimeTagId(runtimeTag));
            Assert.True(filterDesc.terms[0].oper == flecs.ecs_oper_kind_t.EcsNot);
        }
        
        [Test]
        public void AddTerm_Pair_TagTag_With()
        {
            flecs.ecs_filter_desc_t filterDesc = new TestBuilder(m_world)
                .WithPair<TestBuilderTag, TestBuilderTag2>()
                .GetFilterDesc();
            
            Assert.True(filterDesc.terms[0].id == m_world.GetPairId<TestBuilderTag, TestBuilderTag2>());
            Assert.True(filterDesc.terms[0].oper == flecs.ecs_oper_kind_t.EcsAnd);
        }
        
        [Test]
        public void AddTerm_Pair_TagTag_Without()
        {
            flecs.ecs_filter_desc_t filterDesc = new TestBuilder(m_world)
                .WithoutPair<TestBuilderTag, TestBuilderTag2>()
                .GetFilterDesc();
            
            Assert.True(filterDesc.terms[0].id == m_world.GetPairId<TestBuilderTag, TestBuilderTag2>());
            Assert.True(filterDesc.terms[0].oper == flecs.ecs_oper_kind_t.EcsNot);
        }
        
        [Test]
        public void AddTerm_Pair_TagComponent_With()
        {
            flecs.ecs_filter_desc_t filterDesc = new TestBuilder(m_world)
                .WithPair<TestBuilderTag, TestBuilderComponent>()
                .GetFilterDesc();
            
            Assert.True(filterDesc.terms[0].id == m_world.GetPairId<TestBuilderTag, TestBuilderComponent>());
            Assert.True(filterDesc.terms[0].oper == flecs.ecs_oper_kind_t.EcsAnd);
        }
        
        [Test]
        public void AddTerm_Pair_TagComponent_Without()
        {
            flecs.ecs_filter_desc_t filterDesc = new TestBuilder(m_world)
                .WithoutPair<TestBuilderTag, TestBuilderComponent>()
                .GetFilterDesc();
            
            Assert.True(filterDesc.terms[0].id == m_world.GetPairId<TestBuilderTag, TestBuilderComponent>());
            Assert.True(filterDesc.terms[0].oper == flecs.ecs_oper_kind_t.EcsNot);
        }
        
        [Test]
        public void AddTerm_Pair_TagEntity_With()
        {
            Entity entity = new Entity();
            
            flecs.ecs_filter_desc_t filterDesc = new TestBuilder(m_world)
                .WithPair<TestBuilderTag>(entity)
                .GetFilterDesc();
            
            Assert.True(filterDesc.terms[0].id == m_world.GetPairId(m_world.GetComponentId<TestBuilderTag>(), entity.Id));
            Assert.True(filterDesc.terms[0].oper == flecs.ecs_oper_kind_t.EcsAnd);
        }
        
        [Test]
        public void AddTerm_Pair_TagEntity_Without()
        {
            Entity entity = new Entity();
            
            flecs.ecs_filter_desc_t filterDesc = new TestBuilder(m_world)
                .WithoutPair<TestBuilderTag>(entity)
                .GetFilterDesc();
            
            Assert.True(filterDesc.terms[0].id == m_world.GetPairId(m_world.GetComponentId<TestBuilderTag>(), entity.Id));
            Assert.True(filterDesc.terms[0].oper == flecs.ecs_oper_kind_t.EcsNot);
        }
        
        [Test]
        public void AddTerm_ManagedComponent_With()
        {
            flecs.ecs_filter_desc_t filterDesc = new TestBuilder(m_world)
                .WithManagedComponent<Transform>()
                .GetFilterDesc();
            
            Assert.True(filterDesc.terms[0].id == m_world.GetManagedComponentId(typeof(Transform)));
            Assert.True(filterDesc.terms[0].oper == flecs.ecs_oper_kind_t.EcsAnd);
        }
        
        [Test]
        public void AddTerm_ManagedComponent_Without()
        {
            flecs.ecs_filter_desc_t filterDesc = new TestBuilder(m_world)
                .WithoutManagedComponent<Transform>()
                .GetFilterDesc();
            
            Assert.True(filterDesc.terms[0].id == m_world.GetManagedComponentId(typeof(Transform)));
            Assert.True(filterDesc.terms[0].oper == flecs.ecs_oper_kind_t.EcsNot);
        }
    }
}