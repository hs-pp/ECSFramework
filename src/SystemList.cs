using System;
using System.Collections.Generic;
using System.Linq;

namespace EcsFramework
{
    public enum SystemPhase : int
    {
        PreUpdate = 0,
        Update = 1,
        PostUpdate = 2,
        Render = 3,
    }

    public class SystemComparer : IComparer<SystemBase>
    {
        public int Compare(SystemBase x, SystemBase y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;

            return ((int)x.GetSystemPhase()).CompareTo((int)y.GetSystemPhase());
        }
    }
    
    public unsafe class SystemList : IDisposable
    {
        private World m_world;
        private List<SystemBase> m_sortedSystems = new();
        private Dictionary<Type, SystemBase> m_systems = new();
        private SystemComparer m_systemComparer = new();

        public SystemList(World world)
        {
            m_world = world;
        }
        
        public void LoadAllSystems()
        {
            Type baseType = typeof(SystemBase);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => baseType.IsAssignableFrom(p) 
                            && !p.IsAbstract 
                            && p.Namespace != "EcsFramework.Tests");

            foreach (Type type in types)
            {
                SystemBase system = (SystemBase)Activator.CreateInstance(type, this);
                RegisterSystem(system);
            }
        }
        
        public T RegisterSystem<T>(T system = null) where T : SystemBase
        {
            Type type = typeof(T);
            if (!m_systems.ContainsKey(type))
            {
                if (system == null)
                {
                    system = (T)Activator.CreateInstance(type, m_world);
                }
                system.Initialize();

                int index = m_sortedSystems.BinarySearch(system, m_systemComparer);
                if(index < 0)
                {
                    m_sortedSystems.Insert(~index, system);
                    m_systems.Add(type, system); 
                }

                return system;
            }

            return GetSystem<T>();
        }
        public T GetSystem<T>() where T : SystemBase
        {
            m_systems.TryGetValue(typeof(T), out SystemBase system);
            return (T)system;
        }
        
        public void TickSystems(float deltaTime, World world)
        {
            for (int i = 0; i < m_sortedSystems.Count; i++)
            {
                flecs.ecs_run(world, m_sortedSystems[i], deltaTime, null);
            }
        }

        public void Dispose()
        {
            m_systems.Clear();
        }
    }
}