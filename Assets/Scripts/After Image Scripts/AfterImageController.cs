
using UnityEngine;
using DG.Tweening;
using DesignPatterns.ObjectPool;
using DesignPatterns.ServiceLocatorPattern;

[RequireComponent(typeof(SpriteRenderer))]
public class AfterImageController : MonoBehaviour
{
    AfterImageSettings _settings;
    SpriteRenderer _spriteRenderer;
    SpriteRenderer _emmiterSpriteRenderer;
    ObjectPoolManager _objectPool;
    Tween _fadeTween;

    /// <summary>
    /// Initialize or reinitialize the afterimage when spawned from pool
    /// </summary>
    public void Activate(AfterImageSettings settings, SpriteRenderer emitterSpriteRenderer)
    {
        _settings = settings;
        _objectPool = ServiceLocator.instance.Get<ObjectPoolManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _emmiterSpriteRenderer = emitterSpriteRenderer;
        UpdateSpriteRenderer();
        _fadeTween?.Kill();

        _fadeTween = _spriteRenderer
            .DOFade(0f, _settings.afterImageLifetime)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _objectPool.ReturnToPool(gameObject);
            });
    }

    void OnDisable()
    {
        _fadeTween?.Kill();
        _fadeTween = null;
    }

    void UpdateSpriteRenderer()
    {
        _spriteRenderer.sprite = _emmiterSpriteRenderer.sprite;
        _spriteRenderer.material = _settings.afterImageMaterial;
        _spriteRenderer.flipX = _emmiterSpriteRenderer.flipX;
        _spriteRenderer.flipY = _emmiterSpriteRenderer.flipY;
        _spriteRenderer.color = _emmiterSpriteRenderer.color;
        _spriteRenderer.sortingLayerID = _emmiterSpriteRenderer.sortingLayerID;
        _spriteRenderer.sortingOrder = _emmiterSpriteRenderer.sortingOrder - 1;

        var color = _spriteRenderer.color;
        color.a = _settings.afterImageStartAlpha;
        _spriteRenderer.color = color;
    }
}
