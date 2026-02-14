using UnityEngine;

[CreateAssetMenu(fileName = "AfterImageSettings", menuName = "After Image Settings")]
public class AfterImageSettings : ScriptableObject
{
    [Header("Emitter Properties")]
    [SerializeField] float _speedThreshold = 12f;
    [SerializeField] float _spawnInterval = 0.05f;

    [Header("After Image Properties")]
    [SerializeField] float _afterImageLifetime = 0.25f;
    [SerializeField] float _afterImageStartAlpha = 0.6f;
    [SerializeField] Material _afterImageMaterial;

    public float speedThreshold => _speedThreshold;
    public float spawnInterval => _spawnInterval;
    public float afterImageLifetime => _afterImageLifetime;
    public float afterImageStartAlpha => _afterImageStartAlpha;
    public Material afterImageMaterial => _afterImageMaterial;
}
