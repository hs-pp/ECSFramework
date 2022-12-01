using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;

namespace EcsFramework
{
    public unsafe struct Entity
    {
        public static implicit operator flecs.ecs_entity_t(Entity entity) => entity.m_handle;

        private GCHandle m_worldHandle; // The GCHandle is a value type which allows this class to be a struct.
        private readonly flecs.ecs_entity_t m_handle;

        public EcsId Id { get; }

        public string Name
        {
            get
            {
                World world = GetWorld();
                return flecs.ecs_get_name(world, this);
            }
            set
            {
                World world = GetWorld();
                flecs.ecs_set_name(world, this, value);
            }
        }
        
        public Entity(World world, flecs.ecs_entity_t handle)
        {
            m_worldHandle = GCHandle.Alloc(world);
            m_handle = handle;
            Id = new EcsId(m_handle.Data);
        }
        
        public bool IsAlive()
        {
            World world = GetWorld();
            return m_handle.Data.Data != 0 && flecs.ecs_is_alive(world, this);
        }
        
        private World GetWorld()
        {
            return (World) m_worldHandle.Target;
        }

        public override bool Equals(object obj)
        {
            return Id.Equals(((Entity)obj).Id);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region Components
        public void AddComponent<T>() where T : unmanaged, IComponent
        {
            World world = GetWorld();
            flecs.ecs_add_id(world, m_handle, world.GetComponentId<T>());
        }
        public void AddComponent<T>(T value) where T : unmanaged, IComponent
        {
            SetComponent<T>(ref value);
        }
        public void RemoveComponent<T>() where T : unmanaged, IComponent
        {
            World world = GetWorld();
            flecs.ecs_remove_id(world, m_handle, world.GetComponentId<T>());
        }
        public bool HasComponent<T>() where T : unmanaged, IComponent
        {
            World world = GetWorld();
            return flecs.ecs_has_id(world, this, world.GetComponentId<T>());
        }
        /// <summary>
        /// Get will ADD the component to the entity if it doesn't already have it!!!!
        /// </summary>
        public ref T GetComponent<T>() where T : unmanaged, IComponent // currently returned as a ref but maybe we don't want that.
        {
            if (!HasComponent<T>())
            {
                //Debug.LogWarning("WARNING! Getting a component that the entity doesn't have! This will auto add it to the entity!");
            }
            
            World world = GetWorld();
            void* pointer = flecs.ecs_get_mut_id(world, m_handle, world.GetComponentId<T>());
            return ref UnsafeUtility.AsRef<T>(pointer);
        }
        public void SetComponent<T>(ref T component) where T : unmanaged, IComponent
        {
            World world = GetWorld();
            flecs.ecs_id_t componentId = world.GetComponentId<T>();
            ulong structSize = (ulong)UnsafeUtility.SizeOf(component.GetType());
            void* pointer = UnsafeUtility.AddressOf(ref component);
            flecs.ecs_set_id(world, this, componentId, structSize, pointer);
        }
        public void MarkComponentModified<T>() where T : unmanaged, IComponent
        {
            World world = GetWorld();
            flecs.ecs_modified_id(world, this, world.GetComponentId<T>());
        }
        #endregion

        #region Tags
        public void AddTag<T>() where T : unmanaged, ITag
        {
            World world = GetWorld();
            flecs.ecs_add_id(world, this, world.GetComponentId<T>());
        }
        public void RemoveTag<T>() where T : unmanaged, ITag
        {
            World world = GetWorld();
            flecs.ecs_remove_id(world, m_handle, world.GetComponentId<T>());
        }
        public bool HasTag<T>() where T : unmanaged, ITag
        {
            World world = GetWorld();
            return flecs.ecs_has_id(world, this, world.GetComponentId<T>());
        }

        public void AddTag(RuntimeTag runtimeTag)
        {
            World world = GetWorld();
            flecs.ecs_add_id(world, m_handle, world.GetRuntimeTagId(runtimeTag));
        }
        public void RemoveTag(RuntimeTag runtimeTag)
        {
            World world = GetWorld();
            flecs.ecs_remove_id(world, m_handle, world.GetRuntimeTagId(runtimeTag));
        }
        public bool HasTag(RuntimeTag runtimeTag)
        {
            World world = GetWorld();
            return flecs.ecs_has_id(world, this, world.GetRuntimeTagId(runtimeTag));
        }
        #endregion
        
        #region ManagedComponents
        public void AddManagedComponent<T>(T managedComponent)
        {
            World world = GetWorld();
            world.SetManagedComponent(managedComponent, this);
            if (!HasManagedComponent<T>())
            {
                flecs.ecs_add_id(world, this, world.GetManagedComponentId(managedComponent.GetType()));
            }
        }
        public T GetManagedComponent<T>()
        {
            object managedComponent = GetWorld().GetManagedComponent<T>(this);
            return (T)managedComponent;
        }
        public void RemoveManagedComponent<T>()
        {
            World world = GetWorld();
            flecs.ecs_remove_id(world, this, world.GetManagedComponentId(typeof(T)));
            world.RemoveManagedComponent<T>(this);
        }
        public bool HasManagedComponent<T>()
        {
            World world = GetWorld();
            return flecs.ecs_has_id(world, this, world.GetManagedComponentId(typeof(T)));
        }
        #endregion
        
        /// github.com/SanderMertens/flecs/blob/master/docs/Relationships.md#relationship-components
        #region Pairs
        public void AddPair<TTag, TTag2>()
            where TTag : unmanaged, ITag
            where TTag2 : unmanaged, ITag
        {
            World world = GetWorld();
            EcsId pairId = world.GetPairId<TTag, TTag2>();
            flecs.ecs_add_id(world, this, pairId);
        }
        public void AddPair<TTag, TComponent>(TComponent component)
            where TTag : unmanaged, ITag
            where TComponent : unmanaged, IComponent
        {
            World world = GetWorld();
            var structSize = (ulong)UnsafeUtility.SizeOf<TComponent>();
            var pointer = UnsafeUtility.AddressOf(ref component);
            EcsId pairId = world.GetPairId<TTag, TComponent>();
            flecs.ecs_set_id(world, this, pairId, structSize, pointer);
        }
        public void AddPair<TTag>(Entity entity)
            where TTag : unmanaged, ITag
        {
            World world = GetWorld();
            EcsId first = world.GetComponentId<TTag>();
            EcsId pairId = world.GetPairId(first, entity.Id);
            flecs.ecs_add_id(world, this, pairId);
        }
        public void RemovePair<TTag, TComponent>()
            where TTag : unmanaged, ITag
            where TComponent : unmanaged, IEcsComponent
        {
            World world = GetWorld();
            EcsId pairId = world.GetPairId<TTag, TComponent>();
            flecs.ecs_remove_id(world, this, pairId);
        }
        public void RemovePair<TTag>(Entity entity)
            where TTag : unmanaged, ITag
        {
            World world = GetWorld();
            EcsId first = world.GetComponentId<TTag>();
            EcsId pairId = world.GetPairId(first, entity.Id);
            flecs.ecs_remove_id(world, this, pairId);
        }
        public bool HasPair<TTag, TComponent>()
            where TTag : unmanaged, ITag
            where TComponent : unmanaged, IEcsComponent
        {
            World world = GetWorld();
            EcsId pairId = world.GetPairId<TTag, TComponent>();
            return flecs.ecs_has_id(world, this, pairId);
        }
        public bool HasPair<TTag>(Entity entity)
            where TTag : unmanaged, ITag
        {
            World world = GetWorld();
            EcsId first = world.GetComponentId<TTag>();
            EcsId pairId = world.GetPairId(first, entity.Id);
            return flecs.ecs_has_id(world, this, pairId);
        }
        public ref TComponent GetPair<TTag, TComponent>()
            where TTag : unmanaged, ITag
            where TComponent : unmanaged, IEcsComponent
        {
            if (!HasPair<TTag, TComponent>())
            {
                //Debug.LogWarning("WARNING! Getting a pair that the entity doesn't have! This will auto add it to the entity!");
            }
            World world = GetWorld();
            EcsId pairId = world.GetPairId<TTag, TComponent>();
            var pointer = flecs.ecs_get_mut_id(world, this, pairId);
            return ref UnsafeUtility.AsRef<TComponent>(pointer);
        }

        public void MarkPairModified<TTag, TComponent>()
            where TTag : unmanaged, ITag
            where TComponent : unmanaged, IEcsComponent
        {
            World world = GetWorld();
            flecs.ecs_modified_id(world, this, world.GetPairId<TTag, TComponent>());
        }
        #endregion
    }
}