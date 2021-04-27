using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ArtistNameButtonMotion : MonoBehaviour
{
    private CanvasGroup Oppacity;
    private Sequence Bling; 

    void Awake() 
    {
        Bling = DOTween.Sequence();
        Oppacity = this.GetComponent<CanvasGroup>();
        
        Bling.SetAutoKill(false)
            .Append(Oppacity.DOFade(1, 2).From(0,true))
            .Append(Oppacity.DOFade(1, 2).From())
            .SetLoops(-1, LoopType.Restart)
            .Pause();
    }
    
    void OnEnable()
    {
        Bling.Restart();
    }
}
