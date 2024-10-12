using Cysharp.Threading.Tasks;
using UniRx;

public abstract class BaseModule : IModule
{
    protected ReactiveProperty<bool> _isActive = new ReactiveProperty<bool>(false);
    protected ReactiveProperty<bool> _isInited = new ReactiveProperty<bool>(false);
    protected CompositeDisposable _compositeDisposable;

    public IReactiveProperty<bool> IsActive => _isActive;

    public IReactiveProperty<bool> IsInited => _isInited;

    public string Name => GetType().ToString();

    public int Priority => GetPriority();

    public abstract int GetPriority();


    public virtual UniTask OnEnter()
    {
        _isActive.Value = true;
        _isInited.Value = true;
        return UniTask.CompletedTask;
    }

    public virtual UniTask OnExit()
    {
        _compositeDisposable?.Dispose();
        return UniTask.CompletedTask;
    }

    public virtual UniTask OnPause(bool pause)
    {
        _isActive.Value = pause;
        return UniTask.CompletedTask;
    }
}
