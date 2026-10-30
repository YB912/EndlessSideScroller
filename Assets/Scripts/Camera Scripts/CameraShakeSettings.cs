
using UnityEngine;

[CreateAssetMenu(fileName = "CameraShakeSettings", menuName = "Camera/Camera Shake Settings")]
public class CameraShakeSettings : ScriptableObject
{
    [Header("Damage Shake Settings")]
    [SerializeField] float _damageShakeDuration = 0.1f;
    [SerializeField] float _damageShakeMaxAmplitude = 15f;
    [SerializeField] float _damageShakeFrequency = 0.1f;

    [Header("Death Shake Settings")]
    [SerializeField] float _deathShakeDuration = 0.75f;
    [SerializeField] float _deathShakeMaxAmplitude = 50.0f;
    [SerializeField] float _deathShakeFrequency = 0.1f;

    public float DamageShakeDuration => _damageShakeDuration;
    public float DamageShakeMaxAmplitude => _damageShakeMaxAmplitude;
    public float DamageShakeFrequency => _damageShakeFrequency;

    public float DeathShakeDuration => _deathShakeDuration;
    public float DeathShakeMaxAmplitude => _deathShakeMaxAmplitude;
    public float DeathShakeFrequency => _deathShakeFrequency;
}
