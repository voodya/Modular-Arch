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
            Debug.LogError($"Alredy added");
    }
    public async UniTask ConfigureModules()
    {
        foreach (var module in _currentModules)
        {
            await module.OnEnter();
        }
    }
}
