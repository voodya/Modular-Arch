using System;
using System.Collections.Generic;

public class RuntimeDataHolder : IRuntimeDataHolder
{
    public Dictionary<string, List<Entity>> _hashedEntitys;

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

    public void AddEntity(Entity entity, string hash)
    {
        if (_hashedEntitys.ContainsKey(hash))
        {
            _hashedEntitys[hash].Add(entity);
        }
        else
            _hashedEntitys.Add(hash, new List<Entity>() { entity });
    }

    public void AddEntityRange(List<Entity> entity, string hash)
    {
        if (_hashedEntitys.ContainsKey(hash))
        {
            _hashedEntitys[hash].AddRange(entity);
        }
        else
            _hashedEntitys.Add(hash, new List<Entity>(entity));
    }


}

public interface IEntity
{


}


public struct Entity : IEntity
{
    
}


