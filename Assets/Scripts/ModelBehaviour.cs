using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;


public class ModelBehaviour : MonoBehaviour
{
    public DressIndexContainer DressIndexContainer;
    public static List<MeshUpdater> meshUpdaters = new List<MeshUpdater>();
    public MeshUpdater meshUpdater;
    bool closedDefault;
    Animator ownAnimator;
    ArmIK[] armIKs;
    int lastAnimationId;
    static readonly int TriggerHash_Idle = Animator.StringToHash("idle");
    static readonly int TriggerHash_Tpose = Animator.StringToHash("tpose");
    static readonly int TriggerHash_TposeFast = Animator.StringToHash("tposefast");
    static readonly int TriggerHash_LastPose = Animator.StringToHash("lastpose");

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
                break;
            case GameplayPhase.BikiniModel:
                IKActivity(false);
                Change_Pose(TriggerHash_TposeFast);
                break;
            case GameplayPhase.Painting:
                UpdateMeshs();
                break;
            case GameplayPhase.Customing:
                UpdateMeshs();
                break;
            case GameplayPhase.LastPose:
                Change_Pose(TriggerHash_LastPose);
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

    private void Change_Pose(int poseId)
    {
        ownAnimator.enabled = true;
        if (lastAnimationId != poseId)
        {
            if (lastAnimationId != 0)
                ownAnimator.ResetTrigger(lastAnimationId);
        }
        ownAnimator.SetTrigger(poseId);
        lastAnimationId = poseId;
    }

    IEnumerator ChangePosByDelay(int poseId)
    {
        if (lastAnimationId != poseId)
        {
            if (lastAnimationId != 0)
                ownAnimator.ResetTrigger(lastAnimationId);
        }
        ownAnimator.SetTrigger(poseId);
        lastAnimationId = poseId;
        yield return new WaitForSeconds(.3f);

        UpdateMeshs();
    }

    IEnumerator ChangePosByDelay(int poseId, int nextPoseId, bool includeUpdate)
    {
        IKActivity(false);
        Change_Pose(poseId);
        yield return new WaitForSeconds(.3f);
        Change_Pose(nextPoseId);
        if (includeUpdate)
        {
            yield return new WaitForSeconds(.3f);
            ownAnimator.enabled = false;
            UpdateMeshs();
        }

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

    void UpdateMeshs()
    {
        for (int i = 0; i < meshUpdaters.Count; i++)
        {
            if (meshUpdaters[i].gameObject.activeInHierarchy)
            {
                meshUpdaters[i].UpdateCollider();
            }
        }

    }
}
