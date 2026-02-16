using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform target;
    
    void LateUpdate()
    {
        var newPos = target.position;
        newPos.z = -1;
        Debug.Log(newPos);
        transform.position = newPos;
    }
}
