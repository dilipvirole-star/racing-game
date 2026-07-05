using UnityEngine;
using System.Collections.Generic;

namespace RacingGame.Utilities
{
    /// <summary>
    /// Generic object pool for efficient memory management and object reuse.
    /// </summary>
    public class ObjectPool<T> where T : Component
    {
        private Stack<T> _available = new();
        private HashSet<T> _inUse = new();
        private T _prefab;
        private Transform _parent;
        private int _initialSize;

        public ObjectPool(T prefab, int initialSize = 10, Transform parent = null)
        {
            _prefab = prefab;
            _initialSize = initialSize;
            _parent = parent;

            for (int i = 0; i < initialSize; i++)
            {
                CreateInstance();
            }
        }

        private void CreateInstance()
        {
            T instance = Object.Instantiate(_prefab, _parent);
            instance.gameObject.SetActive(false);
            _available.Push(instance);
        }

        public T Get()
        {
            T instance;
            if (_available.Count > 0)
            {
                instance = _available.Pop();
            }
            else
            {
                CreateInstance();
                instance = _available.Pop();
            }

            instance.gameObject.SetActive(true);
            _inUse.Add(instance);
            return instance;
        }

        public void Return(T instance)
        {
            if (_inUse.Remove(instance))
            {
                instance.gameObject.SetActive(false);
                _available.Push(instance);
            }
        }

        public int GetAvailableCount() => _available.Count;
        public int GetInUseCount() => _inUse.Count;
    }
}
