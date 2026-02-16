using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform target;
    
    void LateUpdate()
    {
        var newPos = target.position;
        newPos.z = -1;
        transform.position = newPos;
    }
}
