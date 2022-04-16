using System;
using PaintIn3D;
using UnityEngine;

public class SprayIntersection : MonoBehaviour, IInteractable
{
    [SerializeField] P3dVrTool trackTool;
    Vector3 startPosition;
    Transform OnWorkingArea;
    Transform OwnerManager;

    private void Start()
    {
        startPosition = transform.position;
        OwnerManager = transform.parent;
        OnWorkingArea = OwnerManager.parent;
    }

    private void OnDisable()
    {
        transform.parent = OwnerManager.transform;
        transform.position = startPosition;
    }

    public void OnExit()
    {
        trackTool.OnTriggerRelease.Invoke();
        trackTool.enabled = false;
        for (int i = 0; i < OwnerManager.childCount; i++)
        {
            OwnerManager.GetChild(i).gameObject.SetActive(true);
        }
        transform.parent = OwnerManager.transform;
    }

    public void OnInteracted()
    {
        trackTool.enabled = true;
        transform.parent = OnWorkingArea;
        for (int i = 0; i < OwnerManager.childCount; i++)
        {
            OwnerManager.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void OnChanged()
    {
        transform.parent = OwnerManager.transform;
        transform.position = startPosition;
    }
}