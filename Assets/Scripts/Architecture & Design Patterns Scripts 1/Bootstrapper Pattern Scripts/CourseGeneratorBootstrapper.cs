using System.Collections;
using UnityEngine;

public class CourseGeneratorBootstrapper : MonoBehaviour, IBootstrapper
{
    [SerializeField] GameObject _courseGeneratorPrefab;

    public IEnumerator BootstrapCoroutine()
    {
        var initializeable = Instantiate(_courseGeneratorPrefab).GetComponent<IInitializeable>();
        initializeable.Initialize();
        yield break;
    }
}
