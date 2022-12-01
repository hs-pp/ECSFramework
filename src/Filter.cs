using System;

namespace EcsFramework
{
    public sealed unsafe class Filter : IDisposable
    {
        public static implicit operator flecs.ecs_filter_t*(Filter filter) => filter.m_ecsFilter;
        
        private World m_world;
        private flecs.ecs_filter_t* m_ecsFilter;
        public bool WasDisposed { get; private set; } = false;

        public Filter(flecs.ecs_filter_desc_t filterDesc, World world)
        {
            m_world = world;
            m_ecsFilter = flecs.ecs_filter_init(world, &filterDesc);
        }
        
        public int GetCount()
        {
            flecs.ecs_iter_t iter = flecs.ecs_filter_iter(m_world, m_ecsFilter);
            int count = flecs.ecs_iter_count(&iter);
            flecs.ecs_iter_fini(&iter);
            return count;
        }
        
        public EcsIter GetIter()
        {
            return new EcsIter(flecs.ecs_filter_iter(m_world, m_ecsFilter),
                m_world, IterType.Filter);
        }

        public void Dispose()
        {
            if (!WasDisposed)
            {
                flecs.ecs_filter_fini(m_ecsFilter);
                WasDisposed = true;
            }
        }
    }
}