using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Module.Character
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private MeshRenderer _meshRenderer;

        public Rigidbody Rb => _rb;

        public void Init()
        {
            _meshRenderer.material.color = Color.blue;

        }

        public async void OnShooted()
        {
            _meshRenderer.material.color = Color.red;
            await UniTask.Delay(500);
            _meshRenderer.material.color = Color.blue;

        }
    }
}