using UnityEngine;

public class Candy : MonoBehaviour, IInteractable
{
    public bool bIsFixed;
    public bool bIsAttached;
    public bool bOnSurface;
    internal CandyData data;
    internal Transform parentSuppose;
    internal Vector3 snapPos;
    int tryCacth;
    private void OnEnable()
    {
        bIsFixed = false;
        bIsAttached = false;
        bOnSurface = false;
        tryCacth = 0;
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
            tryCacth++;
            transform.parent = parentSuppose;
            transform.position = snapPos;
            if (transform.parent != null)
            {
                bIsAttached = true;
            }

            if (tryCacth > 5)
                Destroy(this);

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
            transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
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
