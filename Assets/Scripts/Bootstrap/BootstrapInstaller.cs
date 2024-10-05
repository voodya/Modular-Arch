using VContainer;
using VContainer.Unity;

public class BootstrapInstaller : SceneInstaller
{
    public override void Install(IContainerBuilder builder)
    {
        base.Install(builder);
        builder.RegisterEntryPoint<EntryPoint>();
    }
}
