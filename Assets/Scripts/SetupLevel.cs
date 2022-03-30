using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetupLevel : MonoBehaviour
{
    public GameObject wall;
    public GameObject ground;
    public GameObject player;
    public GameObject food;
    public GameObject background;


    public List<GameObject> ghosts;

    public float groundWidth = 12f;
    public float groundLength = 12f;

    List<GameObject> objects = new List<GameObject>(); // when you want to go to next level / or restart

    void Start()
    {
        SetupGround();
        SetupBackground();
        // transform.position -= new Vector3(0, -0.25f, 0);
        SetupWalls();
        SetupPlayer();
        SetupGhosts();
        SetupFood();
    }

    void SetupPlayer()
    {
        GameObject gamePlayer = Instantiate(player);
        gamePlayer.name = "Player";
        objects.Add(gamePlayer);
    }

    void SetupGhosts()
    {
        GameObject gameGhosts = new GameObject();
        gameGhosts.name = "Ghosts";
        objects.Add(gameGhosts);

        Quaternion quaternion = Quaternion.Euler(0, 0, 0); // setup this smarter
        Vector3 position = new Vector3(-1.5f, 0, -1.75f); // setup this smarter -4.875f, 0, -4.875f
        for (int i = 0; i < ghosts.Count; i++)
        {
            GameObject gameGhost = Instantiate(ghosts[i], position, quaternion, gameGhosts.transform);
            gameGhost.name = "Ghost" + (i + 1);

/*            GameObject gameGhost1 = Instantiate(ghosts[i], position, quaternion, gameGhosts.transform);
            gameGhost.name = "Ghost" + (i + 2);*/
        }
    }

    void SetupGround()
    {
        GameObject gameGround = Instantiate(ground, new Vector3(0, -0.65f, 0), Quaternion.identity, transform);
        gameGround.transform.localScale = new Vector3(groundWidth, 0.3f, groundLength);
        gameGround.name = "Ground";
        objects.Add(gameGround);
    }

    void SetupBackground()
    {
        Quaternion quaternion = Quaternion.Euler(90, 0, 0);
        GameObject gameBackground = Instantiate(background, new Vector3(0, -1.2f, 0), quaternion, transform);
        gameBackground.transform.localScale = new Vector3(groundWidth*3, groundLength*3, 0.1f);
        gameBackground.name = "Background";
        objects.Add(gameBackground);
    }

    void SetupWalls()
    {
        GameObject walls = new GameObject();
        walls.name = "Walls";
        walls.transform.SetParent(transform);
        objects.Add(walls);


        generateWall( -0.125f, -5.75f, 5.875f, Direction.HORIZONTAL, walls.transform);
        generateWall( -5.75f, -0.125f, 5.875f, Direction.VERTICAL, walls.transform);
        generateWall( -0.125f,  5.5f, 5.875f, Direction.HORIZONTAL, walls.transform);
        generateWall(  5.5f, -0.125f, 5.875f, Direction.VERTICAL, walls.transform);


        generateWall(-4.25f, 1.375f, 2.875f, Direction.VERTICAL, walls.transform);
        generateWall(-3.50f, -1.25f, 1.0f, Direction.HORIZONTAL, walls.transform);

        generateWall(-2.75f, 2.125f, 2.125f, Direction.VERTICAL, walls.transform);

        generateWall(-1.25f, 4.25f, 1.25f, Direction.VERTICAL, walls.transform);
        generateWall(-1.25f, 1.00f, 1f, Direction.VERTICAL, walls.transform);
        generateWall(-0.125f, 0.25f, 1.325f, Direction.HORIZONTAL, walls.transform);

        generateWall(-0.125f, -1.25f, 1.325f, Direction.HORIZONTAL, walls.transform);


        generateWall(1.375f, 4.0f, 1.375f, Direction.HORIZONTAL, walls.transform);
        generateWall(0.25f, 2.875f, 1.375f, Direction.VERTICAL, walls.transform);
        generateWall(1.375f, 1.75f, 1.375f, Direction.HORIZONTAL, walls.transform);

        generateWall(2.50f, 0.25f, 1.75f, Direction.VERTICAL, walls.transform);
        generateWall(4.0f, -0.125f, 2.875f, Direction.VERTICAL, walls.transform);
        generateWall(4.0f, 4.625f, 0.875f, Direction.VERTICAL, walls.transform);

        generateWall(3.25f, -2.75f, 1.0f, Direction.HORIZONTAL, walls.transform);
        generateWall(0.625f, -2.75f, 0.625f, Direction.HORIZONTAL, walls.transform);
        generateWall(-1.25f, -2.00f, 1f, Direction.VERTICAL, walls.transform);



        generateWall(0.25f, -3.50f, 1f, Direction.VERTICAL, walls.transform);
        generateWall(-2.75f, -2.00f, 1f, Direction.VERTICAL, walls.transform);

        generateWall(-3.50f, -2.75f, 1.0f, Direction.HORIZONTAL, walls.transform);

        generateWall(-0.125f, -4.25f, 2.875f, Direction.HORIZONTAL, walls.transform);
        generateWall(4.625f, -4.25f, 0.875f, Direction.HORIZONTAL, walls.transform);
        generateWall(-4.875f, -4.25f, 0.875f, Direction.HORIZONTAL, walls.transform);
    }


    enum Direction
    {
        VERTICAL,
        HORIZONTAL
    }

    void generateWall(float x, float z, float size, Direction direction, Transform parent)
    {
        Vector3 position = new Vector3(x, 0, z);
        Quaternion quaternion =  direction == Direction.HORIZONTAL ? Quaternion.Euler(90, 0, 90) : quaternion = Quaternion.Euler(90, 0, 0);
        GameObject newWall = Instantiate(
            wall,
            position,
            quaternion,
            parent
        );
        newWall.name = "Wall";
        newWall.transform.localScale = new Vector3(0.5f, size, 0.5f);
    }


    void SetupFood()
    {
        GameObject foods = new GameObject();
        foods.name = "Foods";
        foods.transform.SetParent(transform);
        objects.Add(foods);

        float to_width = (groundWidth / 2 - 0.25f);
        float from_width = -1 * to_width;

        float to_length = (groundLength / 2 - 0.25f);
        float from_length = -1 * to_length;

        for (float i = from_width; i < to_width; i += 0.75f)
        {
            for(float j = from_length; j < to_length; j += 0.75f)
            {
                if(!check(i,j))
                { 
                    Vector3 position = new Vector3(i, 0, j);
                    Quaternion quaternion = Quaternion.Euler(0, 0, 0);

                    GameObject newFood = Instantiate(
                        food,
                        position,
                        quaternion,
                        foods.transform
                    );
                    newFood.name = "Food";
                    Score.totalPosibleScore += 1;
                }
            }
        }
    }

    private bool check(float position_x, float position_z)
    {
        float x;
        float z;
        float size;
        bool horizontal;
        float start_x;
        float end_x;
        float start_z;
        float end_z;
        foreach (GameObject game_object in objects)
        {   
            if (game_object.name == "Walls")
            {
                foreach (Transform wall in game_object.transform)
                {
                    x = wall.position.x;
                    z = wall.position.z;
                    size = wall.localScale.y;
                    horizontal = wall.rotation.z == 0.5;
                    if (horizontal)
                    {
                        start_x = x - size;
                        end_x = x + size;
                        start_z = z - 0.5f;
                        end_z = z + 0.5f;
                    }
                    else
                    {
                        start_x = x - 0.5f;
                        end_x = x + 0.5f;
                        start_z = z - size;
                        end_z = z + size;
                    }
                    bool check_x = start_x < position_x && position_x < end_x;
                    bool check_z = start_z < position_z && position_z < end_z;
                    if(check_x && check_z)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

}
