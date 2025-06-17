using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesController : MonoBehaviour
{
    private int life;
    public GameObject[] hearts;
    private bool dead = false;

    [SerializeField] public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        life = hearts.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead == true)
        {
            //Debug.Log("Death animation will be play");
            playerController.KillPlayer();
        }
    }

    public void ReduceLives(int Death)
    {
        //Debug.Log("Death :"+ Death);
        //Debug.Log("Life :"+ life);
        life -= Death;
        //Debug.Log("Life :" + life);
        Destroy(hearts[life].gameObject);
        if (life < 1)
        {
            dead = true;
            life = 1;
        }
    }
}
