using UnityEngine;

public class CameraFollowHead : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private float smoothSpeed = 50f;


    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, head.position, smoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, head.rotation, smoothSpeed * Time.deltaTime);
    }
}
