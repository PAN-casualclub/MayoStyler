using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Dress Data Container")]
public class DressIndexContainer : ScriptableObject
{
    [Tooltip("Default Indexs")]
    public int[] IndexsOfMeshs;
    [Tooltip("Runtime Indexs")]
    internal int PreviousIndexOfMesh;
    internal int CurrentIndexOfMesh;

    private Action dressing;

    public Action DressAction { get => dressing; set => dressing = value; }

    public void ChangeDressMeshs(int indexOfMesh)
    {
        CurrentIndexOfMesh = indexOfMesh;
        dressing();
        PreviousIndexOfMesh = indexOfMesh;
    }

}
