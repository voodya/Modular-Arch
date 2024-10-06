using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameCircle : MonoBehaviour, IGameCircle
{
    public async UniTask Initialize()
    {
        await UniTask.Delay(5000);
    }
}
