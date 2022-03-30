using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 25;
    public float maxSpeed = 5f;

    private float speedHolder = -1;

    private Rigidbody _rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        float verticalMove = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(horizontalMove, 0, verticalMove);
        Vector3 velocity = _rigidbody.velocity;
        if (velocity.magnitude < maxSpeed)
        {
            _rigidbody.AddForce(move * speed);
        }
        if (horizontalMove == 0 && verticalMove == 0)
        {
            _rigidbody.velocity = new Vector3(0, 0, 0);
        }

        if(move.x < 0 && this.gameObject.transform.eulerAngles.y != 90.0f) 
        {
            float rotationY = 90.0f - this.gameObject.transform.eulerAngles.y;
            this.gameObject.transform.Rotate(0.0f, rotationY, 0.0f, Space.Self);
        } 
        else if(move.x > 0  && this.gameObject.transform.eulerAngles.y != -90.0f) 
        {
            float rotationY = -90.0f - this.gameObject.transform.eulerAngles.y;
            this.gameObject.transform.Rotate(0.0f, rotationY, 0.0f, Space.Self);
        } 
        else if(move.z < 0  && this.gameObject.transform.eulerAngles.y != 0.0f) 
        {
            float rotationY = 0.0f - this.gameObject.transform.eulerAngles.y;
            this.gameObject.transform.Rotate(0.0f, rotationY, 0.0f, Space.Self);
        }
        else if(move.z > 0  && this.gameObject.transform.eulerAngles.y != 180.0f)  
        {
            float rotationY = 180.0f - this.gameObject.transform.eulerAngles.y;
            this.gameObject.transform.Rotate(0.0f, rotationY, 0.0f, Space.Self);
        } 
        StopStart();
    }

    void StopStart() {
        if(DialogManager.isVisible) {
            speedHolder = 25;
            speed = 0;
        }
        else if(speedHolder > 0) {
            speed = 25;
            speedHolder = -1;
        }
    }
}
