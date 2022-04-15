using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HandIntersection : MonoBehaviour, IInteractable
{
    public Transform TartgetToIntersect;
    public HandIntersection Linked;
    Vector3 StartPos;
    internal bool isComplete;
    Collider _collider;

    private void Awake()
    {
        EventListener.IKPhaseStartedActions += OpenIKHandler; 
    }

    private void Start()
    {
        StartPos = transform.localPosition;
         GetComponent<Rigidbody>().isKinematic = true;
        _collider = GetComponent<Collider>();
        _collider.enabled = true;
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == TartgetToIntersect)
        {
            isComplete = true;
            TartgetToIntersect.gameObject.SetActive(false);
            InputManager.ForceReleaseInput();
            CloseHandlerInput();
            if (CheckAllAccomplished())
            {
                OnAccomplished();
            }
        }
    }

    private bool CheckAllAccomplished()
    {
        if (Linked.isComplete)
        {
            return true;
        }

        return false;
    }

    private void OnAccomplished()
    {
        CloseIKHandlers();
    }

    public void CloseHandlerInput()
    {
        GetComponent<Renderer>().enabled = false;
        _collider.enabled = false;
    }

    public void CloseHandler()
    {
            CloseHandlerInput();
            isComplete = false;
    }

    private void CloseIKHandlers()
    {
        InputManager.ForceReleaseInput();
        EventListener.OnPhaseEnded(GameplayPhase.MesureDia);
        CloseHandler();
        Linked.CloseHandler();
    }


    public void OpenIKHandler()
    {
        TartgetToIntersect.gameObject.SetActive(true);
        GetComponent<Renderer>().enabled = true;
        _collider.enabled = true;
        transform.localPosition = StartPos;

    }

    public void OnInteracted()
    {
    }

    public void OnExit()
    {

    }

    public void OnChanged()
    {
    }
}
