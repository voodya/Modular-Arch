using System.Collections.Generic;
using UnityEngine;
using VContainer;

[CreateAssetMenu(menuName = "Installers/Combined", fileName = "CombinedInstaller")]
public class CombinedInstallerSO : InstallerSO
{
    [SerializeField] private List<InstallerSO> _installers;

    public override void Install(IContainerBuilder builder) => _installers.ForEach(SO => SO.Install(builder));
}
