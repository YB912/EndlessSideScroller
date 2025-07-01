

using DesignPatterns.ObjectPool;
using DesignPatterns.ServiceLocatorPattern;
using UnityEngine;

/// <summary>
/// Returns rope segment objects to the object pool upon collision with this object.
/// </summary>
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
        // Despawn only if the colliding object belongs to the rope segment layer
        if (collision.gameObject.layer == _segmentsLayerBitMap)
        {
            _objectPool.ReturnToPool(collision.gameObject);
        }
    }
}
