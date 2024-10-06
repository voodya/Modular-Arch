using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class GameCircle : MonoBehaviour, IGameCircle
{
    private ModulesConfigurator configurator;


    [Inject]
    private IEnumerable<IModule> _modules;
    public async UniTask Initialize()
    {
        configurator = new ModulesConfigurator();
        foreach (var module in _modules)
        {
            configurator.Register(module);
        }
        await configurator.ConfigureModules();
    }
}
