using UnityEngine;

public class Candy : MonoBehaviour, IInteractable
{
    internal CandyData data;

    bool bIsFixed;
    bool bIsAttached;
    bool bOnSurface;
    Transform parentSuppose;
    Vector3 snapPos;
    Vector3 snapRot;
    int tryCacth_SnapErr;

    private void OnEnable()
    {
        parentSuppose = null;
        bIsFixed = false;
        bIsAttached = false;
        bOnSurface = false;
        tryCacth_SnapErr = 0;
    }


    private void Update()
    {
        if (bIsAttached || !gameObject.activeInHierarchy)
        {
            Destroy(this);
            return;
        }

        if (bIsFixed)
        {
            tryCacth_SnapErr++;
            transform.parent = parentSuppose;
            transform.position = snapPos;
            transform.rotation = Quaternion.FromToRotation(Vector3.forward, snapRot);
            if (transform.parent != null)
            {
                bIsAttached = true;
            }

            if (tryCacth_SnapErr > 5)
                Destroy(gameObject);

            return;
        }

        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, Vector3.forward, out raycastHit, 1, 1 << 8))
        {
            parentSuppose = raycastHit.transform;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10, 1 << 3))
        {
            snapPos = new Vector3(hit.point.x, hit.point.y, hit.point.z - .01f);
            transform.position = Vector3.Lerp(transform.position, snapPos, .05f);
            snapRot = hit.normal;
            InputManager.ObjectInControl = true;
            bOnSurface = true;
        }
        else
        {
            bOnSurface = false;
            InputManager.ObjectInControl = false;
        }
    }

    public void OnChanged()
    {

    }

    public void OnExit()
    {
        if (bOnSurface)
        {
            bIsFixed = true;
        }
        else
        {
            data.BackToPool(gameObject);
            InputManager.ForceReleaseInput(true);
        }

    }

    public void OnInteracted()
    {
    }
}
