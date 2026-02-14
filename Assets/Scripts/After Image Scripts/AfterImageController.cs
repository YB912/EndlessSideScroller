using UnityEngine;
using DG.Tweening;
using DesignPatterns.ObjectPool;
using DesignPatterns.ServiceLocatorPattern;

[RequireComponent(typeof(SpriteRenderer))]
public class AfterImageController : MonoBehaviour
{
    AfterImageSettings _settings;
    SpriteRenderer _spriteRenderer;
    SpriteRenderer _emitterSpriteRenderer;
    ObjectPoolManager _objectPool;

    Tween _fadeTween;

    public void Activate(AfterImageSettings settings, SpriteRenderer emitterSpriteRenderer)
    {
        _settings = settings;
        _emitterSpriteRenderer = emitterSpriteRenderer;

        _objectPool = ServiceLocator.instance.Get<ObjectPoolManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        ApplySnapshot();

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

    void ApplySnapshot()
    {
        _spriteRenderer.sprite = _emitterSpriteRenderer.sprite;
        _spriteRenderer.flipX = _emitterSpriteRenderer.flipX;
        _spriteRenderer.flipY = _emitterSpriteRenderer.flipY;

        _spriteRenderer.material = _settings.afterImageMaterial;
        _spriteRenderer.sortingLayerID = _emitterSpriteRenderer.sortingLayerID;
        _spriteRenderer.sortingOrder = _emitterSpriteRenderer.sortingOrder - 1;

        Color c = _emitterSpriteRenderer.color;
        c.a = _settings.afterImageStartAlpha;
        _spriteRenderer.color = c;
    }
}
