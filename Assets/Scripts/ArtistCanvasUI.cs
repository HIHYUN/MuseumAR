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
    public TextMeshProUGUI ImageInformationText;

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
    private Sequence ImageShow;
    private Sequence ImageDisappear;

    private float offset;
    private float alphoff;
    private float time;
    private int step;
    private int size;

    private ArtistJsonData WhoJsonData;
    private ArtistJsonData.Image[] infolist;

    private void Awake() 
    {
        step = 1;
        TextAsset ajsonFile = Resources.Load("ArtistInformation") as TextAsset;
        WhoJsonData = JsonUtility.FromJson<ArtistJsonData>(ajsonFile.text);  

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
        TextMeshProUGUI who = ArtistName.GetComponent<TextMeshProUGUI>();

        size = ArtistImageList.transform.childCount;

        ButtonUpdate();

        foreach(ArtistJsonData.Artist json in WhoJsonData.artist)
        {
            if(json.name == who.text)
            {
                infolist = json.image;
            }
        }
    }

    private void Update() 
    {
        ButtonUpdate();
    }

    public void OnEnable() 
    {    
        offset = 0;
        alphoff = 0;
        time =0;

        Opening = DOTween.Sequence().Append(AlphaBackground.DOFade(1,0.4f).From(0,true).SetEase(Ease.OutQuad))
                .Join(namecanv.DOFade(1, 2f).From(0, true))
                .Join(namerec.DOLocalMoveY(540, 1f).From(580, true))
                .Join(leftmove.DOLocalMoveX(-320,0.5f).From(-450, true))
                .Join(rightmove.DOLocalMoveX(320,0.5f).From(450, true))
                .Join(descanv.DOFade(1, 2f).From(0, true))
                .Join(desrect.DOLocalMoveY(-370, 2f).From(-430, true))
                .Join(clcanv.DOFade(1, 0.3f).From(0, true))
                .Join(clrec.DOScale(1.2f,0.3f).From(0,true).OnComplete(() => clrec.DOScale(1,0.1f).From(1.3f,true)));

        for(int i = size - 1; 0 <= i; i--)
        {   
            GameObject img = ImageList.transform.GetChild(i).gameObject;
            img.GetComponent<RectTransform>().localPosition = new Vector3(0,offset,0);
            ImageShow = DOTween.Sequence()
                      .Insert(time,img.GetComponent<RectTransform>().DOLocalMoveX(offset,0.5f).From(100+offset, true)).SetEase(Ease.InOutCubic)
                      .Join(img.GetComponent<CanvasGroup>().DOFade(1 - alphoff, 0.5f).From(0, true));
                      
            offset += 20;
            alphoff += 0.3f;
            time += 0.1f;
        }
    }

    public void CloseArtistCanvasMotion()
    {
        offset = 0;
        alphoff = 0;
        time =0;

        Closeing = DOTween.Sequence().Append(AlphaBackground.DOFade(0, 0.4f).SetEase(Ease.OutQuad))
                .Join(namecanv.DOFade(0, 0.3f).From(1, true))
                .Join(namerec.DOLocalMoveY(580, 0.3f).From(540, true))
                .Join(leftmove.DOLocalMoveX(-450,0.3f).From(-320,true))
                .Join(rightmove.DOLocalMoveX(450,0.3f).From(320,true))
                .Join(descanv.DOFade(0, 0.3f).From(1, true))
                .Join(desrect.DOLocalMoveY(-430, 0.3f).From(-370, true))
                .Join(clcanv.DOFade(1, 0.3f).From(0,true))
                .Join(clrec.DOScale(1, 0.3f).From());
        
        for(int i = size - 1; 0 <= i; i--)
        {   
            GameObject img = ImageList.transform.GetChild(i).gameObject;
            ImageDisappear = DOTween.Sequence()
                      .Insert(time,img.GetComponent<RectTransform>().DOLocalMoveX(offset -100,0.3f).From(offset, true)).SetEase(Ease.InOutCubic)
                      .Join(img.GetComponent<CanvasGroup>().DOFade(0, 0.2f).From(1- alphoff, true)).SetEase(Ease.InOutQuint)
                      .OnComplete(CloseArtistCanvas);
                      
            offset += 20;
            alphoff += 0.3f;
            time += 0.1f;
        }
    }


    public void CloseArtistCanvas()
    {
        this.gameObject.SetActive(false);
    }

    public void LeftMoveImage()
    {
        DOTween.Sequence()
                .Append(leftmove.DOLocalMoveX(-340,0.4f).From(-320, true)).SetEase(Ease.OutExpo)
                .Append(leftmove.DOLocalMoveX(-340,0.4f).From()).SetEase(Ease.OutExpo);
    }

    public void RightMoveImage()
    {
        DOTween.Sequence()
                .Append(rightmove.DOLocalMoveX(340,0.5f).From(320, true)).SetEase(Ease.OutExpo)
                .Append(rightmove.DOLocalMoveX(340,0.4f).From()).SetEase(Ease.OutExpo);

        GameObject img = ImageList.transform.GetChild(size - step).gameObject;
    }

    private void ButtonUpdate()
    {
        if (size <= 1)
        {
            leftmove.gameObject.SetActive(false);
            leftmove.gameObject.SetActive(false);
        }
        else if (step == 1)
        {
            leftmove.gameObject.SetActive(false);
        }
        else if (step == size-1)
        {
            rightmove.gameObject.SetActive(false);
        }
        else
        {
            leftmove.gameObject.SetActive(true);
            rightmove.gameObject.SetActive(true);
        }
    }
    

}
