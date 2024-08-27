using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Player player;
    private float horizontal, vertical;
    private Vector2 lookTarget;
    [SerializeField] private NukePickup nuke;
    private bool cooledDown = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        lookTarget = Input.mousePosition;

        if (GameManager.GetInstance().isPlaying) // Added to protect from trying to use powerups after the game ends
        {
            if (GameManager.GetInstance().machineGunActive && Input.GetMouseButton(0) && cooledDown) //Rapid fire mode with cooldown
                StartCoroutine(RapidfireCooldown());
            else if (!GameManager.GetInstance().machineGunActive && Input.GetMouseButtonDown(0))
                player.Shoot();

            if (Input.GetKeyDown(KeyCode.E) && GameManager.GetInstance().nukes > 0) //Activates nuke
                nuke.DestroyAll(); //(Probably should of put this on the gamemanager or something.)
        }
    }

    IEnumerator RapidfireCooldown()
    {
        //Cools down the gun so it's not a bullet a frame
        cooledDown = false;
        player.Shoot();
        yield return new WaitForSeconds(0.2f);
        cooledDown = true;
    }

    private void FixedUpdate()
    {
        player.Move(new Vector2(horizontal, vertical), lookTarget);
    }
}
