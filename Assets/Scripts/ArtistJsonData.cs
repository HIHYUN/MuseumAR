using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArtistJsonData
{   
    public Artist[] artist;

    [System.Serializable]
    public class Artist
    {
        public string name;
        public string history;
        public Image[] image;
    }

    [System.Serializable]
    public class Image
    {
        public string url;
        public string about;
    }
}

