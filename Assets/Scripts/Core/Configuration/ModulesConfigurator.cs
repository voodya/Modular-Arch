using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class ModulesConfigurator : MonoBehaviour
{
    private readonly List<IModule> _currentModules = new(); //TODO dictionary

    public void Register(IModule module)
    {
        if (!_currentModules.Contains(module))
            _currentModules.Add(module);
        else
            Debug.LogError("Alredy added");
    }

    public void Release(IModule module)
    {
        if (_currentModules.Contains(module))
            _currentModules.Remove(module);
        else
            Debug.LogError("Alrady released");
    }

    public async UniTask ConfigureModules()
    {
        foreach (var module in _currentModules)
        {
            if(!module.IsInited.Value)
            await module.OnEnter();
        }
    }
}
