using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFocus : MonoBehaviour
{
    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera underCam;
   
    private Player player;
    public GameObject weaponToActivate;


    // Start is called before the first frame update

    private void Awake()
    {
        player = GetComponent<Player>();
        underCam.Priority = 1;
    }
    void Start()
    {
        // Initialize player in case it's not set in Awake
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        // Check if the 5th weapon is equipped and the player clicks the mouse button
        if (player != null && player.equipWeaponIndex == 4 && Input.GetMouseButtonDown(0) && player.ammo > 0)
        {
            // Check if the weapon's name is "Weapon explosion"
            if (weaponToActivate != null && weaponToActivate.name == "Weapon explosion")
            {
                ActivateWeapon();
                StartCoroutine(SwitchCamera());
            }
        }
    }

    void ActivateWeapon()
    {
        if (weaponToActivate != null)
        {
            weaponToActivate.SetActive(true); // Activate the specific weapon
        }
    }


    IEnumerator SwitchCamera()
    {
        if (underCam == null)
        {
            Debug.LogError("underCam is not assigned.");
            yield break; // Exit if underCam is not assigned
        }

        underCam.Priority = 11; // Higher than any other virtual cameras
        //Priorty는 우선순위를 의미함.

        // Wait for 3 to 5 seconds
        yield return new WaitForSeconds(Random.Range(3, 5));

        // Disable underCam
        underCam.Priority = 1; // Lower than main virtual camera
    }
}
