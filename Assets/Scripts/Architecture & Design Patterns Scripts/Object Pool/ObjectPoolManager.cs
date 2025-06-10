
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.ObjectPool
{
    public class ObjectPoolManager : MonoBehaviour
    {
        // Acts like a dictionary
        private List<ObjectPool> _objectPools;

        internal void Bootstrap()
        {
            _objectPools = new List<ObjectPool>();
        }

        #region Public Methods
        public GameObject TakeFromPool(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            var pool = FindOrCreatePool(prefab);

            var objectToReturn = FindOrInstantiateObject(pool, prefab, position, rotation, parent);

            return objectToReturn;
        }

        public void ReturnToPool(GameObject gameObject)
        {
            var pool = FindOrCreatePool(gameObject);

            if (pool.objects.Contains(gameObject) == false) { pool.objects.Add(gameObject); }

            gameObject.SetActive(false);
        }
        #endregion

        #region Private Methods
        private ObjectPool FindOrCreatePool(GameObject gameObject)
        {
            var output = _objectPools.Find(p => p.tag == gameObject.name);

            // Create a new pool if no existing pool has a tag that matches the given gameObject's name.
            if (output == null)
            {
                output = new ObjectPool(gameObject.name);
                _objectPools.Add(output);
            }
            return output;
        }

        private GameObject FindOrInstantiateObject(ObjectPool pool, GameObject gameObject, Vector3 position, Quaternion rotation, Transform parent)
        {
            var output = pool.objects.Find(o => o.activeSelf == false);

            // Instantiate a gameObject if there are no active instances in the pool.
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
        #endregion

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