using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using VContainer;

public class CharacterModule : BaseModule
{
    private CharacterDatabase _characterDatabase;
    private CharacterView _characterView;
    private IRuntimeDataHolder _runtimeDataHolder;
    private Vector2Int _position;
    private CharacterRuntimeData _characterRuntimeData;

    [Inject]
    public CharacterModule(
        CharacterDatabase characterDatabase,
        IRuntimeDataHolder runtimeDataHolder)
    {
        _runtimeDataHolder = runtimeDataHolder;
        _characterDatabase = characterDatabase;
    }

    public override int GetPriority()
    {
        return 1;
    }

    public override async UniTask OnEnter()
    {
        await UniTask.Yield();

        if (!_runtimeDataHolder.TryGetData(out MapGeneratorRuntimeData data)) return;
        _isActive.Value = true;
        _isInited.Value = true;
        _compositeDisposable = new CompositeDisposable();
        _characterRuntimeData = new CharacterRuntimeData();
        _position = data.LowPoints[Random.Range(0, data.LowPoints.Count)];
        data.LowPoints.Remove(_position);
        _characterView = MonoBehaviour.Instantiate(_characterDatabase.CharacterPfb);
        _characterView.transform.position = new(_position.x, 1f, _position.y);
        _characterRuntimeData.CharacterInstance = _characterView;
        Observable.EveryFixedUpdate().Subscribe(CharacterMove).AddTo(_compositeDisposable);
        _runtimeDataHolder.SetData(_characterRuntimeData);
    }

    private void CharacterMove(long obj)
    {
        _characterView.Rb.velocity = Vector3.zero;

        Vector3 down = Vector3.Project(_characterView.Rb.velocity, _characterView.transform.up);
        Vector3 forward = _characterView.transform.forward * Input.GetAxis("Vertical") * 200 * Time.fixedDeltaTime;
        Vector3 right = _characterView.transform.right * Input.GetAxis("Horizontal") * 200 * Time.fixedDeltaTime;

        _characterView.Rb.velocity = right + forward + down;
        Vector3 up = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.Space))
            _characterView.Rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);


    }
}

[System.Serializable]
public class CharacterRuntimeData : IModuleRuntimeData
{
    public CharacterView CharacterInstance;
}
