using Unity.Netcode;
using UnityEngine;

namespace Multi
{
    public class RelativeTransformSync : NetworkBehaviour
    {
        private NetworkVariable<Vector3> relativePos = new NetworkVariable<Vector3>();
        private NetworkVariable<Quaternion> relativeRot = new NetworkVariable<Quaternion>();

        void Update()
        {
            if (MainManager2.Instance == null || MainManager2.Instance.marker == null)
                return;

            Transform marker = MainManager2.Instance.marker;

            if (IsOwner)
            {
                relativePos.Value = marker.InverseTransformPoint(transform.position);
                relativeRot.Value = Quaternion.Inverse(marker.rotation) * transform.rotation;
            }
            else
            {
                transform.position = marker.TransformPoint(relativePos.Value);
                transform.rotation = marker.rotation * relativeRot.Value;
            }
        }
    }
}