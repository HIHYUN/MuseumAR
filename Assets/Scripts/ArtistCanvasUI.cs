using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ArtistCanvasUI : MonoBehaviour
{
    public CanvasGroup AlphaBackground;
    public GameObject ArtistImageList;
    public GameObject ArtistName;

    public Button LeftButton;
    public Button RightButton;
    public Button CloseButton;

    public GameObject DescriptionPanel;
    private RectTransform leftmove;
    private RectTransform rightmove;

    private void Awake() 
    {
        CloseButton.onClick.AddListener(CloseArtistCanvasMotion);
        LeftButton.onClick.AddListener(LeftMoveImage);
        RightButton.onClick.AddListener(RightMoveImage);
    }
    
    public void OnEnable() 
    {
        leftmove = LeftButton.GetComponent(typeof (RectTransform)) as RectTransform;  
        rightmove = RightButton.GetComponent(typeof (RectTransform)) as RectTransform;  

        AlphaBackground.DOFade(1,0.4f).From(0,true).SetEase(Ease.OutQuad);

        ArtistName.GetComponent<CanvasGroup>().DOFade(1, 2f).From(0, true);
        ArtistName.GetComponent<RectTransform>().DOLocalMoveY(540, 1f).From(580, true);
        leftmove.DOLocalMoveX(-320,0.5f).From(-450, true);
        rightmove.DOLocalMoveX(320,0.5f).From(450, true);
        DescriptionPanel.GetComponent<CanvasGroup>().DOFade(1, 2f).From(0, true);
        DescriptionPanel.GetComponent<RectTransform>().DOLocalMoveY(-370, 2f).From(-430, true);
        CloseButton.GetComponent<CanvasGroup>().DOFade(1, 0.3f).From(0, true);
        CloseButton.GetComponent<RectTransform>().DOScale(1,0.3f).From(0,true);
    }

    public void CloseArtistCanvasMotion()
    {
        AlphaBackground.DOFade(0, 0.4f).SetEase(Ease.OutQuad);

        ArtistName.GetComponent<CanvasGroup>().DOFade(0, 0.3f).From(1, true);
        ArtistName.GetComponent<RectTransform>().DOLocalMoveY(580, 0.3f).From(540, true);
        leftmove.DOLocalMoveX(-450,0.3f).From(-320,true);
        rightmove.DOLocalMoveX(450,0.3f).From(320,true);
        DescriptionPanel.GetComponent<CanvasGroup>().DOFade(0, 0.3f).From(1, true);
        DescriptionPanel.GetComponent<RectTransform>().DOLocalMoveY(-430, 0.3f).From(-370, true).OnComplete(CloseArtistCanvas);
        CloseButton.GetComponent<RectTransform>().DOScale(1, 0.3f).From();
    }
    public void CloseArtistCanvas()
    {
        this.gameObject.SetActive(false);
    }

    public void LeftMoveImage()
    {

    }
    public void RightMoveImage()
    {

    }
    

}
