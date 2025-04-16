using UnityEngine;
using Zenject;

public class Bootstrapper : IInitializable
{
    private const string GameplaySceneName = "Gameplay";

    private ISceneLoader _sceneLoader;
    [Inject]
    public void Init(ISceneLoader sceneLoader) 
        => _sceneLoader = sceneLoader;

    public async void Initialize()
    {
        Debug.Log("Bootstrapper: Initialization Started");
        await _sceneLoader.LoadSceneAsync(GameplaySceneName);
    }
}
 