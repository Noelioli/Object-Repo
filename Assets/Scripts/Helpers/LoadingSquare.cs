using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSquare : MonoBehaviour
{
    //The purpose of this file is to make the canvas image follow the player/disable on gameover

    private Transform target;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (this.isActiveAndEnabled)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                target = playerObject.transform;

                Vector3 playerScreenPos = mainCamera.WorldToScreenPoint(target.position);
                transform.position = playerScreenPos;
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
