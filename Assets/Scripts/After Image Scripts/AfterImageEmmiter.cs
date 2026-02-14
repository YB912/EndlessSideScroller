using DesignPatterns.ObjectPool;
using DesignPatterns.ServiceLocatorPattern;
using UnityEngine;

public class AfterImageEmitter : MonoBehaviour, IInitializeable
{
    [Header("Settings & Components")]
    [SerializeField] AfterImageSettings _settings;
    [SerializeField] Rigidbody2D _rigidBody;

    static GameObject _globalAfterImageContainer;

    ObjectPoolManager _objectPool;
    GameObject _afterImageTemplate;
    SpriteRenderer[] _bodyPartRenderers;

    float _timer;

    public void Initialize()
    {
        _objectPool = ServiceLocator.instance.Get<ObjectPoolManager>();

        EnsureGlobalContainer();
        SetupAfterImagePrefab();

        // Find ALL sprite renderers under the player (body parts)
        _bodyPartRenderers = GetComponentsInChildren<SpriteRenderer>(true);
    }

    void Update()
    {
        if (_rigidBody.linearVelocity.magnitude < _settings.speedThreshold)
        {
            _timer = 0f;
            return;
        }

        _timer += Time.deltaTime;
        if (_timer >= _settings.spawnInterval)
        {
            SpawnAfterImages();
            _timer = 0f;
        }
    }

    void SpawnAfterImages()
    {
        foreach (var sr in _bodyPartRenderers)
        {
            if (!sr.enabled)
                continue;

            var afterImage = _objectPool.TakeFromPool(
                _afterImageTemplate,
                sr.transform.position,
                sr.transform.rotation,
                _globalAfterImageContainer.transform
            );

            afterImage.transform.localScale = sr.transform.lossyScale;

            afterImage.GetComponent<AfterImageController>().Activate(_settings, sr);
        }
    }

    static void EnsureGlobalContainer()
    {
        if (_globalAfterImageContainer != null)
            return;

        _globalAfterImageContainer = new GameObject("AfterImages");
        _globalAfterImageContainer.transform.position = Vector3.zero;
        _globalAfterImageContainer.transform.rotation = Quaternion.identity;
    }

    void SetupAfterImagePrefab()
    {
        _afterImageTemplate = new GameObject("AfterImage_TEMPLATE");
        _afterImageTemplate.SetActive(false);
        _afterImageTemplate.transform.SetParent(_globalAfterImageContainer.transform);

        _afterImageTemplate.AddComponent<SpriteRenderer>();
        _afterImageTemplate.AddComponent<AfterImageController>();
    }
}
