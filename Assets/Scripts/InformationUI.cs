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
    public RectTransform InformationPanel;
    public Button CloseButton;
    public CanvasGroup AlphaBackground;

    public Button AudioPlayButton;
    public Button AudioPauseButton;
    public GameObject WaveBackground;
    public AudioSource Docent;

    private void Awake() 
    {
        CloseButton.onClick.AddListener(CloseInformationMotion);
        AudioPlayButton.onClick.AddListener(PlayDocent);
        AudioPauseButton.onClick.AddListener(PauseDocent);
    }

    public void OnEnable() 
    {
        InformationPanel.transform.localPosition = new Vector3(0,-1300,0);
        InformationPanel.transform.DOLocalMoveY(0, 0.4f).SetEase(Ease.OutQuad);
        AlphaBackground.DOFade(1,0.4f).SetEase(Ease.OutQuad);
        WaveBackground.gameObject.SetActive(false);
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

    public void PlayDocent()
    {
        AudioPlayButton.gameObject.SetActive(false);
        AudioPauseButton.gameObject.SetActive(true);
        WaveBackground.gameObject.SetActive(true);
        Docent.Play();
    }

    public void PauseDocent()
    {
        AudioPauseButton.gameObject.SetActive(false);
        AudioPlayButton.gameObject.SetActive(true);
        WaveBackground.gameObject.SetActive(false);
        Docent.Pause();
    }
}
