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
    public GameObject ImageList;

    private RectTransform leftmove;
    private RectTransform rightmove;
    private CanvasGroup namecanv;
    private RectTransform namerec;
    private CanvasGroup descanv;
    private RectTransform desrect;
    private CanvasGroup clcanv;
    private RectTransform clrec;

    private Sequence Opening;
    private Sequence Closeing;
    private Sequence ImageSlide;
    private void Awake() 
    {
        CloseButton.onClick.AddListener(CloseArtistCanvasMotion);
        LeftButton.onClick.AddListener(LeftMoveImage);
        RightButton.onClick.AddListener(RightMoveImage);

        leftmove = LeftButton.GetComponent(typeof (RectTransform)) as RectTransform;  
        rightmove = RightButton.GetComponent(typeof (RectTransform)) as RectTransform;
        namecanv = ArtistName.GetComponent<CanvasGroup>();
        namerec = ArtistName.GetComponent<RectTransform>();
        descanv = DescriptionPanel.GetComponent<CanvasGroup>();
        desrect = DescriptionPanel.GetComponent<RectTransform>();
        clcanv = CloseButton.GetComponent<CanvasGroup>();
        clrec = CloseButton.GetComponent<RectTransform>();

        Opening = DOTween.Sequence();
        Closeing = DOTween.Sequence();
        ImageSlide = DOTween.Sequence();
    }
  
    
    public void OnEnable() 
    {    
        float offset = 0;
        float alphoff = 0;
        Opening.Append(AlphaBackground.DOFade(1,0.4f).From(0,true).SetEase(Ease.OutQuad))
                .Join(namecanv.DOFade(1, 2f).From(0, true))
                .Join(namerec.DOLocalMoveY(540, 1f).From(580, true))
                .Join(leftmove.DOLocalMoveX(-320,0.5f).From(-450, true))
                .Join(rightmove.DOLocalMoveX(320,0.5f).From(450, true))
                .Join(descanv.DOFade(1, 2f).From(0, true))
                .Join(desrect.DOLocalMoveY(-370, 2f).From(-430, true))
                .Join(clcanv.DOFade(1, 0.3f).From(0, true))
                .Join(clrec.DOScale(1.2f,0.3f).From(0,true).OnComplete(() => clrec.DOScale(1,0.1f).From(1.3f,true)));

        for(int i = ImageList.transform.childCount-1; 0 <= i; i--)
        {   
            GameObject img = ImageList.transform.GetChild(i).gameObject;
            img.GetComponent<RectTransform>().localPosition = new Vector3(0,offset,0);
            ImageSlide.Append(img.GetComponent<RectTransform>().DOLocalMoveX(offset,0.2f).From(50+offset, true))
                      .Join(img.GetComponent<CanvasGroup>().DOFade(1 - alphoff, 0.2f).From(0, true));

            offset += 20;
            alphoff += 0.3f;
        }
    }

    public void CloseArtistCanvasMotion()
    {

        Closeing.Append(AlphaBackground.DOFade(0, 0.4f).SetEase(Ease.OutQuad))
                .Join(namecanv.DOFade(0, 0.3f).From(1, true))
                .Join(namerec.DOLocalMoveY(580, 0.3f).From(540, true))
                .Join(leftmove.DOLocalMoveX(-450,0.3f).From(-320,true))
                .Join(rightmove.DOLocalMoveX(450,0.3f).From(320,true))
                .Join(descanv.DOFade(0, 0.3f).From(1, true))
                .Join(desrect.DOLocalMoveY(-430, 0.3f).From(-370, true))
                .Join(clcanv.DOFade(1, 0.3f).From(0,true))
                .Join(clrec.DOScale(1, 0.3f).From().OnComplete(CloseArtistCanvas));
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
