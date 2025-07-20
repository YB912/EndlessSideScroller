
using UnityEngine;
using System.Collections;

public class GameManagerBootstrapper : MonoBehaviour, IBootstrapper
{
    [SerializeField] GameObject _gameManagerPrefab;
    public IEnumerator BootstrapCoroutine()
    {
        var gameManager = Instantiate(_gameManagerPrefab);
        gameManager.GetComponent<IInitializeable>().Initialize();
        yield break;
    }
}
