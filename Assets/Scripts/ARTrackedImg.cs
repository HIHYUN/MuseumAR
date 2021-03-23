using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class ARTrackedImg : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public List<GameObject> _objectList = new List<GameObject>(); // Prefabs 들을 list 형태로 저장
    private Dictionary<string, GameObject> _prefabDic = new Dictionary<string, GameObject>(); // Image들을 이름으로 호출하기 위해 사전형 변수

    void Awake()
    {
        foreach( GameObject obj in _objectList)
        {
            string tName = obj.name;
            _prefabDic.Add(tName,obj);
        }
    }

    // Script가 활성화 될때마다 실행
    private void OnEnable()
    {
        // ARTrackedImageManager 에서 이미지가 바뀌면 ImageChanged 함수 실행
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }
    private void OnDisable() 
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach(ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }

        foreach(ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }

    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name; //인식하는 이미지의 이름을 가져옴
        GameObject tObj = _prefabDic[name];  // 이미지의 이름과 맞는 prefab의 오브젝트 선언
        tObj.transform.position = trackedImage.transform.position;  // 현재 이미지의 위치와 맞도록 오브젝트 위치 선정
        tObj.transform.rotation = trackedImage.transform.rotation;
        tObj.SetActive(true); // 오브젝트 활성화

    }
}
