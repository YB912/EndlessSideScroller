using DesignPatterns.ObjectPool;
using DesignPatterns.ServiceLocatorPattern;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AfterImageEmitter : MonoBehaviour, IInitializeable
{
    [Header("Settings & Components")]
    [SerializeField] AfterImageSettings _settings;
    [SerializeField] Rigidbody2D _rigidBody;

    static GameObject _globalAfterImageContainer;

    SpriteRenderer _spriteRenderer;
    GameObject _afterImageTemplate;
    ObjectPoolManager _objectPool;
    float _timer;

    public void Initialize()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _objectPool = ServiceLocator.instance.Get<ObjectPoolManager>();

        EnsureGlobalContainer();
        SetupAfterImagePrefab();
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
            SpawnAfterImage();
            _timer = 0f;
        }
    }

    void SpawnAfterImage()
    {
        var afterImage = _objectPool.TakeFromPool(
            _afterImageTemplate,
            transform.position,
            transform.rotation,
            _globalAfterImageContainer.transform
        );

        afterImage.transform.localScale = transform.lossyScale;

        afterImage.GetComponent<AfterImageController>()
                  .Activate(_settings, _spriteRenderer);
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
        _afterImageTemplate = new GameObject($"{name}_AfterImage_TEMPLATE");
        _afterImageTemplate.SetActive(false);
        _afterImageTemplate.transform.SetParent(_globalAfterImageContainer.transform);

        var spriteRenderer = _afterImageTemplate.AddComponent<SpriteRenderer>();
        _afterImageTemplate.AddComponent<AfterImageController>();

        spriteRenderer.material = _spriteRenderer.material;
        spriteRenderer.sortingLayerID = _spriteRenderer.sortingLayerID;
        spriteRenderer.sortingOrder = _spriteRenderer.sortingOrder - 1;
    }
}
