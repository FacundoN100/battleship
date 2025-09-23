using System;
using System.Collections.Generic;

namespace MyGame
{
    public class ObjectPool<T> where T : IPoolable, new()
    {
        private List<T> pool = new List<T>();

        public T Get()
        {
            foreach (var obj in pool)
            {
                if (!obj.IsActive)
                {
                    obj.OnActivate();
                    return obj;
                }
            }

            T newObj = new T();
            newObj.OnActivate();
            pool.Add(newObj);
            return newObj;
        }

        public void Return(T obj)
        {
            obj.OnDeactivate();
        }

        public List<T> GetActiveObjects()
        {
            return pool.FindAll(o => o.IsActive);
        }
    }

    public interface IPoolable
    {
        bool IsActive { get; }
        void OnActivate();
        void OnDeactivate();
    }
}
