
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.ObjectPool
{
    /// <summary>
    /// Manages pools of reusable GameObjects to optimize instantiation overhead.
    /// </summary>
    public class ObjectPoolManager : MonoBehaviour
    {
        private List<ObjectPool> _objectPools;

        internal void Bootstrap()
        {
            _objectPools = new List<ObjectPool>();
        }

        /// <summary>
        /// Retrieves an inactive object from the pool or instantiates a new one if none are available.
        /// </summary>
        public GameObject TakeFromPool(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            var pool = FindOrCreatePool(prefab);
            var objectToReturn = FindOrInstantiateObject(pool, prefab, position, rotation, parent);
            return objectToReturn;
        }

        /// <summary>
        /// Returns a GameObject to its pool and deactivates it.
        /// </summary>
        public void ReturnToPool(GameObject gameObject)
        {
            var pool = FindOrCreatePool(gameObject);
            if (!pool.objects.Contains(gameObject)) pool.objects.Add(gameObject);
            gameObject.SetActive(false);
        }

        private ObjectPool FindOrCreatePool(GameObject gameObject)
        {
            var output = _objectPools.Find(p => p.tag == gameObject.name);
            if (output == null)
            {
                output = new ObjectPool(gameObject.name);
                _objectPools.Add(output);
            }
            return output;
        }

        private GameObject FindOrInstantiateObject(ObjectPool pool, GameObject gameObject, Vector3 position, Quaternion rotation, Transform parent)
        {
            var output = pool.objects.Find(o => !o.activeSelf);
            if (output == null)
            {
                output = Instantiate(gameObject, position, rotation, parent);
                pool.objects.Add(output);
            }
            output.transform.parent = parent;
            output.transform.position = position;
            output.transform.rotation = rotation;
            output.SetActive(true);
            return output;
        }

        /// <summary>
        /// Represents a pool of reusable GameObjects identified by a unique tag (usually the prefab name).
        /// Manages a list of pooled objects for efficient reuse.
        /// </summary>
        private class ObjectPool
        {
            public string tag { get; private set; }
            public List<GameObject> objects { get; private set; }

            public ObjectPool(string tag)
            {
                this.tag = tag;
                objects = new List<GameObject>();
            }
        }
    }
}
