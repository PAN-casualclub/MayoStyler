using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraMovement : MonoBehaviour
{
    Camera cameraInComponent;
    public static List<Action> OnCameraFinished = new List<Action>();

    private void Start()
    {
        EventListener.PhaseFinishedActions += SetFOW;
        cameraInComponent = GetComponent<Camera>();
    }

    public void SetFOW(GameplayPhase CurrentZoom)
    {
        StartCoroutine(SetZoom((int)CurrentZoom));
    }

    IEnumerator SetZoom(float toSet)
    {
        float modifier = .1f;

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

        if (OnCameraFinished.Count > 0)
        {
            foreach (var func in OnCameraFinished)
            {
                func();
            }
        }

        OnCameraFinished.Clear();
    }


}
