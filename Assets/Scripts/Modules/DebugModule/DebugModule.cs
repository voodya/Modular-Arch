using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class DebugModule : BaseModule
{
    private IRuntimeDataHolder _runtimeDataHolder;
    private MapGeneratorRuntimeData _mapGeneratorRuntimeData;
    private DebugModuleDatabase _database;

    [Inject]
    public DebugModule(IRuntimeDataHolder runtimeDataHolder, DebugModuleDatabase database)
    {
        _runtimeDataHolder = runtimeDataHolder;
        _database = database;
    }


    public override UniTask OnEnter()
    {
        _isInited.Value = true;
#if UNITY_EDITOR
        if (_runtimeDataHolder.TryGetData(out MapGeneratorRuntimeData data))
        {
            _mapGeneratorRuntimeData = data;
            MonoBehaviour.Instantiate(_database.GizmosDrower).Configure(_mapGeneratorRuntimeData);
            _isActive.Value = true;
        }
#endif
        return UniTask.CompletedTask;
    }


}
