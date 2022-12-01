namespace EcsFramework
{
    public abstract class BaseQueryBuilder<TBuilder> where TBuilder : BaseQueryBuilder<TBuilder>
    {
        protected World m_world;
        protected flecs.ecs_filter_desc_t m_filterDesc = new();
        private int m_termCount = 0;

        protected BaseQueryBuilder(World world)
        {
            m_world = world;
        }
        
        public TBuilder WithComponent<T>() where T : unmanaged, IComponent
        {
            AddTerm<T>(flecs.ecs_oper_kind_t.EcsAnd);
            return (TBuilder)this;
        }
        public TBuilder WithoutComponent<T>() where T : unmanaged, IComponent
        {
            AddTerm<T>(flecs.ecs_oper_kind_t.EcsNot);
            return (TBuilder)this;
        }
        
        public TBuilder WithTag<T>() where T : unmanaged, ITag
        {
            AddTerm<T>(flecs.ecs_oper_kind_t.EcsAnd);
            return (TBuilder)this;
        }
        public TBuilder WithoutTag<T>() where T : unmanaged, ITag
        {
            AddTerm<T>(flecs.ecs_oper_kind_t.EcsNot);
            return (TBuilder)this;
        }
        public TBuilder WithTag(RuntimeTag runtimeTag)
        {
            AddRuntimeTagTerm(runtimeTag, flecs.ecs_oper_kind_t.EcsAnd);
            return (TBuilder)this;
        }
        public TBuilder WithoutTag(RuntimeTag runtimeTag)
        {
            AddRuntimeTagTerm(runtimeTag, flecs.ecs_oper_kind_t.EcsNot);
            return (TBuilder)this;
        }

        public TBuilder WithPair<TTag,TComponent>()
            where TTag : unmanaged, ITag
            where TComponent : unmanaged, IEcsComponent
        {
            AddPairTerm<TTag, TComponent>(flecs.ecs_oper_kind_t.EcsAnd);
            return (TBuilder)this;
        }
        public TBuilder WithoutPair<TTag,TComponent>()
            where TTag : unmanaged, ITag
            where TComponent : unmanaged, IEcsComponent
        {
            AddPairTerm<TTag, TComponent>(flecs.ecs_oper_kind_t.EcsNot);
            return (TBuilder)this;
        }
        public TBuilder WithPair<TTag>(Entity entity)
            where TTag : unmanaged, ITag
        {
            AddPairTerm<TTag>(entity, flecs.ecs_oper_kind_t.EcsAnd);
            return (TBuilder)this;
        }
        public TBuilder WithoutPair<TTag>(Entity entity)
            where TTag : unmanaged, ITag
        {
            AddPairTerm<TTag>(entity, flecs.ecs_oper_kind_t.EcsNot);
            return (TBuilder)this;
        }

        public TBuilder WithManagedComponent<T>()
        {
            AddManagedTerm<T>(flecs.ecs_oper_kind_t.EcsAnd);
            return (TBuilder)this;
        }
        public TBuilder WithoutManagedComponent<T>()
        {
            AddManagedTerm<T>(flecs.ecs_oper_kind_t.EcsNot);
            return (TBuilder)this;
        }

        private void AddTerm<T>(flecs.ecs_oper_kind_t oper) where T : unmanaged, IEcsComponent
        {
            m_filterDesc.terms[m_termCount++] = new flecs.ecs_term_t()
            {
                id = m_world.GetComponentId<T>(),
                oper = oper,
            };
        }
        private void AddRuntimeTagTerm(RuntimeTag runtimeTag, flecs.ecs_oper_kind_t oper)
        {
            m_filterDesc.terms[m_termCount++] = new flecs.ecs_term_t()
            {
                id = m_world.GetRuntimeTagId(runtimeTag),
                oper = oper,
            };
        }
        private void AddPairTerm<TTag, TComponent>(flecs.ecs_oper_kind_t oper)
            where TTag : unmanaged, ITag
            where TComponent : unmanaged, IEcsComponent
        {
            EcsId firstId = m_world.GetComponentId<TTag>();
            EcsId secondId = m_world.GetComponentId<TComponent>();

            m_filterDesc.terms[m_termCount].id = m_world.GetPairId(firstId, secondId);
            m_filterDesc.terms[m_termCount].oper = oper;
            m_termCount++;
        }
        private void AddPairTerm<TTag>(Entity entity, flecs.ecs_oper_kind_t oper)
            where TTag : unmanaged, ITag
        {
            EcsId firstId = m_world.GetComponentId<TTag>();

            m_filterDesc.terms[m_termCount].id = m_world.GetPairId(firstId, entity.Id);
            m_filterDesc.terms[m_termCount].oper = oper;
            m_termCount++;
        }
        private void AddManagedTerm<T>(flecs.ecs_oper_kind_t oper)
        {
            m_filterDesc.terms[m_termCount++] = new flecs.ecs_term_t()
            {
                id = m_world.GetManagedComponentId(typeof(T)),
                oper = oper,
            };
        }

        protected virtual void ResetBuilder()
        {
            m_filterDesc = new();
            m_termCount = 0;
        }
    }
}