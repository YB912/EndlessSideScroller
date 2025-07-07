using UnityEngine;
using System.Collections.Generic;

namespace Mechanics.Grappling
{
    /// <summary>
    /// Controls the animated appearance (fade-in) of rope segments during grappling.
    /// </summary>
    public class RopeCreationAnimationController : MonoBehaviour
    {
        float _totalFadeInDuration;
        AnimationCurve _fadeInRateCurve;

        bool _shouldFade = false;
        float _fadeTimer = 0f;
        int _currentSegmentIndex = 0;

        List<RopeSegmentController> _rope;
        List<float> _fadeIntervals;

        internal void Initialize(RopeAnimationDependencies fadeInDependencies)
        {
            _totalFadeInDuration = fadeInDependencies.totalAnimationDuration;
            _fadeInRateCurve = fadeInDependencies.animationCurve;
        }

        /// <summary>
        /// Prepares rope fade-in data and activates animation sequence.
        /// </summary>
        public void AnimateRopeCreation(List<RopeSegmentController> rope)
        {
            _rope = rope;
            _fadeIntervals = new List<float>();
            _fadeTimer = 0f;
            _currentSegmentIndex = 0;

            int segmentCount = rope.Count;
            float weightSum = 0f;
            List<float> weights = new List<float>();

            for (int i = 0; i < segmentCount; i++)
            {
                float t = segmentCount == 1 ? 0.5f : (float)i / (segmentCount - 1);
                float weight = _fadeInRateCurve.Evaluate(t);
                weights.Add(weight);
                weightSum += weight;
            }

            for (int i = 0; i < segmentCount; i++)
            {
                float normalizedInterval = (weights[i] / weightSum) * _totalFadeInDuration;
                _fadeIntervals.Add(normalizedInterval);
            }

            _shouldFade = true;
        }

        void FixedUpdate()
        {
            if (_shouldFade == false || _rope == null || _currentSegmentIndex >= _rope.Count)
                return;

            _fadeTimer += Time.fixedDeltaTime;

            while (_currentSegmentIndex < _rope.Count && _fadeTimer >= _fadeIntervals[_currentSegmentIndex])
            {
                _rope[_currentSegmentIndex].FadeIn(_fadeIntervals[_currentSegmentIndex]);
                _fadeTimer -= _fadeIntervals[_currentSegmentIndex];
                _currentSegmentIndex++;
            }

            if (_currentSegmentIndex >= _rope.Count)
            {
                _shouldFade = false;
                _rope = null;
                _fadeIntervals = null;
            }
        }
    }
}
