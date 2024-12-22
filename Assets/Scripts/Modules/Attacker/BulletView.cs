using Cysharp.Threading.Tasks;
using Module.Character;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Module.Attcker
{
    public class BulletView : MonoBehaviour
    {
        [SerializeField] private Rigidbody Rb;
        [SerializeField] private float _bulletForce = 100f;
        [SerializeField] private Collider _collider;

        public bool IsActive;
        public Action<BulletView> OnDisposed;

        private void OnEnable()
        {
            _collider.OnTriggerEnterAsObservable().Subscribe(OnShooted).AddTo(this);
        }

        private void OnShooted(Collider collider)
        {
            if (collider.TryGetComponent(out CharacterView character))
            {
                character.OnShooted();
                if (IsActive)
                {
                    IsActive = false;
                    OnDisposed?.Invoke(this);
                }
            }
        }

        public async void AddForce(Vector3 forceDirection)
        {
            IsActive = true;
            Rb.AddForce(forceDirection * _bulletForce, ForceMode.Impulse);
            await UniTask.Delay(3000);
            if (IsActive)
            {
                IsActive = false;
                OnDisposed?.Invoke(this);
            }
        }

        internal void SetPose(Vector3 attackPosition)
        {
            Rb.linearVelocity = Vector3.zero;
            transform.position = attackPosition;

        }
    }
}


