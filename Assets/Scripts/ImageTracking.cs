using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;


[RequireComponent (typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;
    public GameObject PaintNameButtonList;
    private List<Button> ArtButtonList = new List<Button>();

    // Aritst Canvas 
    public GameObject AritstCanvas;
    public Button ArtistNameButton;
    public TextMeshProUGUI ArtistNameButtonText;
    public TextMeshProUGUI ArtistNameInAritstCanvas;
    public TextMeshProUGUI ArtistHistoryInAritstCanvas;
    public TextMeshProUGUI ImageInformation;
    private List<string> aboutlist = new List<string>();
    public GameObject ParentImageButton;
    public GameObject ImageButtonPrefab;
    private List<Button> WhoImageList = new List<Button>();
    

    // Information Canvas
    public GameObject InformationCanvas;
    public TextMeshProUGUI ArtNameInInformation;
    public TextMeshProUGUI ArtistNameInInformation;
    public TextMeshProUGUI DateInInformation;
    public TextMeshProUGUI DescriptionInInformation;
    public AudioSource Docent;

    // Get Json Data
    private ArtJsonData PaintJsonData;
    private ArtistJsonData WhoJsonData;
    void Start() 
    {
        TextAsset pjsonFile = Resources.Load("MuseumInformation") as TextAsset;
        PaintJsonData = JsonUtility.FromJson<ArtJsonData>(pjsonFile.text);  

        TextAsset ajsonFile = Resources.Load("ArtistInformation") as TextAsset;
        WhoJsonData = JsonUtility.FromJson<ArtistJsonData>(ajsonFile.text);  
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
            ArtistNameButton.gameObject.SetActive(false);
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
                //StartCoroutine(GetAudioClip(json.audio));
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
                foreach(ArtistJsonData.Artist ajson in WhoJsonData.artist)
                {
                    if(ajson.name == pjson.artist)
                    {
                        ArtistNameButtonText.text = ajson.name;
                        ArtistNameInAritstCanvas.text = ajson.name;
                        ArtistHistoryInAritstCanvas.text = ajson.history;
                        
                        foreach(ArtistJsonData.Image img in ajson.image)
                        {   
                            ImageInformation.text = img.about;
                            GameObject imgbttnO = Instantiate(ImageButtonPrefab) as GameObject;
                            imgbttnO.transform.SetParent(ParentImageButton.transform);
                            imgbttnO.transform.localScale = Vector3.one;
                            imgbttnO.transform.localPosition = new Vector3(600, 0, 0);
                            Button imgbttn = imgbttnO.GetComponent(typeof(Button)) as Button;

                            WhoImageList.Add(imgbttn);
                            aboutlist.Add(img.about);
                        }
                    }
                }
            }
        }
        for (int i =0; i < WhoImageList.Count; i++)
        {   
            string path = "Image/Artist/" + ArtistNameInAritstCanvas.text+ i.ToString();

            if(i ==0)
            {
                WhoImageList[i].GetComponent<RectTransform>().localPosition = Vector3.zero;
                WhoImageList[i].gameObject.transform.Find("Artist Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(path) as Sprite;
            }
            else
            {
                WhoImageList[i].gameObject.transform.Find("Artist Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(path) as Sprite;
            }
        }
        ArtistNameButton.gameObject.SetActive(true);
    }

    private void OpenArtistCanvas()
    {
        ArtistNameButton.gameObject.SetActive(false);
        AritstCanvas.gameObject.SetActive(true);
    }

    public void GetAudio(string artname)
    {
        Docent.clip = Resources.Load<AudioClip>("Audio/" + artname);
    }
    
    /*
    IEnumerator SetSprite(string url, int i)
    {

        UnityWebRequest wr = new UnityWebRequest(url);
        DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
        wr.downloadHandler = texDl;
        wr.timeout = 3;
        yield return wr.SendWebRequest();

        Texture2D tex = texDl.texture;
        WhoImageList[i].gameObject.transform.Find("Artist Image").GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0,0, tex.width, tex.height), Vector2.one * 0.5f);        
    }

    IEnumerator GetAudioClip(string audiourl) 
    {
        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(audiourl, AudioType.MPEG)) 
        {
            yield return uwr.SendWebRequest();
        
            Docent.clip = DownloadHandlerAudioClip.GetContent(uwr);
        }
    }   */
}
