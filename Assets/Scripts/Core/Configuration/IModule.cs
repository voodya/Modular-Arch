using Cysharp.Threading.Tasks;
using UniRx;

public interface IModule
{
    public int Priority { get; }
    public string Name { get; }
    public IReactiveProperty<bool> IsActive { get;} 
    public IReactiveProperty<bool> IsInited { get;} 

    public UniTask OnEnter();
    public UniTask OnEnter(bool state);
    public UniTask OnExit();
    public UniTask OnPause(bool pause);
}
