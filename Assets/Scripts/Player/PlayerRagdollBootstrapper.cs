using System.Collections;
using UnityEngine;

public class PlayerRagdollBootstrapper : MonoBehaviour, ISceneBootstrappable
{
    [SerializeField] GameObject _ragdollPrefab;

    const string PLAYER_GAMEOBJECT_NAME = "Player";

    public IEnumerator BootstrapCoroutine()
    {
        var ragdoll = Instantiate(_ragdollPrefab);
        ragdoll.name = PLAYER_GAMEOBJECT_NAME;
        ragdoll.GetComponent<IInitializeable>().Initialize();
        yield break;
    }
}
