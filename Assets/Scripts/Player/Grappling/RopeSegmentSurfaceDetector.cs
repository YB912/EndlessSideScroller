
using DesignPatterns.ObserverPattern;
using UnityEngine;

namespace Mechanics.Grappling
{
    public class RopeSegmentSurfaceDetector : MonoBehaviour
    {
        static string GRAPPLEABLE_TAG = "GrappleableSurface";

        // Need to be able to reset this for the next rope
        internal Observable<bool> hasHitAGrappleableSurfaceObservable = new Observable<bool>(false);

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(GRAPPLEABLE_TAG))
            {
                hasHitAGrappleableSurfaceObservable.Set(true);
            }
        }
    }
}
