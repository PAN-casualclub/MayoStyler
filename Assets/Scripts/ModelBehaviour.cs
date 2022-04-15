using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;


public class ModelBehaviour : MonoBehaviour
{
    public DressIndexContainer DressIndexContainer;
    public List<MeshUpdater> meshUpdaters = new List<MeshUpdater>(); 
    public MeshUpdater meshUpdater;
    bool closedDefault;
    Animator ownAnimator;
    string lastanimation;
    ArmIK[] armIKs ;

    private void OnEnable()
    {
        UIPerformer.CurrentModel = transform.parent.gameObject;
    }

    private void Start()
    {
        armIKs = GetComponents<ArmIK>();
        DressIndexContainer.PreviousIndexOfMesh = -1;
        DressIndexContainer.DressAction = ChangeModelDress;
        EventListener.PhaseEndedActions += ChangePose;
        ownAnimator = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        EventListener.PhaseEndedActions -= ChangePose;
    }

    /// <summary>
    /// Phase changing last frame opr.
    /// </summary>
    /// <param name="gameplayPhase"></param>
    public void ChangePose(GameplayPhase gameplayPhase)
    {
        switch (gameplayPhase)
        {
            case GameplayPhase.Start:
                break;
            case GameplayPhase.IKDrag:
                IKActivity(true);
                break;
            case GameplayPhase.MesureDia:
                Change_Pose("tpose");
                break;
            case GameplayPhase.BikiniModel:
                IKActivity(false);
                Change_Pose("idle");
                StartCoroutine(UpdateMeshs());
                break;
            case GameplayPhase.Painting:
                IKActivity(true);
                break;
            case GameplayPhase.Customing:
                IKActivity(false);
                Change_Pose("lastpose");
                StartCoroutine(UpdateMeshs());
                break;
            case GameplayPhase.LastPose:

                break;
            
        }
    }

    private void IKActivity(bool state)
    {
        foreach (var ik in armIKs)
        {
            ik.enabled = state;
        }
    }

    private void Change_Pose(string poseName)
    {
        if (!string.IsNullOrEmpty(lastanimation))
        {
            ownAnimator.ResetTrigger(lastanimation);
        }
        ownAnimator.SetTrigger(poseName);
        lastanimation = poseName;
    }
    

    public void ChangeModelDress()
    {
        if (!closedDefault)
        {
            closedDefault = true;
            for (int i = 0; i < DressIndexContainer.IndexsOfMeshs.Length; i++)
            {
                transform.GetChild(DressIndexContainer.IndexsOfMeshs[i]).gameObject.SetActive(false);
            }
        }

        GameObject lastUsed = null;
        if (DressIndexContainer.PreviousIndexOfMesh > 1)
        {
            lastUsed = transform.GetChild(DressIndexContainer.PreviousIndexOfMesh).gameObject;
        }

        if (lastUsed != null)
            lastUsed.SetActive(false);

        lastUsed = transform.GetChild(DressIndexContainer.CurrentIndexOfMesh).gameObject;
        lastUsed.SetActive(true);
    }

    IEnumerator UpdateMeshs()
    {
        for (int i = 0; i < meshUpdaters.Count; i++)
        {
            meshUpdaters[i].enabled = true;
        }
        meshUpdater.enabled = true;

        yield return null;

        for (int i = 0; i < meshUpdaters.Count; i++)
        {
            meshUpdaters[i].enabled = false;
        }
        meshUpdater.enabled = false;
    }
}
