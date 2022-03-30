using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public float value = 1000;
    private bool eaten = false;
    AudioSource sound;

    void Start() {
        sound =  GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            updateValues(other.transform.transform.gameObject);
            eat_food();
        }
    }

    void updateValues( GameObject pacman  )
    {
        GameObject[] food = GameObject.FindGameObjectsWithTag("Food");
        foreach (GameObject f in food)
        {
            Food script = f.GetComponent<Food>();
            script.value = Manhattan_distance(pacman, f);
        }
    }

    float Manhattan_distance(GameObject pacman, GameObject food )
    {
        float pacman_x = pacman.transform.position.x;
        float pacman_z = pacman.transform.position.z;
        float food_x = food.transform.position.x;
        float food_z = food.transform.position.z;
        return Mathf.Abs(pacman_x - food_x) + Mathf.Abs(pacman_z - food_z);
    }

    void eat_food() 
    {
        if(!eaten){
            Score.score += 1;
            eaten = true;
            GetComponent<Renderer>().enabled = false;
            DialogManager.PassLevelTriggerDialog();
            if(sound != null) {
                sound.Play();
            }
        }
    }
}
