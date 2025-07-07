using DesignPatterns.ServiceLocatorPattern;
using System.Collections;
using UnityEngine;

public class PlayerRagdollBootstrapper : MonoBehaviour, IBootstrapper
{
    [SerializeField] GameObject _ragdollPrefab;

    const string PLAYER_GAMEOBJECT_NAME = "Player";

    public IEnumerator BootstrapCoroutine()
    {
        var ragdoll = Instantiate(_ragdollPrefab);
        ragdoll.name = PLAYER_GAMEOBJECT_NAME;
        var playerController = ragdoll.GetComponent<PlayerController>();
        ServiceLocator.instance.Register(playerController);
        playerController.Initialize();
        yield break;
    }
}
