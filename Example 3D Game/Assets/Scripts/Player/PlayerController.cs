using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMove playerMove;

    private Camera cam;
    private void Start()
    {
        cam = Camera.main;

        playerMove = this.GetComponent<PlayerMove>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //To see where are you clickung
            /*Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                Debug.Log("We hit: " + hit.collider.name + " " + hit.point);
            }*/

            playerMove.MeeleAttack();

        }
    }
}
