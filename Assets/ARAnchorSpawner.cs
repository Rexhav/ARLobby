using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Netcode;
using System.Collections.Generic;

public class ARAnchorSpawner : NetworkBehaviour
{
    public ARRaycastManager raycastManager;
    public ARAnchorManager anchorManager;
    public GameObject markerPrefab;
    public GameObject cubePrefab;

    private List<ARRaycastHit> hits = new();

    void Update()
    {
        if (!IsOwner) return;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPos = Input.GetTouch(0).position;
            if (raycastManager.Raycast(touchPos, hits, TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;

                GameObject anchorObject = new GameObject("ARAnchor");
                anchorObject.transform.position = hitPose.position;
                anchorObject.transform.rotation = hitPose.rotation;
                ARAnchor anchor = anchorObject.AddComponent<ARAnchor>();

                if (anchor != null)
                {
                    GameObject marker = Instantiate(markerPrefab, anchor.transform.position, anchor.transform.rotation);
                    marker.GetComponent<NetworkObject>().Spawn(true);

                    // Save marker reference
                    if (MainManager2.Instance != null && MainManager2.Instance.arColab != null)
                      MainManager2.Instance.marker = marker.transform;
                    GameObject cube = Instantiate(cubePrefab, marker.transform.position, marker.transform.rotation, marker.transform);
                    cube.GetComponent<NetworkObject>().Spawn(true);
                }
            }
        }
    }
}