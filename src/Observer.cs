using System;
using System.Runtime.InteropServices;

namespace EcsFramework
{
    public unsafe delegate void EcsObserverCallback(flecs.ecs_iter_t* iter);
    public delegate void ObserverCallback(EcsIter itr);

    public sealed unsafe class Observer : IDisposable
    {
        public static implicit operator flecs.ecs_entity_t(Observer observer) => observer.m_ecsObserver;

        private World m_world;
        private flecs.ecs_entity_t m_ecsObserver;
        private EcsObserverCallback m_ecsObserverCallback; // cached raw ecs callback
        private ObserverCallback m_observerCallback; // dev callback
        public bool WasDisposed { get; private set; } = false;

        public Observer(flecs.ecs_observer_desc_t observerDesc, ObserverCallback callback, World world)
        {
            m_world = world;
            m_observerCallback = callback;
            m_ecsObserverCallback = new EcsObserverCallback(HandleObserverCallback);
            
            observerDesc.callback.Data.Pointer = 
                (delegate* unmanaged[Stdcall]<flecs.ecs_iter_t*, void>)Marshal.GetFunctionPointerForDelegate(m_ecsObserverCallback);
            
            m_ecsObserver = flecs.ecs_observer_init(world, &observerDesc);
        }

        private void HandleObserverCallback(flecs.ecs_iter_t* iter)
        {
            if(m_world.IsShuttingDown)
            {
                return;
            }

            m_world.DeferStart();
            EcsIter ecsIter = new EcsIter(*iter, m_world, IterType.Observer);
            m_observerCallback?.Invoke(ecsIter);
            ecsIter.Dispose();
            m_world.DeferStop();
        }
        
        public void Dispose()
        {
            if (!WasDisposed)
            {
                // TODO: Can we easily dispose an observer?
                m_ecsObserverCallback = null;
                m_observerCallback = null;
                WasDisposed = true;
            }
        }
    }
}