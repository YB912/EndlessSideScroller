
using System.Collections;
using UnityEngine;

public class ScoreManagerBootstrapper : MonoBehaviour, IBootstrapper
{
    [SerializeField] GameObject _scoreManagerPrefab;
    public IEnumerator BootstrapCoroutine()
    {
        var scoreManager = Instantiate(_scoreManagerPrefab);
        scoreManager.name = "ScoreManager";
        scoreManager.GetComponent<IInitializeable>().Initialize();
        yield break;
    }
}
