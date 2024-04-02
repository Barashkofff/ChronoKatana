using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dumping;
    public Vector2 offset;
    private Transform player;

    void Start()
    {
        FindPlayer(PlayerController.instance.GetFacing());
    }


    void Update()
    {
        int currentX = Mathf.RoundToInt(player.position.x);

        Vector3 target;
        if (PlayerController.instance.GetFacing())
        {
            target = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);

        }
        else 
        {
            target = new Vector3(player.position.x - offset.x, player.position.y + offset.y, transform.position.z);
        }

        Vector3 currentPosition = Vector3.Lerp(transform.position, target, dumping * Time.deltaTime);
        transform.position = currentPosition;
    }

    public void FindPlayer(bool playerIsRight)
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if (playerIsRight )
        {
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(player.position.x - offset.x, player.position.y - offset.y, transform.position.z);
        }

    }
}
