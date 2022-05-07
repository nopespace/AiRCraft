using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
namespace UnityEngine.XR.ARFoundation.Samples
{
[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
public class Begin : MonoBehaviour
{
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;

    [SerializeField]
    GameObject welcome_page;

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
            return;

        var touch = Input.GetTouch(0);        
        const TrackableType trackableTypes =
                TrackableType.FeaturePoint |
                TrackableType.PlaneWithinPolygon;
        if(m_RaycastManager.Raycast(touch.position, s_Hits, trackableTypes)){
            welcome_page.SetActive(false);
        }
    }
}
}
