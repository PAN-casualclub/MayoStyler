using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraMovement : MonoBehaviour
{
    Camera cameraInComponent;
    [SerializeField , Range(.1f,1)] float camSpeed;
    private void Start()
    {
        EventListener.PhaseEndedActions += SetFOW;
        cameraInComponent = GetComponent<Camera>();
    }

    public void SetFOW(GameplayPhase CurrentZoom)
    {
        StartCoroutine(SetZoom((int)CurrentZoom));
    }

    IEnumerator SetZoom(float toSet)
    {
        float modifier = .1f + camSpeed;

        if (toSet <= cameraInComponent.fieldOfView)
            modifier *= -1;

        while (true)
        {
            if (modifier < 0)
            {
                cameraInComponent.fieldOfView += modifier;
                yield return null;
                if (cameraInComponent.fieldOfView <= toSet)
                    break;
            }
            else
            {
                cameraInComponent.fieldOfView += modifier;
                yield return null;
                if (cameraInComponent.fieldOfView >= toSet)
                    break;
            }

            yield return null;

        }

        EventListener.OnCameraMovementFinished();

    }


}
