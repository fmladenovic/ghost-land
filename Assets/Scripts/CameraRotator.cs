using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public float speed = 5.0f;
    // Update is called once per frame
    void Update()
    {
        // todo https://www.youtube.com/watch?v=iuygipAigew
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
