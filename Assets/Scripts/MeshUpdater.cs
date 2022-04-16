using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
[CustomEditor(typeof(MeshUpdater))]
public class MeshUpdaterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MeshUpdater updater = (MeshUpdater)target;

        if (GUILayout.Button("Update"))
        {
            updater.UpdateCollider();
        }
    }
}
#endif

public class MeshUpdater : MonoBehaviour
{
    public SkinnedMeshRenderer meshRenderer;
    public MeshCollider _collider;

    private void OnEnable()
    {
        ModelBehaviour.meshUpdaters.Add(this);
    }

    private void OnDisable()
    {
        ModelBehaviour.meshUpdaters.Remove(this);
    }

    public void UpdateCollider()
    {
        Mesh colliderMesh = new Mesh();
        meshRenderer.BakeMesh(colliderMesh);
        _collider.sharedMesh = null;
        _collider.sharedMesh = colliderMesh;
    }

}
