using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;


[RequireComponent (typeof(ARTrackedImageManager))]
public class TestImageTracking : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;
    public GameObject PaintNameButtonList;
    private List<Button> ArtButtonList = new List<Button>();

    // Aritst Canvas 
    public GameObject JohannesCanvas;
    public Button JohannesButton;
    public GameObject VincentCanvas;
    public Button VincentButton;
    

    // Information Canvas
    public GameObject InformationCanvas;
    public TextMeshProUGUI ArtNameInInformation;
    public TextMeshProUGUI ArtistNameInInformation;
    public TextMeshProUGUI DateInInformation;
    public TextMeshProUGUI DescriptionInInformation;
    public AudioSource Docent;

    // Get Json Data
    private ArtJsonData PaintJsonData;
    void Start() 
    {
        TextAsset pjsonFile = Resources.Load("MuseumInformation") as TextAsset;
        PaintJsonData = JsonUtility.FromJson<ArtJsonData>(pjsonFile.text);  
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

        JohannesButton.onClick.AddListener(OpenJohannesCanvas);
        VincentButton.onClick.AddListener(OpenVincentCanvas);
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
            JohannesButton.gameObject.SetActive(false);
            VincentButton.gameObject.SetActive(false);
        }

        if(eventArgs.updated.Count == 1 && eventArgs.updated[0].trackingState == TrackingState.Tracking)
        {
            AlertArtistButton(eventArgs.updated[0].referenceImage.name);
        }
        else
        {
            JohannesButton.gameObject.SetActive(false);
            VincentButton.gameObject.SetActive(false);
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
            JohannesButton.gameObject.SetActive(false);
            VincentButton.gameObject.SetActive(false);
        }
    }

    private void OpenInformationCanvas(string buttonname)
    {
        foreach (ArtJsonData.Data json in PaintJsonData.data)
        {
            if (json.name == buttonname)
            {
                ArtNameInInformation.text = json.name;
                ArtistNameInInformation.text = json.artist;
                DateInInformation.text = json.date;
                DescriptionInInformation.text = json.description;
                GetAudio(json.name);
            }
        }
        InformationCanvas.SetActive(true);
    }
    private void AlertArtistButton(string buttonname)
    {
        foreach (ArtJsonData.Data pjson in PaintJsonData.data)
        {
            if (pjson.name == buttonname)
            {
                if(pjson.artist == "Johannes Vermeer")
                {
                    VincentButton.gameObject.SetActive(false);
                    JohannesButton.gameObject.SetActive(true);
                }
                else if(pjson.artist == "Vincent van Gogh")
                {
                    JohannesButton.gameObject.SetActive(false);
                    VincentButton.gameObject.SetActive(true);
                }
            }
        }
    }

    private void OpenJohannesCanvas()
    {
        JohannesCanvas.gameObject.SetActive(true);
        JohannesButton.gameObject.SetActive(false);
    }
    private void OpenVincentCanvas()
    {
        VincentCanvas.gameObject.SetActive(true);
        VincentButton.gameObject.SetActive(false);
    }

    public void GetAudio(string artname)
    {
        Docent.clip = Resources.Load<AudioClip>("Audio/" + artname);
    }
}
