using System;
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
    public TextMeshProUGUI ImageAboutText;
    public TextMeshProUGUI ImageStoryText;

    private RectTransform leftmove;
    private RectTransform rightmove;
    private CanvasGroup namecanv;
    private RectTransform namerec;
    private CanvasGroup aboutcanv;
    private RectTransform aboutrec;
    private CanvasGroup descanv;
    private RectTransform desrect;
    private CanvasGroup clcanv;
    private RectTransform clrec;

    private Sequence Opening;
    private Sequence Closeing;
    private Sequence ImageShow;
    private Sequence ImageDisappear;

    private int step;
    private int size;

    private List<string> ablist = new List<string>();
    private List<string> stlist = new List<string>();

    private void Awake() 
    {
        ablist = new List<string>(ImageTracking.aboutlist);
        ablist.Reverse();
        stlist = new List<string>(ImageTracking.storylist);
        stlist.Reverse();

        CloseButton.onClick.AddListener(CloseArtistCanvasMotion);
        LeftButton.onClick.AddListener(BeforeImage);
        RightButton.onClick.AddListener(NextImage);

        leftmove = LeftButton.GetComponent(typeof (RectTransform)) as RectTransform;  
        rightmove = RightButton.GetComponent(typeof (RectTransform)) as RectTransform;
        namecanv = ArtistName.GetComponent<CanvasGroup>();
        namerec = ArtistName.GetComponent<RectTransform>();
        aboutcanv = ImageAboutText.gameObject.GetComponent<CanvasGroup>();
        aboutrec = ImageAboutText.gameObject.GetComponent<RectTransform>();
        descanv = DescriptionPanel.GetComponent<CanvasGroup>();
        desrect = DescriptionPanel.GetComponent<RectTransform>();
        clcanv = CloseButton.GetComponent<CanvasGroup>();
        clrec = CloseButton.GetComponent<RectTransform>();
        TextMeshProUGUI who = ArtistName.GetComponent<TextMeshProUGUI>();

        size = ArtistImageList.transform.childCount;
        // step의 번호는 image 리스트 index로 한다.
        step = ArtistImageList.transform.childCount-1;

        
        ButtonUpdate();
    }

    private void Update() 
    {
        ButtonUpdate();
    }

    public void OnEnable() 
    {    

        Opening = DOTween.Sequence().Append(AlphaBackground.DOFade(1,0.4f).From(0,true).SetEase(Ease.OutQuad))
                .Join(namecanv.DOFade(1, 2f).From(0, true))
                .Join(namerec.DOLocalMoveY(540, 1f).From(580, true))
                .Join(leftmove.DOLocalMoveX(-320,0.5f).From(-450, true))
                .Join(rightmove.DOLocalMoveX(320,0.5f).From(450, true))
                .Join(aboutcanv.DOFade(1, 2f).From(0, true))
                .Join(aboutrec.DOLocalMoveY(-30, 2f).From(-70, true))
                .Join(descanv.DOFade(1, 2f).From(0, true))
                .Join(desrect.DOLocalMoveY(-370, 2f).From(-430, true))
                .Join(clcanv.DOFade(1, 0.3f).From(0, true))
                .Join(clrec.DOScale(1.2f,0.3f).From(0,true).OnComplete(() => clrec.DOScale(1,0.1f).From(1.3f,true)));
        
        for(int i = size - 1; 0 <= i; i--)
        {   
            GameObject img = ImageList.transform.GetChild(i).gameObject;
            if(i == size -1)
            {
                img.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
                ImageShow = DOTween.Sequence()
                        .Append(img.GetComponent<RectTransform>().DOLocalMoveY(0, 1f).From(100, true)).SetEase(Ease.InOutQuad)
                        .Join(img.GetComponent<CanvasGroup>().DOFade(1, 1f).From(0, true));
            }
            else
            {
                img.GetComponent<CanvasGroup>().alpha = 0;
                img.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
                ImageShow = DOTween.Sequence()
                        .Append(img.GetComponent<RectTransform>().DOLocalMoveX(30,1f).From(100, true)).SetEase(Ease.InOutQuad);
            }
        }
    }

    public void CloseArtistCanvasMotion()
    {
        
        Closeing = DOTween.Sequence().Append(AlphaBackground.DOFade(0, 0.4f).SetEase(Ease.OutQuad))
                .Join(namecanv.DOFade(0, 0.4f).From(1, true))
                .Join(namerec.DOLocalMoveY(580, 0.4f).From(540, true))
                .Join(leftmove.DOLocalMoveX(-450,0.4f).From(-320,true))
                .Join(rightmove.DOLocalMoveX(450,0.4f).From(320,true))
                .Join(aboutcanv.DOFade(0, 0.4f).From(1, true))
                .Join(aboutrec.DOLocalMoveY(-70, 0.4f).From(-30, true))
                .Join(descanv.DOFade(0, 0.4f).From(1, true))
                .Join(desrect.DOLocalMoveY(-430, 0.4f).From(-370, true))
                .Join(clcanv.DOFade(1, 0.4f).From(0,true))
                .Join(clrec.DOScale(1, 0.4f).From());
        
        for(int i = size - 1; 0 <= i; i--)
        {   
            GameObject img = ImageList.transform.GetChild(i).gameObject;
            if(i == step)
            {
                img.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
                ImageShow = DOTween.Sequence()
                        .Append(img.GetComponent<RectTransform>().DOLocalMoveY(100,0.5f).From(0, true)).SetEase(Ease.InOutQuad)
                        .Join(img.GetComponent<CanvasGroup>().DOFade(0, 0.5f).From(1, true)).OnComplete(CloseArtistCanvas);
            }
            else
            {
                img.GetComponent<CanvasGroup>().alpha = 0;
                img.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
                ImageShow = DOTween.Sequence()
                        .Append(img.GetComponent<RectTransform>().DOLocalMoveY(100,0.5f).From(0, true)).SetEase(Ease.InOutQuad);
            }
        }
    }


    public void CloseArtistCanvas()
    {
        this.gameObject.SetActive(false);
    }


    // 맨 처음순서가 Image List 중 마지막 Index, 맨 마지막 순서가 List 중 맨 처음 Index = 0
    public void BeforeImage()
    {
        DOTween.Sequence()
                .Append(leftmove.DOLocalMoveX(-340,0.4f).From(-320, true)).SetEase(Ease.OutExpo)
                .Append(leftmove.DOLocalMoveX(-340,0.4f).From()).SetEase(Ease.OutExpo);
        
        if(0 <= step && step < ArtistImageList.transform.childCount)
        {
            
            RectTransform nowimg = ImageList.transform.GetChild(step).gameObject.GetComponent<RectTransform>();
            CanvasGroup nowalpha = ImageList.transform.GetChild(step).gameObject.GetComponent<CanvasGroup>();
            RectTransform beforeimg = ImageList.transform.GetChild(step+1).gameObject.GetComponent<RectTransform>();
            CanvasGroup beforealpha = ImageList.transform.GetChild(step+1).gameObject.GetComponent<CanvasGroup>();

            // 가장 현재 있는 image position 0,0 next 30, 0
            DOTween.Sequence()
                .Append(nowimg.DOLocalMoveX(nowimg.localPosition.x+80,0.5f).From(nowimg.localPosition.x, true)).SetEase(Ease.InOutQuint)
                .Join(nowalpha.DOFade(0,0.5f).From(nowalpha.alpha,true)).SetEase(Ease.InOutQuint)
                .Join(beforeimg.DOLocalMoveX(0,0.5f).From(beforeimg.localPosition.x, true)).SetEase(Ease.InOutQuint)
                .Join(beforealpha.DOFade(1,0.5f).From(beforealpha.alpha,true)).SetEase(Ease.InOutQuint)
                .Join(aboutcanv.DOFade(1, 0.5f).From(0, true))
                .Join(aboutrec.DOLocalMoveY(-30, 0.5f).From(-50, true))
                .Join(descanv.DOFade(1, 0.5f).From(0, true))
                .Join(desrect.DOLocalMoveY(-370, 0.5f).From(-400, true));

            step += 1;

            ImageAboutText.text = ablist[step];
            ImageStoryText.text = stlist[step];
        }
    }

    public void NextImage()
    {
        DOTween.Sequence()
                .Append(rightmove.DOLocalMoveX(340,0.5f).From(320, true)).SetEase(Ease.OutExpo)
                .Append(rightmove.DOLocalMoveX(340,0.4f).From()).SetEase(Ease.OutExpo);

        if(0 <= step && step < ArtistImageList.transform.childCount)
        {
            
            RectTransform nowimg = ImageList.transform.GetChild(step).gameObject.GetComponent<RectTransform>();
            CanvasGroup nowalpha = ImageList.transform.GetChild(step).gameObject.GetComponent<CanvasGroup>();
            RectTransform nextimg = ImageList.transform.GetChild(step-1).gameObject.GetComponent<RectTransform>();
            CanvasGroup nextalpha = ImageList.transform.GetChild(step-1).gameObject.GetComponent<CanvasGroup>();

            // 가장 현재 있는 image position 0,0 next 30, 0
            DOTween.Sequence()
                .Append(nowimg.DOLocalMoveX(nowimg.localPosition.x-80,0.5f).From(nowimg.localPosition.x, true)).SetEase(Ease.InOutQuint)
                .Join(nowalpha.DOFade(0,0.5f).From(nowalpha.alpha,true)).SetEase(Ease.InOutQuint)
                .Join(nextimg.DOLocalMoveX(0,0.5f).From(nextimg.localPosition.x, true)).SetEase(Ease.InOutQuint)
                .Join(nextalpha.DOFade(1,0.5f).From(nextalpha.alpha,true)).SetEase(Ease.InOutQuint)
                .Join(aboutcanv.DOFade(1, 0.5f).From(0, true))
                .Join(aboutrec.DOLocalMoveY(-30, 0.5f).From(-50, true))
                .Join(descanv.DOFade(1, 0.5f).From(0, true))
                .Join(desrect.DOLocalMoveY(-370, 0.5f).From(-400, true));
            
            step -= 1;
            ImageAboutText.text = ablist[step];
            ImageStoryText.text = stlist[step];
        }
    }

    private void ButtonUpdate()
    {
        if (size <= 1)
        {
            leftmove.gameObject.SetActive(false);
            rightmove.gameObject.SetActive(false);
        }
        else if (step == ArtistImageList.transform.childCount-1)
        {
            leftmove.gameObject.SetActive(false);
            rightmove.gameObject.SetActive(true);
        }
        else if (step == 0)
        {
            rightmove.gameObject.SetActive(false);
            leftmove.gameObject.SetActive(true);
        }
        else
        {
            leftmove.gameObject.SetActive(true);
            rightmove.gameObject.SetActive(true);
        }
    }
}
