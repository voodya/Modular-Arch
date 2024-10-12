using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ModuleView : MonoBehaviour
{
    [SerializeField] private Button _moduleInclude;
    [SerializeField] private GameObject _moduleIncludeImage;

    [SerializeField] private Button _moduleActive;
    [SerializeField] private GameObject _moduleActiveImage;

    [SerializeField] private GameObject _moduleInitedImage;

    [SerializeField] private TextMeshProUGUI _moduleName;

    private bool IsIncluded;

    private IReactiveProperty<bool> _currentState;
    private IModule _module;

    public Action<IModule> OnIncluded;
    public Action<IModule> OnExcluded;

    public void Configure(IModule currentModule)
    {
        _module = currentModule;
        _moduleName.text = _module.Name;
        _moduleActive.interactable = false;
        _moduleInclude.onClick.AddListener(SwichModuleInclude);
        _module.IsInited.Subscribe(UpdateInitedState).AddTo(this);
    }

    public void SetPausable()
    {
        _moduleActive.interactable = true;
        _moduleActive.onClick.AddListener(SwichModuleState);
        _module.IsActive.Subscribe(UpdateModuleState).AddTo(this);
    }

    private void SwichModuleInclude()
    {
        if (IsIncluded)
        {
            IsIncluded = false;
            _moduleIncludeImage.SetActive(false);
            OnExcluded?.Invoke(_module);
        }
        else
        {
            IsIncluded = true;
            _moduleIncludeImage.SetActive(true);
            OnIncluded?.Invoke(_module);
        }
    }

    public void ForceInclude()
    {
        IsIncluded = true;
        _moduleInclude.interactable = false;
        _moduleIncludeImage.SetActive(true);
        OnIncluded?.Invoke(_module);
    }

    private void SwichModuleState()
    {
        _module.OnPause(!_module.IsActive.Value);
    }

    private void UpdateInitedState(bool obj)
    {
        _moduleInitedImage.SetActive(obj);
    }

    private void UpdateModuleState(bool obj)
    {
        _moduleActiveImage.SetActive(obj);
    }
}
