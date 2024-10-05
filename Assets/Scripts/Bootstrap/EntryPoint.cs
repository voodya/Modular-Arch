using Cysharp.Threading.Tasks;
using System.Threading;
using VContainer;
using VContainer.Unity;

public class EntryPoint : IAsyncStartable
{
    readonly ISceneHolder _sceneHolder;

    [Inject]
    public EntryPoint(ISceneHolder sceneHolder)
    {
        _sceneHolder = sceneHolder;
    }

    public async UniTask StartAsync(CancellationToken cancellation = default)
    {
        ILoadScreenService service = await _sceneHolder.GetScene<ILoadScreenService>();
        service.StartLoad();
    }
}
