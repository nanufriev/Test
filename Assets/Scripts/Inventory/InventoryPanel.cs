using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InventoryPanel : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    private bool _isShow = false;
    private const float _fadeTime = 0.5f;

    public bool IsShow => _isShow;
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show()
    {
        _canvasGroup.DOFade(1, _fadeTime);
        DOTween.Play(_canvasGroup);
        _isShow = true;
    }

    public void Hide()
    {
        _canvasGroup.DOFade(0, _fadeTime);
        DOTween.Play(_canvasGroup);
        _isShow = false;
    }
}
