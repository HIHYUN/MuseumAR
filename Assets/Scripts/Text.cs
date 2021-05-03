using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Text : MonoBehaviour
{
    public GameObject AritstCanvas;
    public Button ArtistNameButton;
    public TextMeshProUGUI ArtistNameButtonText;
    public TextMeshProUGUI ArtistNameInAritstCanvas;
    public TextMeshProUGUI ArtistHistoryInAritstCanvas;
    public TextMeshProUGUI ImageInformation;
    public GameObject ParentImageList;
    public GameObject ArtistImagePrefab;
    public static List<string> aboutlist = new List<string>();
    public static List<string> storylist = new List<string>();


    // Json
    private ArtJsonData PaintJsonData;
    private ArtistJsonData WhoJsonData;

    void Awake() 
    {
        TextAsset pjsonFile = Resources.Load("MuseumInformation") as TextAsset;
        PaintJsonData = JsonUtility.FromJson<ArtJsonData>(pjsonFile.text);  

        TextAsset ajsonFile = Resources.Load("ArtistInformation") as TextAsset;
        WhoJsonData = JsonUtility.FromJson<ArtistJsonData>(ajsonFile.text);  

        AlertArtistButton("Self Portrait");
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
                            aboutlist.Add(img.about);
                            storylist.Add(img.story);
                        }
                    }
                }
            }
        }
        for (int i = aboutlist.Count-1; 0 <= i; i--)
        {   
            GameObject imgO = Instantiate(ArtistImagePrefab) as GameObject;
            imgO.transform.SetParent(ParentImageList.transform);
            imgO.transform.localScale = Vector3.one;
            imgO.transform.localPosition = new Vector3(0, 0, 0);
            Image imgp = imgO.GetComponent(typeof(Image)) as Image;

            string path = "Image/Artist/" + ArtistNameInAritstCanvas.text+ i.ToString();
            imgp.sprite = Resources.Load<Sprite>(path) as Sprite;
            ImageInformation.text = aboutlist[i];
        }
        ArtistNameButton.gameObject.SetActive(true);
    }
}
