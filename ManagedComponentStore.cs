using System;
using System.Collections.Generic;

namespace EcsFramework
{
    public class ManagedComponentStore : IDisposable
    {
        private object[] m_managedComponentStore = new object[64];
        private Dictionary<ulong, int> m_indexLookup = new();
        private Queue<int> m_freeIndices = new();
        private int m_currentCount = 0;

        public void SetManagedComponent(EcsId id, object managedComponent)
        {
            if (m_indexLookup.TryGetValue(id, out int existingIndex))
            {
                m_managedComponentStore[existingIndex] = managedComponent;
                return;
            }

            if (m_freeIndices.Count > 0)
            {
                int newIndex = m_freeIndices.Dequeue();
                m_indexLookup.Add(id, newIndex);
                m_managedComponentStore[newIndex] = managedComponent;
                return;
            }

            if (m_currentCount >= m_managedComponentStore.Length)
            {
                Array.Resize(ref m_managedComponentStore, m_managedComponentStore.Length*2);
            }

            int index = m_currentCount++;
            m_indexLookup.Add(id, index);
            m_managedComponentStore[index] = managedComponent;
        }

        public object GetManagedComponent(EcsId id)
        {
            if (m_indexLookup.TryGetValue(id, out int index))
            {
                return m_managedComponentStore[index];
            }
            return null;
        }
        
        public void RemoveManagedComponent<T>(EcsId id)
        {
            if (m_indexLookup.TryGetValue(id, out int index))
            {
                m_managedComponentStore[index] = null;
                m_indexLookup.Remove(id);
                m_freeIndices.Enqueue(index);
            }
        }
        
        public void Dispose()
        {
            for (int i = 0; i < m_currentCount; i++)
            {
                if (m_managedComponentStore[m_currentCount] is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            m_indexLookup.Clear();
            m_freeIndices.Clear();
            m_currentCount = 0;
        }
    }
}