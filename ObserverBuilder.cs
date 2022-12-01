namespace EcsFramework
{
    public enum ObserverEvent
    {
        EcsOnAdd = flecs.ECS_HI_COMPONENT_ID + 33,
        EcsOnRemove = flecs.ECS_HI_COMPONENT_ID + 34,
        EcsOnSet = flecs.ECS_HI_COMPONENT_ID + 35,
        EcsOnUnSet = flecs.ECS_HI_COMPONENT_ID + 36,
        EcsOnDelete = flecs.ECS_HI_COMPONENT_ID + 37,
    }
    
    public class ObserverBuilder : BaseQueryBuilder<ObserverBuilder>
    {
        private flecs.ecs_observer_desc_t m_observerDesc = default;
        private ObserverCallback m_observerCallback;
        
        public ObserverBuilder(World world) : base(world) { }
        
        public ObserverBuilder WithCallback(ObserverCallback callback)
        {
            m_observerCallback = callback;
            return this;
        }

        public ObserverBuilder OnEvent(ObserverEvent observerEvent)
        {
            m_observerDesc.events[0] = (flecs.ecs_id_t)(ulong)observerEvent;
            return this;
        }
        
        public Observer Build()
        {
            m_observerDesc.filter = m_filterDesc;
            Observer observer = new Observer(m_observerDesc, m_observerCallback, m_world);
            ResetBuilder();
            return observer;
        }

        protected override void ResetBuilder()
        {
            base.ResetBuilder();
            m_observerDesc = default;
            m_observerCallback = null;
        }
    }
}