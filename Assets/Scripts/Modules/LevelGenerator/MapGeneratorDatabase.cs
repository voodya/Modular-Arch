using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

[CreateAssetMenu(menuName = "Installers/Modules/Map", fileName = "MapGeneratorDatabase")]
public class MapGeneratorDatabase : InstallerSO
{
    [SerializeField] public AssetReference _groundBlockPfb;
    [SerializeField] public float _heightStep;
    [SerializeField] public float _noizeScale;
    [SerializeField] public int _seed;

    public override void Install(IContainerBuilder builder)
    {
        builder.RegisterInstance(this);
        builder.Register<MapMeshGenerator>(Lifetime.Singleton).As<IModule>();
    }
}



