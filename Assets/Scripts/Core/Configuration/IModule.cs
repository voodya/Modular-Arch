using Cysharp.Threading.Tasks;
using UniRx;

public interface IModule
{
    public string Name { get; }
    public IReactiveProperty<bool> IsActive { get;}

    public UniTask OnEnter();
    public UniTask OnExit();
    public UniTask OnPause();
}
