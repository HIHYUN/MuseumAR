using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArtJsonData
{   
    public Data[] data;

    /*
    public ArtJsonData()
    {
        LoadJson();
    }

    public void LoadJson()
    {
        TextAsset jsonFile = Resources.Load("MuseumInformation") as TextAsset; 
        data = JsonUtility.FromJson<Data[]>(jsonFile.text);
    }*/

    [System.Serializable]
    public class Data
    {
        public string name;
        public string artist;
        public string date;
        public string description;
        public string human;
    }
}

