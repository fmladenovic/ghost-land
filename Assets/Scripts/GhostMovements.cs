using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovements : MonoBehaviour
{

    public float speed = 25f;
    public float maxSpeed = 5f;

    private float speedHolder;

    float vision_range = 1f;

    private Rigidbody _rigidbody;
    private string _current_direction;

    private AudioSource _sound;

    private string convert_to_string(Vector3 direction )
    {
        if(direction.x == 1 && direction.z == 0)
        {
            return "right";
        }
        else if (direction.x == -1 && direction.z == 0)
        {
            return "left";
        }
        else if (direction.x == 0 && direction.z == 1)
        {
            return "up";
        }
        else if (direction.x == 0 && direction.z == -1)
        {
            return "down";
        }
        return "";
    }  


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _sound =  GetComponent<AudioSource>();
        _current_direction = "";
    }

    // Update is called once per frame
    void Update()
    {

        List<GameObject> near_food = get_near_food();
        Vector3 move = get_best_move(near_food);

        string string_direction = convert_to_string(move);
        if (string_direction != _current_direction)
        {
            _current_direction = string_direction;
            _rigidbody.velocity = Vector3.zero;
        }

        Vector3 velocity = _rigidbody.velocity;
        if (velocity.magnitude < maxSpeed)
        {
            _rigidbody.AddForce(move * speed);
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


    private List<GameObject> get_near_food()
    {
        GameObject[] food = GameObject.FindGameObjectsWithTag("Food");
        List<GameObject> near_food = new List<GameObject>();
        float start_x = transform.position.x - vision_range;
        float end_x = transform.position.x + vision_range;
        float start_z = transform.position.z - vision_range;
        float end_z = transform.position.z + vision_range;
        foreach( GameObject f in food )
        {
            if(start_x <= f.transform.position.x && f.transform.position.x <= end_x && start_z <= f.transform.position.z && f.transform.position.z <= end_z)
            {
                near_food.Add(f);
            }
        }
        return near_food;
    }

    private Vector3 get_best_move(List<GameObject>  near_food)
    {
        float value = 1000.0f;
        Vector3 best = Vector3.zero;
        foreach( GameObject food in near_food )
        {
            Food script = food.GetComponent<Food>();
            if(value > script.value)
            {
                Vector3 to_check = food.transform.position - transform.position;
                to_check = Vector3.Normalize(to_check);
                to_check = new Vector3(Mathf.Round(to_check.x), 0, Mathf.Round(to_check.z));
                value = script.value;
                best = to_check;
            }
        }
        return best;
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            DialogManager.HitByGhostTriggerDialog();
            Destroy(collision.gameObject);
            if(_sound != null) {
                _sound.Play();
            }
        }
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
