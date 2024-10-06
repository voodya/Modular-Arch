using System;
using System.Collections.Generic;

public class RuntimeDataHolder : IRuntimeDataHolder
{

    public readonly Dictionary<Type, IModuleRuntimeData> _runtimeDatas = new();
    public void SetData<T>(T Data) where T : IModuleRuntimeData
    {
        _runtimeDatas[typeof(T)] = Data;
    }

    public bool TryGetData<T>(out T data) where T : IModuleRuntimeData
    {
        data = default(T);
        if (_runtimeDatas.ContainsKey(typeof(T)))
        {
            data = (T)_runtimeDatas[typeof(T)];
            return true;
        }
        else return false;
    }
}
