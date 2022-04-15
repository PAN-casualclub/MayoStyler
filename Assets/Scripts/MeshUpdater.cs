using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshUpdater : MonoBehaviour
{
    public SkinnedMeshRenderer meshRenderer;
    public MeshCollider _collider;

    public void UpdateCollider()
    {
        Mesh colliderMesh = new Mesh();
        meshRenderer.BakeMesh(colliderMesh);
        _collider.sharedMesh = null;
        _collider.sharedMesh = colliderMesh;
    }

    private void Update()
    {
        UpdateCollider();
    }
}
