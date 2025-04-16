using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : ISceneLoader
{
    private const string LoadingSceneName = "Loading";

    public async UniTask LoadSceneAsync(string sceneName, bool useLoadingScreen = false)
    {
        if (useLoadingScreen)
        {
            await SceneManager.LoadSceneAsync(LoadingSceneName).ToUniTask();
            await UniTask.Delay(100);
        }

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName);
        await loadOp.ToUniTask();
    }
}
