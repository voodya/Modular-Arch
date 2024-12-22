using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using UObject = UnityEngine.Object;

[CreateAssetMenu(menuName = "Installers/SceneHolder", fileName = "SceneHolder")]
public class SceneHolderInstaller : InstallerSO
{
    [SerializeField] private AssetReference _loadingScreenScene;
    [SerializeField] private AssetReference _gameCircleScene;

    public override void Install(IContainerBuilder builder)
    {
        SceneHolder holder = new SceneHolder();
        holder.RegisterScene<ILoadScreenService>(_loadingScreenScene);
        holder.RegisterScene<IGameCircle>(_gameCircleScene);
        builder.RegisterInstance<ISceneHolder>(holder);
    }
}
