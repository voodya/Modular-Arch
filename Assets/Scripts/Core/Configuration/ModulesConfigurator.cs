using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class ModulesConfigurator
{
    private readonly List<IModule> _currentModules = new(); //TODO dictionary
    private IEnumerable<IModule> _allModules;

    public ModulesConfigurator(IEnumerable<IModule> modules)
    {
        _allModules = modules;
    }


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
            if (!module.IsInited.Value)
                await module.OnEnter();
        }
    }

    public async UniTask UpdateBodulesState()
    {
        foreach (var item in _allModules)
        {
            Debug.LogError($"All modules is {item.Name}");
        }
        foreach (var item in _currentModules)
        {
            Debug.LogError($"Current modules is {item.Name}");
        }
        foreach (var module in _allModules)
        {
            await module.OnEnter(_currentModules.Contains(module));
        }
    }
}
