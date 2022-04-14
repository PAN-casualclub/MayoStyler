using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelBehaviour : MonoBehaviour
{
    Animator ownAnimator;

    private void Start()
    {
        EventListener.PhaseFinishedActions += ChangePose;
        ownAnimator = GetComponent<Animator>();
    }

    public void ChangePose(GameplayPhase gameplayPhase)
    {
        switch (gameplayPhase)
        {
            case GameplayPhase.Default:
                break;
            case GameplayPhase.MesureDia:
                Change_TPose();
                break;
            case GameplayPhase.BikiniModel:
                break;
            case GameplayPhase.Customing:
                break;
            case GameplayPhase.LastPose:
                break;
        }
    }

    private void Change_TPose()
    {
        ownAnimator.SetTrigger("TPose");
    }
}
