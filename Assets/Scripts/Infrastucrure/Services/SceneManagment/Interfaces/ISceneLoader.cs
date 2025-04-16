using Cysharp.Threading.Tasks;

public interface ISceneLoader
{
    UniTask LoadSceneAsync(string sceneName, bool useLoadingScreen = false);
}
