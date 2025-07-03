using UnityEngine;

public interface IInitializeable
{
    public GameObject gameObject { get; }
    public void Initialize();
}
