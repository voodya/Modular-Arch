using DG.Tweening;
using UnityEngine;

public class LoadScreenService : MonoBehaviour, ILoadScreenService, IScene
{
    [SerializeField] private CanvasGroup _canvasGroup;

    private Tween _currentTween;

    public void StartLoad()
    {
        _currentTween?.Kill();
        _canvasGroup.blocksRaycasts = true;
        _currentTween = _canvasGroup.DOFade(1f, 0.5f);
    }

    public void StopLoad()
    {
        _currentTween?.Kill();
        _currentTween = _canvasGroup.DOFade(0f, 0.5f);
        _canvasGroup.blocksRaycasts = false;
    }
}
