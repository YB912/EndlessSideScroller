
using System.Linq;
using UnityEngine;

public class MainCameraController : MonoBehaviour, IInitializeable
{
    public void Initialize()
    {
        var initializeables = GetComponentsInChildren<IInitializeable>();
        foreach (var initializeable in initializeables.Where(i => i != (IInitializeable)this))
        {
            initializeable.Initialize();
        }
    }
}
