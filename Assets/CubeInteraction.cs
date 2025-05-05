using Unity.Netcode;
using UnityEngine;

public class CubeInteraction : NetworkBehaviour
{
    private float initialDistance;
    private Vector3 initialScale;

    void Update()
    {
        if (!IsOwner) return;

        if (Input.touchCount == 2)
        {
            var touch0 = Input.GetTouch(0);
            var touch1 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touch0.position, touch1.position);
                initialScale = transform.localScale;
            }
            else if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                float currentDistance = Vector2.Distance(touch0.position, touch1.position);
                float scaleFactor = currentDistance / initialDistance;
                Vector3 newScale = initialScale * scaleFactor;
                UpdateScaleServerRpc(newScale);
            }
        }

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            float rotY = Input.GetTouch(0).deltaPosition.x * 0.2f;
            transform.Rotate(0, rotY, 0);
            UpdateRotationServerRpc(transform.rotation);
        }
    }

    [ServerRpc]
    void UpdateScaleServerRpc(Vector3 scale)
    {
        transform.localScale = scale;
    }

    [ServerRpc]
    void UpdateRotationServerRpc(Quaternion rot)
    {
        transform.rotation = rot;
    }
}