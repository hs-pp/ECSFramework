namespace EcsFramework
{
    public unsafe delegate void EcsSystemCallback(flecs.ecs_iter_t* itr);

    public abstract unsafe class SystemBase
    {
        public static implicit operator flecs.ecs_entity_t(SystemBase system) => system.m_ecsSystem;

        protected World m_world;
        private flecs.ecs_entity_t m_ecsSystem;
        private EcsSystemCallback m_systemCallback;

        public bool IsEnabled { get; set; } = true; // TODO: Look into how flecs natively disables systems.

        protected abstract void BuildSystem(SystemBuilder systemBuilder);
        protected abstract void OnInit();
        protected abstract void Update(EcsIter ecsIter);

        public virtual SystemPhase GetSystemPhase()
        {
            return SystemPhase.Update;
        }

        public SystemBase(World world)
        {
            m_world = world;
        }

        private bool m_wasInitialized = false;
        public void Initialize()
        {
            if (m_wasInitialized)
            {
                return;
            }
            
            SystemBuilder systemBuilder = new SystemBuilder(m_world);
            BuildSystem(systemBuilder);
            
            // If this callback isn't saved, it goes out of scope after a few seconds...
            m_systemCallback = new EcsSystemCallback(HandleUpdate);
            
            m_ecsSystem = systemBuilder
                .WithCallback(m_systemCallback)
                .Build();
            
            OnInit();
            m_wasInitialized = true;
        }
        
        private void HandleUpdate(flecs.ecs_iter_t* iter)
        {
            if(m_world.IsShuttingDown || !IsEnabled)
            {
                return;
            }

            m_world.DeferStart();
            EcsIter ecsIter = new EcsIter(*iter, m_world, IterType.System);
            Update(ecsIter);
            ecsIter.Dispose();
            m_world.DeferStop();
        }
    }
}