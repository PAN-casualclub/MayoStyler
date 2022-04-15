using System;
using PaintIn3D;
using UnityEngine;

public class SprayIntersection : MonoBehaviour, IInteractable
{
    [SerializeField] P3dVrTool trackTool;
    Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    public void OnExit()
    {
        trackTool.OnTriggerRelease.Invoke();
        trackTool.enabled = false;
        // Fix Rot
    }

    public void OnInteracted()
    {
        trackTool.enabled = true;
    }

    public void OnChanged()
    {
        transform.position = startPosition;
    }
}