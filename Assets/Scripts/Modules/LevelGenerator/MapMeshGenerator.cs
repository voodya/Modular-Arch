using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using DG.Tweening;
using VContainer;
using System.Collections.Generic;

public class MapMeshGenerator : IModule
{
    private Vector2Int _mapSize = new Vector2Int(30, 30);
    private float _noizeScale = 0.2f;
    private float[,] _mapRaw;
    private GameObject _mapMeshCell;
    private ReactiveProperty<bool> _isActive;
    private string _moduleName = "Base Map Generator";

    public IReactiveProperty<bool> IsActive => _isActive;
    public string Name => _moduleName;

    private MapGeneratorDatabase _database;
    private MapGeneratorRuntimeData _runtimeData;

    private IRuntimeDataHolder _runtimeDataHolder;

    [Inject]
    public MapMeshGenerator(
        MapGeneratorDatabase database,
        IRuntimeDataHolder runtimeDataHolder)
    {
        _runtimeDataHolder = runtimeDataHolder;
        _database = database;
    }

    public async UniTask OnEnter()
    {
        _runtimeData = new MapGeneratorRuntimeData();
        _isActive = new ReactiveProperty<bool>(false);
        _mapMeshCell = await Addressables.LoadAssetAsync<GameObject>(_database._groundBlockPfb);
        await PrepareRawData();
        await InstanceObjects();
        _isActive.Value = true;
        _runtimeData.MapData = _mapRaw;
        _runtimeDataHolder.SetData(_runtimeData);
    }

    private async UniTask InstanceObjects()
    {
        for (int i = 0; i < _mapSize.x; i++)
        {
            for (int j = 0; j < _mapSize.y; j++)
            {
                var obj = MonoBehaviour.Instantiate(_mapMeshCell);
                bool isHigh = _mapRaw[i, j] > 0.5f;
                float poseY = Round(_mapRaw[i, j], 0.25f);
                Vector3 pose = new Vector3(i, poseY, j);
                obj.transform.position = new Vector3(i, poseY, j);
                obj.transform.DOJump(pose, 1, 1, 0.5f);
                if(isHigh)
                {
                    _runtimeData.HightPoints.Add(new(i, j));
                }
                else
                    _runtimeData.LowPoints.Add(new(i, j));

            }
            await UniTask.Yield();
        }
    }

    private float Round(float value, float step) => (value - (value%step));

    private async UniTask PrepareRawData()
    {
        _mapRaw = new float[_mapSize.x, _mapSize.y];

        for (int i = 0; i < _mapSize.x; i++)
        {
            for (int j = 0; j < _mapSize.y; j++)
            {
                _mapRaw[i, j] = Mathf.PerlinNoise(i * _noizeScale, j * _noizeScale);
            }
            await UniTask.Yield();
        }
    }

    public UniTask OnExit()
    {
        throw new System.NotImplementedException();
    }

    public UniTask OnPause()
    {
        throw new System.NotImplementedException();
    }
}

[System.Serializable]
public class MapGeneratorRuntimeData : IModuleRuntimeData
{
    public float[,] MapData;
    public List<Vector2Int> LowPoints;
    public List<Vector2Int> HightPoints;

    public MapGeneratorRuntimeData()
    {
        LowPoints = new List<Vector2Int>();
        HightPoints = new List<Vector2Int>();
    }
}

