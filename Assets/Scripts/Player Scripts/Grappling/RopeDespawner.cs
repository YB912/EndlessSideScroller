
using DesignPatterns.ObjectPool;
using DesignPatterns.ServiceLocatorPattern;
using UnityEngine;

public class RopeDespawner : MonoBehaviour, IInitializeable
{
    const string SEGMENT_LAYER_NAME = "RopeSegments";
    int _segmentsLayerBitMap;
    ObjectPoolManager _objectPool;

    public void Initialize()
    {
        _segmentsLayerBitMap = LayerMask.NameToLayer(SEGMENT_LAYER_NAME);
        _objectPool = ServiceLocator.instance.Get<ObjectPoolManager>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _segmentsLayerBitMap)
        {
            _objectPool.ReturnToPool(collision.gameObject);
        }
    }
}
