using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHandlerPoint : MonoBehaviour ,IInteractable
{
    [SerializeField] HandIntersection ownerIntersection;
    public void OnInteracted()
    {
    }

    public void OnExit()
    {
        transform.position = ownerIntersection.transform.position;
    }

    public void OnChanged()
    {
    }

}
