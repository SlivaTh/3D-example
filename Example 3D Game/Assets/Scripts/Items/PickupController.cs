using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public Transform equipPosition;
    public Transform player;
    public Transform fpsCam;
    public Rigidbody rb;
    public BoxCollider coll;
    public int idWeapon;
    /// <summary>
    /// ID = 1 : Sword
    /// ID = 2 : Machete
    /// ID = 3 : Axe
    /// ID = 4 : ...
    /// </summary>
    public float pickUpRange = 6f;
    public float minRange = 1f;
    public float dropForwardForce;
    public float dropUpForce;

    public bool equipped;
    public static bool slotFull;
    public static bool canPick;

    [Header("Transform")]
    public Vector3 pos;
    public Vector3 rot;
    public Vector3 scale;

    private bool canBePicked;

    private void Start()
    {
        equipped = false;

        canBePicked = true;

        canPick = true;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        equipPosition = GameObject.FindGameObjectWithTag("WeaponSlot").transform;

        if (!equipped)
        {
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        else if (equipped)
        {
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
        }
    }

    private void Update()
    {
        //Check if Player is in Range and E is pressed;
        Vector3 distanceToPlayer = player.position - transform.position;
        if(!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull)
        {
            if (canBePicked && canPick)
            {
                PickUp();
            }
        }

        //Pick Up without press E
        if (!equipped && distanceToPlayer.magnitude <= minRange && !slotFull)
        {
            if (canBePicked && canPick)
            {
                PickUp();
            }
        }

        //Drop weapon if Q is pressed;
        if (equipped && Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }
    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        player.GetComponent<PlayerMove>().PickUpAnimation();

        // Make RB kinematic
        rb.isKinematic = true;
        coll.isTrigger = true;

        // Make weapon a child of the equipPosition
        transform.SetParent(equipPosition);
        transform.localPosition = pos;
        transform.localRotation = Quaternion.Euler(rot);
        transform.localScale = scale;

        player.GetComponent<PlayerMove>().CurrentWeaponID(idWeapon);
    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        //Fix Bug (cannot pick immediately machete after drop a sword)
        canPick = false;
        Invoke("CanPickStatic", 0.1f);

        player.GetComponent<PlayerMove>().CurrentWeaponID(0);

        //Make RB kinematic
        rb.isKinematic = false;
        coll.isTrigger = false;

        //Set parent == null
        transform.SetParent(null);

        //Add Force
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpForce, ForceMode.Impulse);

        //Pause between picked
        canBePicked = false;
        Invoke("PickUpPermise", 1f);
    }

    private void PickUpPermise()
    {
        canBePicked = true;
    }

    private void CanPickStatic()
    {
        canPick = true;
    }

    public int ReturnId()
    {
        return idWeapon;
    }


}
