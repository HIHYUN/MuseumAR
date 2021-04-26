using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class InformationUI : MonoBehaviour
{

    public GameObject HomeCanvas;
    public RectTransform InformationCanvas;
    public RectTransform InformationPanel;
    public Button CloseButton;
    public CanvasGroup AlphaBackground;

    private void Awake() {
        CloseButton.onClick.AddListener(CloseInformationMotion);
    }

    public void OnEnable() {
        InformationPanel.transform.localPosition = new Vector3(0,-1300,0);
        InformationPanel.transform.DOLocalMoveY(0, 0.4f).SetEase(Ease.OutQuad);
        AlphaBackground.DOFade(1,0.4f).SetEase(Ease.OutQuad);
    }

    public void CloseInformationMotion()
    {
        AlphaBackground.DOFade(0, 0.4f).SetEase(Ease.OutQuad);
        InformationPanel.transform.DOLocalMoveY(-1300, 0.4f).SetEase(Ease.OutQuad).OnComplete(CloseInformationCanvas);
    }
    public void CloseInformationCanvas()
    {
        this.gameObject.SetActive(false);
    }
}
