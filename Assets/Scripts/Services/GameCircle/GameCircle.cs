using Cysharp.Threading.Tasks;
using Module.Character;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Voodya.VooAutoInject.VContainer;

public class GameCircle : VContainerMonoBehaviour, IGameCircle
{
    [SerializeField] private Button _generateLevel;
    [SerializeField] private ModuleView _viewPfb;
    [SerializeField] private Transform _parent;


    private List<ModuleView> ModuleViews = new();

    private readonly IList<Type> ReqiredTypes = new List<Type>
    {
        typeof(MapMeshGenerator),
        typeof(CharacterModule)
    };


    [Inject] private IEnumerable<IModule> _modules;
    private ModulesConfigurator _moduleConfigurator;
    public async UniTask Initialize()
    {
        _generateLevel.onClick.AddListener(GenerateGame);
        _moduleConfigurator = new ModulesConfigurator(_modules);
        foreach (var module in _modules)
        {
            ModuleView view = Instantiate(_viewPfb, _parent);
            view.Configure(module);
            view.OnIncluded += _moduleConfigurator.Register;
            view.OnExcluded += _moduleConfigurator.Release;
            if (ReqiredTypes.Contains(module.GetType()))
            {
                //view.ForceInclude();
            }
            ModuleViews.Add(view);
        }
    }

    private async void GenerateGame()
    {
        await _moduleConfigurator.UpdateBodulesState();
        //ModuleViews.ForEach(view => view.ForceInclude());
        ModuleViews.ForEach(view => view.SetPausable());
    }
}
