using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Attcker
{
    public class AttakerViewBase : MonoBehaviour
    {
        [SerializeField] private float _attackRange;
        [SerializeField] private Transform _lookerPart;
        [SerializeField] private BulletView _bulletView;
        [SerializeField] private int _maxBulletCount;
        [SerializeField] private float _attackDelay = 1f;

        private Queue<BulletView> _bulletPool;
        public float AttackRange => _attackRange;
        public Vector3 AttackPosition => _lookerPart.position;

        private bool IsDelayed;

        public void Init()
        {
            _bulletPool = new Queue<BulletView>();
            for (int i = 0; i < _maxBulletCount; i++)
            {
                var bullet = Instantiate(_bulletView, _lookerPart);
                bullet.SetPose(AttackPosition);
                _bulletPool.Enqueue(bullet);
                bullet.OnDisposed += ReturnToPool;
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _maxBulletCount; i++)
            {
                _bulletPool.Peek().OnDisposed -= ReturnToPool;
            }
        }

        private void ReturnToPool(BulletView view)
        {
            view.SetPose(AttackPosition);
            _bulletPool.Enqueue(view);
        }

        public void LookAt(Vector3 target)
        {
            _lookerPart.LookAt(target);
        }

        public async void TryAttack(Vector3 target)
        {
            if (!IsDelayed)
            {
                _bulletPool.Dequeue().AddForce((target - AttackPosition).normalized);
                IsDelayed = true;
                await UniTask.Delay(TimeSpan.FromSeconds(_attackDelay));
                IsDelayed = false;
            }
        }
    }
}

