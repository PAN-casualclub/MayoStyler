using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(ModelBehaviour))]
public class ModelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ModelBehaviour behaviour = (ModelBehaviour)target;

        if (GUILayout.Button("Merge And Put"))
        {
            behaviour.MergeMeshs();
        }
    }

}


public class ModelBehaviour : MonoBehaviour
{
    public DressIndexContainer DressIndexContainer;
    public List<MeshUpdater> meshUpdaters = new List<MeshUpdater>(); 
    public static List<MeshFilter> MeshFilters = new List<MeshFilter>();
    public MeshFilter ToBake;
    public MeshUpdater meshUpdater;
    bool closedDefault;
    Animator ownAnimator;
    string lastanimation;
    ArmIK[] armIKs ;


    private void Start()
    {
        armIKs = GetComponents<ArmIK>();
        DressIndexContainer.PreviousIndexOfMesh = -1;
        DressIndexContainer.DressAction = ChangeModelDress;
        EventListener.PhaseEndedActions += ChangePose;
        ownAnimator = GetComponent<Animator>();
    }

    public void ChangePose(GameplayPhase gameplayPhase)
    {
        switch (gameplayPhase)
        {
            case GameplayPhase.IKDrag:
                break;
            case GameplayPhase.MesureDia:
                Change_Pose("tpose");
                break;
            case GameplayPhase.BikiniModel:
                foreach (var ik in armIKs)
                {
                    ik.enabled = false;
                }
                Change_Pose("idle");
                meshUpdater.enabled = true;
                break;
            case GameplayPhase.Painting:
                foreach (var ik in armIKs)
                {
                    ik.enabled = true;
                }

                for (int i = 0; i < meshUpdaters.Count; i++)
                {
                    meshUpdaters[i].enabled = false;
                }
                break;
            case GameplayPhase.Customing:
                break;
            case GameplayPhase.LastPose:
                foreach (var ik in armIKs)
                {
                    ik.enabled = false;
                }
                Change_Pose("lastpose");
                break;
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

    internal void MergeMeshs()
    {
        Mesh bakedMesh = new Mesh();
        MeshFilter[] filters = MeshFilters.ToArray();
        CombineInstance[] combineInstances = new CombineInstance[filters.Length];
        for (int i = 0; i < filters.Length; i++)
        {
            combineInstances[i].subMeshIndex = 0;
            combineInstances[i].mesh = filters[i].sharedMesh;
            combineInstances[i].transform = filters[i].transform.localToWorldMatrix;
        }
        bakedMesh.CombineMeshes(combineInstances);
        ToBake.sharedMesh = bakedMesh;
        ToBake.GetComponent<SkinnedMeshRenderer>().sharedMesh = bakedMesh;
        ToBake.GetComponent<MeshCollider>().sharedMesh = bakedMesh;

    }
}
