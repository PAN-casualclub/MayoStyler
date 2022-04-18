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
    string lastanimation;
    ArmIK[] armIKs;
    int hash1;
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
                StartCoroutine(ChangePosByDelay("tposefast", "idle", false));
                break;
            case GameplayPhase.Painting:
                StartCoroutine(ChangePosByDelay("idle", "tpose", true));
                break;
            case GameplayPhase.Customing:
                break;
            case GameplayPhase.LastPose:
                Change_Pose("lastpose");
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
        ownAnimator.enabled = true;
        if (!string.IsNullOrEmpty(lastanimation))
        {
            ownAnimator.ResetTrigger(lastanimation);
        }
        ownAnimator.SetTrigger(poseName);
        lastanimation = poseName;
    }

    IEnumerator ChangePosByDelay(string poseName)
    {
        if (!string.IsNullOrEmpty(lastanimation))
        {
            ownAnimator.ResetTrigger(lastanimation);
        }
        ownAnimator.SetTrigger(poseName);
        lastanimation = poseName;
        yield return new WaitForSeconds(.3f);

        ownAnimator.enabled = false;
        UpdateMeshs();
    }

    IEnumerator ChangePosByDelay(string poseName, string nextPoseName, bool includeUpdate)
    {
        IKActivity(false);
        Change_Pose(poseName);
        yield return new WaitForSeconds(.3f);
        Change_Pose(nextPoseName);
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
