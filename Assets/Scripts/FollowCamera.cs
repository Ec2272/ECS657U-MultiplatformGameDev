using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FollowCamera : MonoBehaviour
{
    [Header("Target to follow")]
    public Transform target;

    [Header("Offset from target")]
    public Vector3 offset = new Vector3(0f, 6f, -6f);

    [Header("Follow smoothness")]
    public float smoothSpeed = 10f;

    [Header("World Z bounds")]
    public float minZ = -20f;
    public float maxZ =  20f;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 desiredPosition = target.position + offset;

        // Clamp world Z
        desiredPosition.z = Mathf.Clamp(desiredPosition.z, minZ, maxZ);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}