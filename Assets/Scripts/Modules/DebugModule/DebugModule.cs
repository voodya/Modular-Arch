using Cysharp.Threading.Tasks;
using Module.Attcker;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class DebugModule : BaseModule
{
    private IRuntimeDataHolder _runtimeDataHolder;
    private DebugModuleDatabase _database;
    private DebugRuntimeData _data;

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

    public override UniTask OnEnter(bool state)
    {
        if(state && !IsInited.Value)
        {
#if UNITY_EDITOR

            _data = new DebugRuntimeData();

            if (_runtimeDataHolder.TryGetData(out MapGeneratorRuntimeData data))
            {
                _data.DebugObjects.Add(MonoBehaviour.Instantiate(_database.GizmosDrower).Configure(data));
            }
            if (_runtimeDataHolder.TryGetData(out AttakerModuleRuntimeData AtData))
            {
                _data.DebugObjects.Add(MonoBehaviour.Instantiate(_database.AttakerDrower).Configure(AtData));
            }
            _isActive.Value = true;
#endif
            return UniTask.CompletedTask;
        }
        else if (!state)
        {
            _data?.ClearData();
        }
        
        return base.OnEnter(state);
    }

    public override UniTask OnEnter()
    {
        return base.OnEnter();
    }


}

[System.Serializable]
public class DebugRuntimeData : IModuleRuntimeData
{
    public List<GameObject> DebugObjects;

    public DebugRuntimeData()
    {
        DebugObjects = new List<GameObject>();
    }

    public void ClearData()
    {
        for (var i = 0; i < DebugObjects.Count; i++)
        {
            MonoBehaviour.Destroy(DebugObjects[i]);
        }
        DebugObjects.Clear();
    }
}

