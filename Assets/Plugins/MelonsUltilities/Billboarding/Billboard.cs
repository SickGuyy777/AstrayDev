using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private bool rotateX = true;
    [SerializeField] private bool rotateY = true;

    private Camera cam;


    private void Awake() => cam = Camera.main;

    private void Update()
    {
        if (cam == null)
        {
            Debug.LogError("Cannot Billboard without a camera in scene");
            return;
        }

        Vector3 rot = Quaternion.LookRotation(GetDirectionTowardsCamera(), transform.up).eulerAngles;
        Debug.DrawRay(transform.position, GetDirectionTowardsCamera() * 5, Color.red);
        
        rot.x = rotateX ? rot.x : 0;
        rot.y = rotateY ? rot.y : 0;
        rot.z = 0;
        
        transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);
    }
    
    private Vector3 GetDirectionTowardsCamera() => (cam.transform.position - transform.position).normalized;
}
