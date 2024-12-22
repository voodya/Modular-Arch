using Cysharp.Threading.Tasks;
using Module.Attcker;
using UnityEngine;
using VContainer;

public class DebugModule : BaseModule
{
    private IRuntimeDataHolder _runtimeDataHolder;
    private DebugModuleDatabase _database;

    [Inject]
    public DebugModule(IRuntimeDataHolder runtimeDataHolder, DebugModuleDatabase database)
    {
        _runtimeDataHolder = runtimeDataHolder;
        _database = database;
    }

    public override int GetPriority()
    {
        return 100;
    }

    public override UniTask OnEnter()
    {
        _isInited.Value = true;
#if UNITY_EDITOR
        if (_runtimeDataHolder.TryGetData(out MapGeneratorRuntimeData data))
        {
            MonoBehaviour.Instantiate(_database.GizmosDrower).Configure(data);
        }
        if (_runtimeDataHolder.TryGetData(out AttakerModuleRuntimeData AtData))
        {
            MonoBehaviour.Instantiate(_database.AttakerDrower).Configure(AtData);
        }
        _isActive.Value = true;
#endif
        return UniTask.CompletedTask;
    }


}
