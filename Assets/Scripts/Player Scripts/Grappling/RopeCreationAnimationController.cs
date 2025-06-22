
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Mechanics.Grappling
{
    public class RopeCreationAnimationController : MonoBehaviour
    {
        float _totalFadeInDuration;
        AnimationCurve _fadeInRateCurve;

        internal void Initialize(RopeAnimationDependencies fadeInDependencies)
        {
            _totalFadeInDuration = fadeInDependencies.totalAnimationDuration;
            _fadeInRateCurve = fadeInDependencies.animationCurve;
        }

        public void AnimateRopeCreation(List<RopeSegmentController> rope)
        {
            StartCoroutine(AnimationCoroutine(rope));
        }

        IEnumerator AnimationCoroutine(List<RopeSegmentController> rope)
        {
            int segmentCount = rope.Count;
            var weights = new List<float>();
            float weightSum = 0f;

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
                rope[i].FadeIn(normalizedInterval);
                yield return new WaitForSeconds(normalizedInterval);
            }
        }
    }
}
