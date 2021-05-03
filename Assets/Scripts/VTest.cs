using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class VTest : MonoBehaviour
{
    public GameObject JohannesCanvas;
    public Button JohannesButton;
    public GameObject VincentCanvas;
    public Button VincentButton;
    private ArtJsonData PaintJsonData;

    void Awake() 
    {
        TextAsset pjsonFile = Resources.Load("MuseumInformation") as TextAsset;
        PaintJsonData = JsonUtility.FromJson<ArtJsonData>(pjsonFile.text);  

        AlertArtistButton("Self Portrait");

        JohannesButton.onClick.AddListener(OpenJohannesCanvas);
        VincentButton.onClick.AddListener(OpenVincentCanvas);
    }

    private void AlertArtistButton(string buttonname)
    {
        foreach (ArtJsonData.Data pjson in PaintJsonData.data)
        {
            if (pjson.name == buttonname)
            {
                if(pjson.artist == "Johannes Vermeer")
                {
                    JohannesButton.gameObject.SetActive(true);
                }
                else if(pjson.artist == "Vincent van Gogh")
                {
                    VincentButton.gameObject.SetActive(true);
                }
            }
        }
    }

    private void OpenJohannesCanvas()
    {
        JohannesButton.gameObject.SetActive(false);
        JohannesCanvas.gameObject.SetActive(true);
    }
    private void OpenVincentCanvas()
    {
        VincentButton.gameObject.SetActive(false);
        VincentCanvas.gameObject.SetActive(true);
    }

}
