using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

[CreateAssetMenu(menuName = "Installers/Modules/Map", fileName = "MapGeneratorDatabase")]
public class MapGeneratorDatabase : InstallerSO
{
    [SerializeField] public AssetReference _groundBlockPfb;

    public override void Install(IContainerBuilder builder)
    {
        builder.RegisterInstance(this);
        builder.Register<MapMeshGenerator>(Lifetime.Singleton).As<IModule>();
    }
}


