
using System.Collections;
using UnityEngine;

public class AudioManagerBootstrapper : MonoBehaviour, IBootstrapper
{
    [SerializeField] GameObject _audioManagerPrefab;

    public IEnumerator BootstrapCoroutine()
    {
        var audioManager = Instantiate(_audioManagerPrefab);
        audioManager.name = "AudioManager";
        audioManager.GetComponent<IInitializeable>().Initialize();
        yield return null;
    }
}
