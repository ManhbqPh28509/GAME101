using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public float followSpeed = 5f;
    public Transform target;
    public Vector3 offSet;

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredPos = target.position + offSet;
        Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);
        smoothPos.z = -10f;
        transform.position = smoothPos;
        
        
    }
}
