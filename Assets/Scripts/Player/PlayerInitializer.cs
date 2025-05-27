using System.Linq;
using UnityEngine;

public class PlayerInitializer : MonoBehaviour, IInitializeable
{
    public void Initialize()
    {
        var initializeables = GetComponentsInChildren<IInitializeable>();
        foreach (var initializeable in initializeables.Where(i => i.gameObject != gameObject))
        {
            initializeable.Initialize();
        }
    }
}
