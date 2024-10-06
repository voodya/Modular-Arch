using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using VContainer;

public class CharacterController : IModule
{
    private CharacterDatabase _characterDatabase;
    private string _moduleName = "CharacterController";
    private ReactiveProperty<bool> _isActive;

    private IRuntimeDataHolder _runtimeDataHolder;

    [Inject]
    public CharacterController(
        CharacterDatabase characterDatabase,
        IRuntimeDataHolder runtimeDataHolder)
    {
        _runtimeDataHolder = runtimeDataHolder;
        _characterDatabase = characterDatabase;
    }

    public string Name => _moduleName;

    public IReactiveProperty<bool> IsActive => _isActive;

    public async UniTask OnEnter()
    {
        await UniTask.Yield();
        if(_runtimeDataHolder.TryGetData(out MapGeneratorRuntimeData data))
        Debug.LogError($"runtimeDataHolder map height = {data.MapData[0,0]}");
    }

    public UniTask OnExit()
    {
        throw new System.NotImplementedException();
    }

    public UniTask OnPause()
    {
        throw new System.NotImplementedException();
    }
}
