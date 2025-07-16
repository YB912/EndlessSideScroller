
using DesignPatterns.ServiceLocatorPattern;
using Mechanics.CourseGeneration;
using System.Collections;
using UnityEngine;

public class PlayerRagdollBootstrapper : MonoBehaviour, IBootstrapper
{
    [SerializeField] GameObject _ragdollPrefab;

    GameObject _player;

    const string PLAYER_GAMEOBJECT_NAME = "Player";

    public IEnumerator BootstrapCoroutine()
    {
        CreatePlayer();
        var playerController = _player.GetComponent<PlayerController>();
        ServiceLocator.instance.Register(playerController);
        playerController.Initialize();
        yield break;
    }

    void CreatePlayer()
    {
        var position = ServiceLocator.instance.Get<CourseGenerationManager>().playerSpawnPoint;
        _player = Instantiate(_ragdollPrefab, position, Quaternion.identity);
        _player.name = PLAYER_GAMEOBJECT_NAME;
    }
}
