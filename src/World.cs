using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;

namespace EcsFramework
{
    // raw.githubusercontent.com/SanderMertens/flecs/master/flecs.c
    public enum BuiltInTag : ulong
    {
        EcsWorld = flecs.ECS_HI_COMPONENT_ID + 0,
        EcsSystem = 10,
        Wildcard = flecs.ECS_HI_COMPONENT_ID + 10,
        Any = flecs.ECS_HI_COMPONENT_ID + 11,
        EcsPhase = flecs.ECS_HI_COMPONENT_ID + 75,
        EcsRest =  flecs.ECS_HI_COMPONENT_ID + 105,
    }
    
    public struct WildCardTag : ITag { }
    public struct AnyTag : ITag { }
    
    public unsafe class World
    {
        public static implicit operator flecs.ecs_world_t*(World world) => world.m_ecsWorld;

        private flecs.ecs_world_t* m_ecsWorld;
        public flecs.ecs_entity_t WorldEntity { get; private set; }

        private Dictionary<Type, BuiltInTag> m_builtInTagLookup = new()
        {
            [typeof(WildCardTag)] = BuiltInTag.Wildcard,
            [typeof(AnyTag)] = BuiltInTag.Any,
        };
        private Dictionary<Type, EcsId> m_componentToIdLookup = new Dictionary<Type, EcsId>();
        private SystemList m_systemlist;
        private ManagedComponentStore m_managedComponentStore;
        
        public bool IsShuttingDown { get; private set; } = false;
        
        public World(bool autoLoadSystem = true)
        {
            m_ecsWorld = flecs.ecs_init();
            WorldEntity = new flecs.ecs_entity_t() { Data = (ulong)BuiltInTag.EcsWorld };
            m_managedComponentStore = new ManagedComponentStore();
            m_systemlist = new SystemList(this);
            if (autoLoadSystem)
            {
                m_systemlist.LoadAllSystems();
            }
            EnableRest();
        }

        public void Dispose()
        {
            if (!IsShuttingDown)
            {
                IsShuttingDown = true;
                flecs.ecs_fini(m_ecsWorld);
                m_systemlist.Dispose();
                m_managedComponentStore.Dispose();
            }
        }

        public void Tick(float deltaTime)
        {
            flecs.ecs_progress(m_ecsWorld, deltaTime);
            m_systemlist.TickSystems(deltaTime, this);
        }

        public void DeferStart()
        {
            flecs.ecs_defer_begin(this);
        }

        public void DeferStop()
        {
            flecs.ecs_defer_end(this);
        }

        public void EnableRest()
        {
            RestApi.EnableRest(this);
        }

        #region Entities
        public Entity CreateEntity(string name = "")
        {
            var desc = default(flecs.ecs_entity_desc_t);
            var entity = flecs.ecs_entity_init(m_ecsWorld, &desc);
            Entity result = new Entity(this, entity);
            if (!string.IsNullOrEmpty(name))
            {
                result.Name = $"{name} ({(ulong)(result.Id)})";
            }
            return result;
        }
        public void DeleteEntity(Entity entity)
        {
            flecs.ecs_delete(m_ecsWorld, entity);
        }
        public Entity GetEntity(flecs.ecs_id_t id)
        {
            Entity entity = new Entity(this, id);
            if (!flecs.ecs_exists(this, entity))
            {
                var entityDesc = default(flecs.ecs_entity_desc_t);
                entityDesc.id = id;
                flecs.ecs_entity_init(this, &entityDesc);
            }
            return entity;
        }
        public int GetNumEntitiesWithComponent<T>() where T : unmanaged, IEcsComponent
        {
            EcsId componentId = GetComponentId<T>();
            return flecs.ecs_count_id(this, componentId);
        }
        #endregion
        
