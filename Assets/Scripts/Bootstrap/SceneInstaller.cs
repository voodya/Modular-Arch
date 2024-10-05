using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class SceneInstaller : LifetimeScope
{
    [Header("Scriptable Installers")]
    [SerializeField] private List<InstallerSO> _mainInstallers;
    [SerializeField] private List<InstallerSO> _testModeInstallers;

    [Header("Scene Objects")]
    [SerializeField] private GameObject _testModeObjects;

    private IDisposable _globalParentOverride;

    public virtual void Install(IContainerBuilder builder) { }
    public virtual void TestModeInstall(IContainerBuilder builder) { }
    public virtual void Initialize() { }
    public virtual void TestModeInitialize() { }
    protected override void Awake()
    {
        base.Awake();

        Initialize();
        if (Parent == null)
        {
            TestModeInitialize();
            Container.InjectGameObject(_testModeObjects);
            _globalParentOverride = EnqueueParent(this);
        }
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        _globalParentOverride?.Dispose();
    }

    protected override void Configure(IContainerBuilder builder)
    {
        EntryPointsBuilder.EnsureDispatcherRegistered(builder);
        Install(builder);
        foreach (var installer in _mainInstallers)
        {
            installer.Install(builder);
        }
        if (Parent == null)
        {
            TestModeInstall(builder);
            foreach (var installer in _testModeInstallers)
            {
                installer.Install(builder);
            }
            if (_testModeObjects != null) _testModeObjects?.SetActive(true);
        }
        else
        {
            if (_testModeObjects != null) _testModeObjects?.SetActive(false);
        }
    }
}
