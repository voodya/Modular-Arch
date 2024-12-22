using Cysharp.Threading.Tasks;
using Module.Character;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class GameCircle : MonoBehaviour, IGameCircle
{
    [SerializeField] private Button _generateLevel;
    [SerializeField] private ModuleView _viewPfb;
    [SerializeField] private Transform _parent;


    private List<ModuleView> ModuleViews = new();
    private ModulesConfigurator configurator;

    private readonly IList<Type> ReqiredTypes = new List<Type>
    {
        typeof(MapMeshGenerator),
        typeof(CharacterModule)
    };


    [Inject]
    private IEnumerable<IModule> _modules;
    public async UniTask Initialize()
    {
        _generateLevel.onClick.AddListener(GenerateGame);


        configurator = new ModulesConfigurator();
        foreach (var module in _modules)
        {
            ModuleView view = Instantiate(_viewPfb, _parent);
            view.Configure(module);
            view.OnIncluded += configurator.Register;
            view.OnExcluded += configurator.Release;
            if (ReqiredTypes.Contains(module.GetType()))
            {
                view.ForceInclude();
            }
            ModuleViews.Add(view);
        }
    }

    private async void GenerateGame()
    {
        await configurator.ConfigureModules();
        ModuleViews.ForEach(view => view.ForceInclude());
        ModuleViews.ForEach(view => view.SetPausable());
    }
}
