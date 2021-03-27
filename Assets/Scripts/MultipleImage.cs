using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using System.Collections;



[RequireComponent (typeof(ARTrackedImageManager))]
public class MultipleImage : MonoBehaviour
{

    private ARTrackedImageManager trackedImageManager;
    // struct 구조 prefab
    public PlaceablePrefab[] objectlist;
    private Dictionary<string, GameObject> prefabDic = new Dictionary<string,GameObject>();
    public Camera camera;

    void Awake() 
    {   
        // Get ARTrackedImageManager Component
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        // 시작 시 지정한 오브젝트 사전형에 할당. 
        foreach(PlaceablePrefab prefab in objectlist)
        {
            GameObject obj = Instantiate(prefab.prefab, Vector3.zero, Quaternion.identity);
            obj.name =  prefab.name;
            prefabDic.Add(prefab.name, obj);
        }
    }


    // 발생한 이벤트 활성화 및 subscirbe.
    void OnEnable() 
    {
        trackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    // 이미지 track 이벤트 발생
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
            prefabDic[img.referenceImage.name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage img)
    {   
        // tracked 된 이미지 이름 할당
        string imgName = img.referenceImage.name;
        // 해당 이미지의 이름과 같은 prefab 할당
        GameObject prefab = prefabDic[imgName];

        // tracking 상태가 normal이면 버튼 UI와 이미지 위치 일치 시킴
        if(img.trackingState == TrackingState.Tracking)
        {   
            
            Vector3 screenPos = camera.WorldToScreenPoint(img.transform.position);
            prefab.transform.position = new Vector3(screenPos.x, screenPos.y, prefab.transform.position.z);

            prefab.SetActive(true);
        }
        // 이미지가 tracking에서 벗어난 상태일 경우 prefab disable
        else if(img.trackingState == TrackingState.Limited || img.trackingState == TrackingState.None)
        {
            prefab.SetActive(false);    
        }
    }

    // struct 구조 Prefab
    [System.Serializable]
    public struct PlaceablePrefab
    { 
        public string name;
        public GameObject prefab;
    }

    

}
