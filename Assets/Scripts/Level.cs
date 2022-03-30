using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    Text text;
    public static int level;

    void Start()
    {
        text = GetComponent<Text>();
        level = SceneManager.GetActiveScene().buildIndex;
        text.text = level.ToString();
    }
}
