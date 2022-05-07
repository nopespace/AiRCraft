using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

// Modified from ARCore foundation Samples
// commented by qg
namespace UnityEngine.XR.ARFoundation.Samples
// namespace UnityEngine.XR.ARFoundation
{
    [RequireComponent(typeof(ARAnchorManager))]
    [RequireComponent(typeof(ARRaycastManager))]
    public class AnchorCreator : MonoBehaviour
    {
        [SerializeField]
        GameObject m_Prefab;

        public Transform Panel_menu;

        public GameObject prefab
        {
            get => m_Prefab;
            set => m_Prefab = value;
        }

        public void RemoveAllAnchors()
        {
            foreach (var anchor in m_Anchors)
            {
                Destroy(anchor.gameObject);
            }
            m_Anchors.Clear();
        }

        void Awake()
        {
            m_RaycastManager = GetComponent<ARRaycastManager>();
            m_AnchorManager = GetComponent<ARAnchorManager>();
        }

        ARAnchor CreateAnchor(in ARRaycastHit hit)
        {
            ARAnchor anchor = null;

            // If we hit a plane, try to "attach" the anchor to the plane
            if (hit.trackable is ARPlane plane)
            {
                var planeManager = GetComponent<ARPlaneManager>();
                if (planeManager)
                {
                    var oldPrefab = m_AnchorManager.anchorPrefab;
                    m_AnchorManager.anchorPrefab = prefab;
                    anchor = m_AnchorManager.AttachAnchor(plane, hit.pose);
                    m_AnchorManager.anchorPrefab = oldPrefab;
                    return anchor;
                }
            }

            // Note: the anchor can be anywhere in the scene hierarchy
            var gameObject = Instantiate(prefab, hit.pose.position, Quaternion.identity);

            // Make sure the new GameObject has an ARAnchor component
            anchor = gameObject.GetComponent<ARAnchor>();
            if (anchor == null)
            {
                anchor = gameObject.AddComponent<ARAnchor>();
            }

            return anchor;
        }

        void Update()
        {
            // TrackableHit hit;
            // TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
            //     TrackableHitFlags.FeaturePointWithSurfaceNormal;

            // if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
            // {
            //     // Use hit pose and camera pose to check if hittest is from the
            //     // back of the plane, if it is, no need to create the anchor.
            //     if ((hit.Trackable is DetectedPlane) &&
            //         Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
            //             hit.Pose.rotation * Vector3.up) < 0)
            //     {
            //         Debug.Log("Hit at back of the current DetectedPlane");
            //     }
            //     else
            //     {
            //         // Instantiate prefab at the hit pose.
            //         var gameObject = Instantiate(prefab, hit.Pose.position, hit.Pose.rotation);

            //         // Compensate for the hitPose rotation facing away from the raycast (i.e.
            //         // camera).
            //         gameObject.transform.Rotate(0, _prefabRotation, 0, Space.Self);

            //         // Create an anchor to allow ARCore to track the hitpoint as understanding of
            //         // the physical world evolves.
            //         var anchor = hit.Trackable.CreateAnchor(hit.Pose);

            //         // Make game object a child of the anchor.
            //         gameObject.transform.parent = anchor.transform;
            //     }
            // }
            // comment from qg: here i commented the old code in order to try new version
            if (Input.touchCount == 0)
                return;

            var touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Began)
                return;

            // Raycast against planes and feature points
            const TrackableType trackableTypes =
                TrackableType.FeaturePoint |
                TrackableType.PlaneWithinPolygon;

            // Perform the raycast
            if (m_RaycastManager.Raycast(touch.position, s_Hits, trackableTypes))
            {
                // Raycast hits are sorted by distance, so the first one will be the closest hit.
                var hit = s_Hits[0];

                // Create a new anchor
                var anchor = CreateAnchor(hit);
                if (anchor)
                {
                    // Remember the anchor so we can remove it later.
                    m_Anchors.Add(anchor);
                }
            }
        }

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        List<ARAnchor> m_Anchors = new List<ARAnchor>();

        ARRaycastManager m_RaycastManager;

        ARAnchorManager m_AnchorManager;
    }
}