using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArtJsonData
{   
    public Data[] data;

    [System.Serializable]
    public class Data
    {
        public string name;
        public string artist;
        public string date;
        public string description;
        public string audio;
    }
}

