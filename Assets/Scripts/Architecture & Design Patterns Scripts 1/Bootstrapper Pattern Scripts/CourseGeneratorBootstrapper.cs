
using System.Collections;
using UnityEngine;

public class CourseGeneratorBootstrapper : MonoBehaviour, IBootstrapper
{
    [SerializeField] GameObject _courseGeneratorPrefab;

    public IEnumerator BootstrapCoroutine()
    {
        var courseGenerator = Instantiate(_courseGeneratorPrefab);
        courseGenerator.name = "CourseGenerator";
        courseGenerator.GetComponent<IInitializeable>().Initialize();
        yield break;
    }
}
