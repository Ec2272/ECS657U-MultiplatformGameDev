using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerLoader : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 alwaysOffset = new Vector3(4f, 0f, 4f);

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        var data = SaveSystem.Load();
        if (data == null) return;
        if (data.sceneIndex != SceneManager.GetActiveScene().buildIndex) return;
        Vector3 spawn = new Vector3(data.x, data.y, data.z) + alwaysOffset;

        //TP a Rigidbody player
        bool wasKinematic = rb.isKinematic;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = new Vector3(data.x, data.y, data.z);
        rb.isKinematic = wasKinematic;

    }
}


