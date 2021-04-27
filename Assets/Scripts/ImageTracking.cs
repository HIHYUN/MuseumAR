using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


[RequireComponent (typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;
    public GameObject PaintNameButtonList;
    public List<Button> ArtButtonList = new List<Button>();

    // Aritst Canvas 
    public GameObject AritstCanvas;
    public Button ArtistNameButton;
    public TextMeshProUGUI ArtistNameButtonText;
    public TextMeshProUGUI ArtistNameInAritstCanvas;
    public TextMeshProUGUI ArtistHistoryInAritstCanvas;

    // Information Canvas
    public GameObject InformationCanvas;
    public TextMeshProUGUI ArtNameInInformation;
    public TextMeshProUGUI ArtistNameInInformation;
    public TextMeshProUGUI DateInInformation;
    public TextMeshProUGUI DescriptionInInformation;

    // Get Json Data
    private ArtJsonData JsonData;
    
    void Start() 
    {
        TextAsset jsonFile = Resources.Load("MuseumInformation") as TextAsset;
        JsonData = JsonUtility.FromJson<ArtJsonData>(jsonFile.text);  
    }

    void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        
        for (int i = 0; i < PaintNameButtonList.transform.childCount; i++)
        {
            Button bttn = PaintNameButtonList.transform.GetChild(i).GetComponent<Button>();
            ArtButtonList.Add(bttn);
        }

        foreach (Button button in ArtButtonList)
        {
            button.onClick.AddListener(() => OpenInformationCanvas(button.name));
        }

        ArtistNameButton.onClick.AddListener(OpenArtistCanvas);
    }

    void OnEnable() 
    {
        trackedImageManager.trackedImagesChanged += OnImageChanged;
    }
    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        
        foreach(ARTrackedImage img in eventArgs.added)
        {
            UpdateImage(img);
        }
        foreach(ARTrackedImage img in eventArgs.updated)
        {
            UpdateImage(img);
        }
        foreach(ARTrackedImage img in eventArgs.removed)
        {
            ArtButtonList.Find(b => b.name == img.referenceImage.name).gameObject.SetActive(false);
            ArtistNameButton.gameObject.SetActive(false);
        }

        if(eventArgs.updated.Count ==1)
        {
            AlertArtistButton(eventArgs.updated[0].referenceImage.name);
        }
        else
        {
            ArtistNameButton.gameObject.SetActive(false);
        }
        
    }
    private void UpdateImage(ARTrackedImage img)
    {   
        // tracked 된 이미지 이름 할당
        string imgName = img.referenceImage.name;
        // 해당 이미지의 이름과 같은 prefab 할당
        Button ClickButton = ArtButtonList.Find(b => b.name == imgName);
        RectTransform ButtonRect = ClickButton.GetComponent(typeof (RectTransform)) as RectTransform;

        // tracking 상태가 normal이면 버튼 UI와 이미지 위치 일치 시킴
        if(img.trackingState == TrackingState.Tracking)
        {   
            // 버튼은 Canvas 2D로 Text는 3D로 구현 뒤, 버튼 숨김. => 훨씬 물리적 시각효과
            Vector3 screenPos = Camera.main.WorldToScreenPoint(img.transform.position - new Vector3(0,img.size.y * 0.5f,0));
            ButtonRect.position = screenPos;

            ClickButton.gameObject.SetActive(true);            
        }
        // 이미지가 tracking에서 벗어난 상태일 경우 prefab disable
        else if(img.trackingState == TrackingState.Limited || img.trackingState == TrackingState.None)
        {
            ClickButton.gameObject.SetActive(false);
        }
    }

    private void OpenInformationCanvas(string buttonname)
    {
        InformationCanvas.SetActive(true);

        foreach (ArtJsonData.Data json in JsonData.data)
        {
            if (json.name == buttonname)
            {
                ArtNameInInformation.text = json.name;
                ArtistNameInInformation.text = json.artist;
                DateInInformation.text = json.date;
                DescriptionInInformation.text = json.description;
            }
        } 
    }
    private void AlertArtistButton(string buttonname)
    {
        foreach (ArtJsonData.Data json in JsonData.data)
        {
            if (json.name == buttonname)
            {
                ArtistNameButtonText.text = json.artist;

                ArtistNameInAritstCanvas.text = json.artist;
                ArtistHistoryInAritstCanvas.text = json.human;
            }
        }
        ArtistNameButton.gameObject.SetActive(true);
    }

    private void OpenArtistCanvas()
    {
        ArtistNameButton.gameObject.SetActive(false);
        AritstCanvas.gameObject.SetActive(true);
    }
}