        #region Components
        private void RegisterComponent<T>() where T : unmanaged, IEcsComponent
        {
            flecs.ecs_component_desc_t componentDesc = GetComponentDescriptor<T>();
            var id = flecs.ecs_component_init(m_ecsWorld, &componentDesc);
            m_componentToIdLookup.Add(typeof(T), new EcsId(id.Data));
        }
        private flecs.ecs_component_desc_t GetComponentDescriptor<T>() where T : unmanaged, IEcsComponent
        {
            Type type = typeof(T);
            var structLayoutAttribute = type.StructLayoutAttribute;
            if (structLayoutAttribute == null || structLayoutAttribute.Value == LayoutKind.Auto)
            {
                throw new Exception(
                    "Flecs: Component must have a StructLayout attribute with LayoutKind sequential or explicit. This is to ensure that the struct fields are not reorganized by the C# compiler.");
            }
            var structAlignment = structLayoutAttribute!.Pack;
            int structSize = UnsafeUtility.SizeOf<T>();
            flecs.ecs_component_desc_t compDesc = default(flecs.ecs_component_desc_t);

            var componentName = flecs.Runtime.CStrings.CString(type.FullName);
            flecs.ecs_entity_desc_t entityDesc = new flecs.ecs_entity_desc_t()
            {
                symbol = componentName,
                name = componentName,
            };
            compDesc.entity = flecs.ecs_entity_init(m_ecsWorld, &entityDesc);
            compDesc.type.alignment.Data = structAlignment;
            compDesc.type.size.Data = structSize;
            return compDesc;
        }
        public EcsId GetComponentId<T>() where T : unmanaged, IEcsComponent
        {
            Type type = typeof(T);

            if (m_builtInTagLookup.ContainsKey(type))
            {
                return new EcsId(m_builtInTagLookup[type]);
            }
            if (!m_componentToIdLookup.ContainsKey(type))
            {
                RegisterComponent<T>();
            }
            return m_componentToIdLookup[type];
        }
        public EcsId GetRuntimeTagId(RuntimeTag runtimeTag)
        {
            flecs.ecs_entity_t tagId = flecs.ecs_lookup(this, runtimeTag.Name);

            if (tagId.Data.Data == 0)
            {
                flecs.ecs_entity_desc_t desc = default;
                desc.name = runtimeTag.Name;
                tagId = flecs.ecs_entity_init(this, &desc);
            }

            return new EcsId(tagId);
        }
        public EcsId GetManagedComponentId(Type managedComponentType)
        {
            string idName = $"MANAGED_{managedComponentType.FullName}";

            flecs.ecs_entity_t tagId = flecs.ecs_lookup(this, idName);

            if (tagId.Data.Data == 0)
            {
                flecs.ecs_entity_desc_t desc = default;
                desc.name = idName;
                tagId = flecs.ecs_entity_init(this, &desc);
            }

            return new EcsId(tagId);
        }
        public EcsId GetPairId<TTag, TComponent>()
            where TTag : unmanaged, ITag
            where TComponent : unmanaged, IEcsComponent
        {
            EcsId first = GetComponentId<TTag>();
            EcsId second = GetComponentId<TComponent>();
            return new EcsId(flecs.ecs_make_pair(first, second));
        }
        public EcsId GetPairId(EcsId firstElement, EcsId secondElement)
        {
            return new EcsId(flecs.ecs_make_pair(firstElement, secondElement));
        }
        
        public ref T GetSingletonComponent<T>() where T : unmanaged, ISingletonComponent
        {
            EcsId ecsId = GetComponentId<T>();
            if (!flecs.ecs_has_id(this, WorldEntity, ecsId))
            {
                flecs.ecs_add_id(this, WorldEntity, ecsId);
            }
            void* pointer = flecs.ecs_get_mut_id(this, WorldEntity,ecsId);
            return ref UnsafeUtility.AsRef<T>(pointer);
        }
        public void MarkSingletonComponentModified<T>() where T : unmanaged, ISingletonComponent
        {
            flecs.ecs_modified_id(this, WorldEntity, GetComponentId<T>());
        }

        public void SetManagedComponent(object managedComponent, Entity entity)
        {
            m_managedComponentStore.SetManagedComponent(
                GetPairId(GetManagedComponentId(managedComponent.GetType()), entity.Id),
                managedComponent);
        }
        public object GetManagedComponent<T>(Entity entity)
        {
            return m_managedComponentStore.GetManagedComponent(
                GetPairId(GetManagedComponentId(typeof(T)), entity.Id));
        }
        public void RemoveManagedComponent<T>(Entity entity)
        {
            m_managedComponentStore.RemoveManagedComponent<T>(
                GetPairId(GetManagedComponentId(typeof(T)), entity.Id));
        }
        #endregion
        
        #region Systems
        public T RegisterSystem<T>(T system = null) where T : SystemBase
        {
            return m_systemlist.RegisterSystem<T>(system);
        }
        public T GetSystem<T>() where T : SystemBase
        {
            return m_systemlist.GetSystem<T>();
        }
        #endregion
    }
}