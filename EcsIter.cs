using System;
using Unity.Collections.LowLevel.Unsafe;

namespace EcsFramework
{
    public enum IterType
    {
        Filter,
        Query,
        Observer,
        System,
        Unknown,
    }

    /// <summary>
    /// https://github.com/SanderMertens/flecs/blob/master/docs/Queries.md#query-iteration
    /// </summary>
    public sealed unsafe class EcsIter : IDisposable
    {
        private World m_world;
        private IterType m_iterType;
        private flecs.ecs_iter_t m_iter;
        public int Index { get; private set; } = -1;
        public bool WasDisposed { get; private set; } = false;

        public EcsIter(flecs.ecs_iter_t iter, World world, IterType iterType = IterType.Unknown)
        {
            m_world = world;
            m_iter = iter;
            m_iterType = iterType;
        }

        public void Dispose()
        {
            if (!WasDisposed)
            {
                fixed (flecs.ecs_iter_t* iterPtr = &m_iter)
                {
                    flecs.ecs_iter_fini(iterPtr);
                }
                WasDisposed = true;
            }
        }

        public ref T GetComponent<T>(int termIndex) where T : unmanaged, IEcsComponent
        {
            return ref GetTermArray<T>(termIndex)[Index];
        }
        
        public T GetManagedComponent<T>()
        {
            Entity entity = GetEntity();
            return entity.GetManagedComponent<T>();
        }

        public bool PairFirstIsType<T>(int termIndex) where T : unmanaged, IEcsComponent
        {
            return GetPairFirstId(termIndex).Data == m_world.GetComponentId<T>();
        }
        public bool PairSecondIsType<T>(int termIndex) where T : unmanaged, IEcsComponent
        {
            return GetPairSecondId(termIndex).Data == m_world.GetComponentId<T>();
        }
        public ref T GetPairSecondAsComponent<T>(int termIndex) where T : unmanaged, IEcsComponent
        {
            return ref GetComponent<T>(termIndex);
        }
        public Entity GetPairSecondAsEntity(int termIndex)
        {
            return m_world.GetEntity(GetPairSecondId(termIndex));
        }
        private flecs.ecs_entity_t GetPairFirstId(int termIndex)
        {
            fixed (flecs.ecs_iter_t* iterPtr = &m_iter)
            {
                var pairId = flecs.ecs_field_id(iterPtr, termIndex + 1);
                return m_world.GetEntity(flecs.ecs_get_alive(m_world, 
                    (flecs.ecs_id_t)((pairId & flecs.ECS_COMPONENT_MASK) >> 32)));
            }
        }
        private flecs.ecs_entity_t GetPairSecondId(int termIndex)
        {
            fixed (flecs.ecs_iter_t* iterPtr = &m_iter)
            {
                var pairId = flecs.ecs_field_id(iterPtr, termIndex + 1);
                return flecs.ecs_get_alive(m_world, (flecs.ecs_id_t)(uint)pairId); // TODO: is it ulong???
            }
        }

        public Entity GetEntity()
        {
            return m_world.GetEntity(m_iter.entities[Index]);
        }

        private Span<T> GetTermArray<T>(int termIndex) where T : unmanaged, IEcsComponent
        {
            ulong structSize = (ulong)UnsafeUtility.SizeOf<T>();
            fixed (flecs.ecs_iter_t* iterPtr = &m_iter)
            {
                var pointer = flecs.ecs_field_w_size(iterPtr, structSize, termIndex + 1);
                return new Span<T>(pointer, m_iter.count);
            }
        }

        public bool MoveNext()
        {
            if (++Index < m_iter.count)
            {
                return true;
            }

            Index = 0;
            fixed (flecs.ecs_iter_t* iterPtr = &m_iter)
            {
                switch (m_iterType)
                {
                    case IterType.Filter:
                        return flecs.ecs_filter_next(iterPtr);
                    case IterType.Query:
                    case IterType.System:
                        return flecs.ecs_query_next(iterPtr);
                    case IterType.Unknown: // ecs_iter_next is slightly slower but still works.
                        return flecs.ecs_iter_next(iterPtr);
                    default:
                        return false;
                }
            }
        }
    }
}