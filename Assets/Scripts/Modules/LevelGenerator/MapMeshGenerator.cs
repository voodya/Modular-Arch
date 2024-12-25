using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using DG.Tweening;
using VContainer;
using System.Collections.Generic;


public class MapMeshGenerator : BaseModule
{
    private Vector2Int _mapSize = new Vector2Int(30, 30);
    private float[,] _mapRaw;
    private GameObject _mapMeshCell;
    private MapGeneratorDatabase _database;
    private MapGeneratorRuntimeData _runtimeData;
    private IRuntimeDataHolder _runtimeDataHolder;
    private FastRandom _fastRandom;

    [Inject]
    public MapMeshGenerator(
        MapGeneratorDatabase database,
        IRuntimeDataHolder runtimeDataHolder)
    {
        _runtimeDataHolder = runtimeDataHolder;
        _database = database;
        
    }

    public async override UniTask OnEnter(bool state)
    {
        if(state && !IsInited.Value)
        {
            _runtimeData = new MapGeneratorRuntimeData();
            _isActive = new ReactiveProperty<bool>(false);
            _mapMeshCell = await Addressables.LoadAssetAsync<GameObject>(_database._groundBlockPfb);
            _fastRandom = new FastRandom(_database._seed);
            await PrepareRawData();
            await InstanceObjects();
            _isActive.Value = true;
            _runtimeData.MapData = _mapRaw;
            _runtimeDataHolder.SetData(_runtimeData);
        }
        else if (!state)
        {
            _runtimeData?.ClearData();
        }
        await base.OnEnter(state);
    }

    public override async UniTask OnEnter()
    {
        _runtimeData = new MapGeneratorRuntimeData();
        _isActive = new ReactiveProperty<bool>(false);
        _mapMeshCell = await Addressables.LoadAssetAsync<GameObject>(_database._groundBlockPfb);
        _fastRandom = new FastRandom(_database._seed);
        await PrepareRawData();
        await InstanceObjects();
        _isActive.Value = true;
        _runtimeData.MapData = _mapRaw;
        _runtimeDataHolder.SetData(_runtimeData);
        await base.OnEnter();
        
    }

    private async UniTask InstanceObjects()
    {
        for (int i = 0; i < _mapSize.x; i++)
        {
            for (int j = 0; j < _mapSize.y; j++)
            {
                var obj = MonoBehaviour.Instantiate(_mapMeshCell);
                bool isHigh = _mapRaw[i, j] > 0.5f;
                float poseY = Round(_mapRaw[i, j], _database._heightStep);
                Vector3 pose = new Vector3(i, poseY, j);
                obj.transform.position = new Vector3(i, poseY, j);
                obj.transform.DOJump(pose, 1, 1, 0.5f);
                _runtimeData.Boxes.Add(obj);
                if (isHigh)
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
        float offset = _fastRandom.Range(0f, 1f);
        Debug.LogError(offset);

        for (int i = 0; i < _mapSize.x; i++)
        {
            for (int j = 0; j < _mapSize.y; j++)
            {
                _mapRaw[i, j] = Mathf.PerlinNoise(i * _database._noizeScale + offset, j * _database._noizeScale + offset);
            }
            await UniTask.Yield();
        }
    }

    public override int GetPriority()
    {
        return 2;
    }
}

[System.Serializable]
public class MapGeneratorRuntimeData : IModuleRuntimeData
{
    public float[,] MapData;
    public List<Vector2Int> LowPoints;
    public List<Vector2Int> HightPoints;
    public List<GameObject> Boxes;

    public MapGeneratorRuntimeData()
    {
        LowPoints = new List<Vector2Int>();
        HightPoints = new List<Vector2Int>();
        Boxes = new List<GameObject>();
    }

    public void ClearData()
    {
        for (var i = 0; i < Boxes.Count; i++)
        {
            MonoBehaviour.Destroy(Boxes[i]);
        }
        LowPoints.Clear();
        HightPoints.Clear();
    }
}

