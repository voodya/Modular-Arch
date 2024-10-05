using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

public interface ISceneHolder
{

    public UniTask<T> GetScene<T>() where T : class, IScene;
    public void RegisterScene<T>(AssetReference reference) where T : IScene;
}
