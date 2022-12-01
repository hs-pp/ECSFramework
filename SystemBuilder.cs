using System.Runtime.InteropServices;

namespace EcsFramework
{
    public unsafe class SystemBuilder : BaseQueryBuilder<SystemBuilder>
    {
        private flecs.ecs_system_desc_t m_systemDesc = default;
        
        public SystemBuilder(World world) : base(world) { }

        public SystemBuilder WithCallback(EcsSystemCallback callback)
        {
            m_systemDesc.callback.Data.Pointer = 
                (delegate* unmanaged[Stdcall]<flecs.ecs_iter_t*, void>)Marshal.GetFunctionPointerForDelegate(callback);

            return this;
        }

        public flecs.ecs_entity_t Build()
        {
            m_systemDesc.query = new flecs.ecs_query_desc_t()
            {
                filter = m_filterDesc,
            };
            
            fixed (flecs.ecs_system_desc_t* systemDescPtr = &m_systemDesc)
            {
                flecs.ecs_entity_t systemEntity = flecs.ecs_system_init(m_world, systemDescPtr);
                ResetBuilder();
                return systemEntity;
            }
        }

        protected override void ResetBuilder()
        {
            base.ResetBuilder();
            m_systemDesc = default;
        }
    }
}