namespace EcsFramework
{
    public struct EcsId
    {
        private flecs.ecs_id_t m_handle;
        
        public static implicit operator flecs.ecs_id_t(EcsId ecsId) => ecsId.m_handle;
        public static implicit operator flecs.ecs_entity_t(EcsId ecsId) => new flecs.ecs_entity_t() { Data = ecsId.m_handle };
        public static implicit operator ulong(EcsId ecsId) => ecsId.m_handle.Data;
        
        public EcsId(flecs.ecs_id_t id)
        {
            m_handle = id;
        }

        public EcsId(ulong id)
        {
            m_handle = new flecs.ecs_id_t() { Data = id };
        }

        public EcsId(BuiltInTag builtInTag)
        {
            m_handle = new flecs.ecs_id_t() { Data = (ulong)builtInTag };
        }

        public override bool Equals(object obj)
        {
            return (ulong)this == (ulong)(EcsId)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        
        public override string ToString()
        {
            return $"EcsId: { (ulong)this }";
        }
    }
}