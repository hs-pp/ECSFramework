using System;

namespace EcsFramework
{
    public sealed unsafe class Query : IDisposable
    {
        public static implicit operator flecs.ecs_query_t*(Query query) => query.m_ecsQuery;

        private World m_world;
        private flecs.ecs_query_t* m_ecsQuery;
        public bool WasDisposed { get; private set; } = false;

        public Query(flecs.ecs_filter_desc_t filterDesc, World world)
        {
            m_world = world;

            var queryDesc = new flecs.ecs_query_desc_t()
            {
                filter = filterDesc,
            };
            m_ecsQuery = flecs.ecs_query_init(m_world, &queryDesc);
        }
        
        public int GetCount()
        {
            flecs.ecs_iter_t iter = flecs.ecs_query_iter(m_world, m_ecsQuery);
            int count = flecs.ecs_iter_count(&iter);
            flecs.ecs_iter_fini(&iter);
            return count;
        }
        
        public EcsIter GetIter()
        {
            return new EcsIter(flecs.ecs_query_iter(m_world, m_ecsQuery),
                m_world, IterType.Query);
        }
        
        public void Dispose()
        {
            if (!WasDisposed)
            {
                flecs.ecs_query_fini(m_ecsQuery);
                WasDisposed = true;
            }
        }
    }
}