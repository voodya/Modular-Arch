using UnityEngine;
using VContainer;


[CreateAssetMenu(menuName = "Installers/Modules/Debug", fileName = "DebugModuleDatabase")]
public class DebugModuleDatabase : InstallerSO
{
    [SerializeField] public GizmosDrower GizmosDrower;
    [SerializeField] public AttackerDrower AttakerDrower;

    public override void Install(IContainerBuilder builder)
    {
        builder.RegisterInstance(this);
        builder.Register<DebugModule>(Lifetime.Singleton).As<IModule>();
    }


}
