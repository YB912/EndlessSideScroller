
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Mechanics.Grappling
{
    /// <summary>
    /// Controls the animated appearance (fade-in) of rope segments during grappling.
    /// </summary>
    public class RopeCreationAnimationController : MonoBehaviour
    {
        float _totalFadeInDuration;
        AnimationCurve _fadeInRateCurve;

        internal void Initialize(RopeAnimationDependencies fadeInDependencies)
        {
            _totalFadeInDuration = fadeInDependencies.totalAnimationDuration;
            _fadeInRateCurve = fadeInDependencies.animationCurve;
        }

        /// <summary>
        /// Triggers coroutine to animate rope segments one-by-one.
        /// </summary>
        public void AnimateRopeCreation(List<RopeSegmentController> rope)
        {
            StartCoroutine(AnimationCoroutine(rope));
        }

        IEnumerator AnimationCoroutine(List<RopeSegmentController> rope)
        {
            int segmentCount = rope.Count;
            var weights = new List<float>();
            float weightSum = 0f;

            // Evaluate normalized weights from curve based on position in rope
            for (int i = 0; i < segmentCount; i++)
            {
                float t = segmentCount == 1 ? 0.5f : (float)i / (segmentCount - 1);
                float weight = _fadeInRateCurve.Evaluate(t);
                weights.Add(weight);
                weightSum += weight;
            }

            // Fade in each segment using weighted delay
            for (int i = 0; i < segmentCount; i++)
            {
                float normalizedInterval = (weights[i] / weightSum) * _totalFadeInDuration;
                rope[i].FadeIn(normalizedInterval);
                yield return new WaitForSeconds(normalizedInterval);
            }
        }
    }
}
