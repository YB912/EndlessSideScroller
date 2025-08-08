using UnityEngine;
using System.Collections.Generic;

namespace Player
{
public partial class PlayerController
    {
        /// <summary>
        /// Structurally defines the key rigid body parts of the player used in physics and animation.
        /// </summary>
        [System.Serializable]
        public class PlayerBodyParts
        {
            [SerializeField] GameObject _head;
            [SerializeField] GameObject _chest;
            [SerializeField] GameObject _abdomen;
            [SerializeField] GameObject _backUpperArm;
            [SerializeField] GameObject _backForearm;
            [SerializeField] GameObject _frontUpperArm;
            [SerializeField] GameObject _frontForearm;
            [SerializeField] GameObject _backUpperLeg;
            [SerializeField] GameObject _backLowerLeg;
            [SerializeField] GameObject _backFoot;
            [SerializeField] GameObject _frontUpperLeg;
            [SerializeField] GameObject _frontLowerLeg;
            [SerializeField] GameObject _frontFoot;

            List<GameObject> _allParts = new();

            public GameObject head => _head;
            public GameObject chest => _chest;
            public GameObject abdomen => _abdomen;
            public GameObject backUpperArm => _backUpperArm;
            public GameObject backForearm => _backForearm;
            public GameObject frontUpperArm => _frontUpperArm;
            public GameObject frontForearm => _frontForearm;
            public GameObject backUpperLeg => _backUpperLeg;
            public GameObject backLowerLeg => _backLowerLeg;
            public GameObject backFoot => _backFoot;
            public GameObject frontUpperLeg => _frontUpperLeg;
            public GameObject frontLowerLeg => _frontLowerLeg;
            public GameObject frontFoot => _frontFoot;

            public List<GameObject> allParts
            {
                get
                {
                    if (_allParts.Count == 0)
                    {
                        InitializeAllParts();
                    }
                    return _allParts;
                }
            }

            void InitializeAllParts()
            {
                _allParts.Add(_head);
                _allParts.Add(_chest);
                _allParts.Add(_abdomen);
                _allParts.Add(_backUpperArm);
                _allParts.Add(_backForearm);
                _allParts.Add(_frontUpperArm);
                _allParts.Add(_frontForearm);
                _allParts.Add(_backUpperLeg);
                _allParts.Add(_backLowerLeg);
                _allParts.Add(_backFoot);
                _allParts.Add(_frontUpperLeg);
                _allParts.Add(_frontLowerLeg);
                _allParts.Add(_frontFoot);
            }
        }
    }
}
