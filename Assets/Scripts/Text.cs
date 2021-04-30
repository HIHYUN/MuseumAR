using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Networking;

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
    private List<string> aboutlist = new List<string>();


    // Json
    private List<string> urllist = new List<string>();
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

    /*
    IEnumerator SetSprite(string url, int i)
    {

        UnityWebRequest wr = new UnityWebRequest(url);
        DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
        wr.downloadHandler = texDl;
        wr.timeout = 1;
        yield return wr.SendWebRequest();

        Texture2D tex = texDl.texture;
        WhoImageList[i].gameObject.transform.Find("Artist Image").GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0,0, tex.width, tex.height), Vector2.one * 0.5f);        
    }*/
}
