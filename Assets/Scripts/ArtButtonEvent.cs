using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Networking;

public class ArtButtonEvent : MonoBehaviour
{
    private Button bttn;

    // Information Canvas
    public GameObject InformationCanvas;
    public TextMeshProUGUI ArtNameInInformation;
    public TextMeshProUGUI ArtistNameInInformation;
    public TextMeshProUGUI DateInInformation;
    public TextMeshProUGUI DescriptionInInformation;

    // Json Data
    private ArtJsonData PaintJsonData;
    private ArtistJsonData WhoJsonData;

    private void Awake() 
    {
        bttn = this.GetComponent(typeof(Button)) as Button;
        bttn.onClick.AddListener(() => OpenInformationCanvas(bttn.name));
    }
    

    public void OpenInformationCanvas(string buttonname)
    {
        Debug.Log(buttonname);
        foreach (ArtJsonData.Data json in PaintJsonData.data)
        {   
            Debug.Log(json.name);
            if (json.name == buttonname)
            {
                ArtNameInInformation.text = json.name;
                ArtistNameInInformation.text = json.artist;
                DateInInformation.text = json.date;
                DescriptionInInformation.text = json.description;
            }
        } 
        InformationCanvas.SetActive(true);
    }
}
