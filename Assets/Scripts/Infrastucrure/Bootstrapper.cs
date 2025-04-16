using UnityEngine;
using Zenject;

public class Bootstrapper : IInitializable
{
    public void Initialize()
    {
        Debug.Log("GameBootstrapper: Initialization Started");
    }
}
