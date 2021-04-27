using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;



public class FastWaveMotion : MonoBehaviour
{

    public Button AudioPauseButton;

    private Sequence AudioSequence; 
    private RectTransform rect;
    private CanvasGroup Oppacity;

    void Awake()
    {
        AudioSequence = DOTween.Sequence();
        rect = this.GetComponent<RectTransform>();
        Oppacity = this.GetComponent<CanvasGroup>();

        if(AudioPauseButton.gameObject.activeSelf)
        {
            AudioSequence.SetAutoKill(false)
                     .OnStart(() => {
                        rect.localScale = Vector3.zero;
                     })
                     .Append(Oppacity.DOFade(0, 0.7f).From(1, true))
                     .Join(rect.DOScale(1.5f,0.5f))
                     .SetLoops(-1, LoopType.Restart)
                     .Pause();
        }
    }
    
    void OnEnable()
    {
        AudioSequence.Restart();
    }
}
