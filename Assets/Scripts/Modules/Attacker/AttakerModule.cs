using Cysharp.Threading.Tasks;
using Module.Character;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

using URandom = UnityEngine.Random;

namespace Module.Attcker
{
    public class AttakerModule : BaseModule
    {
        IRuntimeDataHolder _runtimeDataHolder;
        AttakerModuleDatabase _database;
        AttakerModuleRuntimeData _runtimeData;

        [Inject]
        public AttakerModule(IRuntimeDataHolder runtimeDataHolder, AttakerModuleDatabase database)
        {
            _runtimeDataHolder = runtimeDataHolder;
            _database = database;
        }


        public override int GetPriority()
        {
            return 10;
        }

        public override UniTask OnEnter(bool state)
        {
            if(state && !IsInited.Value)
            {
                _compositeDisposable = new CompositeDisposable();
                _runtimeData = new AttakerModuleRuntimeData();
                if (_runtimeDataHolder.TryGetData(out MapGeneratorRuntimeData data))
                {
                    for (int i = 0; i < _database.AttakeCount; i++)
                    {
                        var position = data.HightPoints[URandom.Range(0, data.HightPoints.Count)];
                        data.HightPoints.Remove(position);
                        var attakerObj = MonoBehaviour.Instantiate(_database.ViewPfbs[URandom.Range(0, _database.ViewPfbs.Count)]);
                        attakerObj.transform.position = new(position.x, 2, position.y);
                        attakerObj.Init();
                        _runtimeData.Views.Add(attakerObj);
                    }
                }

                _runtimeDataHolder.SetData(_runtimeData);
                StartAttakerBrains();
            }
            else if(!state)
            {
                _runtimeData?.ClearData();
                _compositeDisposable?.Dispose();
                _runtimeDataHolder.SetData(_runtimeData);
            }


            return base.OnEnter(state);
        }

        public override UniTask OnEnter()
        {
            _compositeDisposable = new CompositeDisposable();
            _runtimeData = new AttakerModuleRuntimeData();
            if (_runtimeDataHolder.TryGetData(out MapGeneratorRuntimeData data))
            {
                for (int i = 0; i < _database.AttakeCount; i++)
                {
                    var position = data.HightPoints[URandom.Range(0, data.HightPoints.Count)];
                    data.HightPoints.Remove(position);
                    var attakerObj = MonoBehaviour.Instantiate(_database.ViewPfbs[URandom.Range(0, _database.ViewPfbs.Count)]);
                    attakerObj.transform.position = new(position.x, 2, position.y);
                    attakerObj.Init();
                    _runtimeData.Views.Add(attakerObj);
                }
            }

            _runtimeDataHolder.SetData(_runtimeData);
            StartAttakerBrains();
            return base.OnEnter();
        }

        public override UniTask OnPause(bool pause)
        {
            foreach (var item in _runtimeData.Views)
            {
                item.gameObject.SetActive(pause);
            }
            return base.OnPause(pause);
        }

        private void StartAttakerBrains()
        {
            Observable
                .EveryFixedUpdate()
                .Subscribe(FindTargets)
                .AddTo(_compositeDisposable);
        }

        private void FindTargets(long obj)
        {
            if (!_isActive.Value) return;
            if (_runtimeDataHolder.TryGetData(out CharacterRuntimeData data))
            {
                var pose = data.CharacterInstance.transform.position;
                foreach (var item in _runtimeData.Views)
                {
                    item.LookAt(pose);
                    var distance = Vector3.Distance(item.AttackPosition, pose);
                    if (distance <= item.AttackRange)
                        item.TryAttack(pose);

                }
            }
        }
    }

    [System.Serializable]
    public class AttakerModuleRuntimeData : IModuleRuntimeData
    {
        public List<AttakerViewBase> Views;

        public AttakerModuleRuntimeData()
        {
            Views = new List<AttakerViewBase>();
        }

        public void ClearData()
        {
            for (var i = 0; i < Views.Count; i++) 
            {
                MonoBehaviour.Destroy(Views[i]);
            }
            Views.Clear();
        }
    }

}


