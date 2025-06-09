
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics.Grappling
{
    public class GrapplingRopeController : MonoBehaviour
    {
        List<List<RopeSegmentController>> _ropes = new();
        List<RopeSegmentController> _currentRope;

        RopeCreator _ropeCreator;
        GrapplingEventBus _grapplingEventBus;
        InputEventBus _inputEventBus;

        internal void Initialize(GrapplingRopeDependencies ropeDependencies, CommonGrapplingDependencies commonDependencies)
        {
            FetchDependencies();
            _ropeCreator.Initialize(ropeDependencies, commonDependencies);
            _grapplingEventBus.Subscribe<GrapplerAimedEvent>(OnGrapplerAimed);
            _inputEventBus.Subscribe<TouchEndedEvent>(OnTouchEnded);
        }

        void FetchDependencies()
        {
            _ropeCreator = GetComponent<RopeCreator>();
            _grapplingEventBus = ServiceLocator.instance.Get<GrapplingEventBus>();
            _inputEventBus = ServiceLocator.instance.Get<InputEventBus>();
        }

        void OnGrapplerAimed()
        {
            var newRope = _ropeCreator.CreateRope();
            _currentRope = newRope;
            _ropes.Add(newRope);
        }

        void OnTouchEnded()
        {

        }
    }
}