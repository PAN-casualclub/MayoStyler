using UnityEngine;

public class Candy : MonoBehaviour, IInteractable
{
    public bool attached;
    public bool bIsFixed;
    public bool bIsAttached;
    internal CandyData data;
    private void Update()
    {
        if (bIsAttached)
            return;

        if (bIsFixed)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(transform.position,Vector3.forward,out raycastHit,10,1<<3))
            {
                transform.position = Vector3.Lerp(transform.position, raycastHit.point, 1f);
                bIsAttached = true;
                transform.parent = raycastHit.transform;
            }
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, 10, 1 << 3))
        {
            Vector3 targetPos = new Vector3(hit.point.x,hit.point.y,hit.point.z - .01f);
            transform.position = Vector3.Lerp(transform.position, targetPos, .05f);
            transform.rotation = Quaternion.FromToRotation(Vector3.forward,hit.normal);
            InputManager.ObjectInControl = true;
            attached = true;
        }
        else
        {
            attached = false;
        }
    }

    public void OnChanged()
    {

    }

    public void OnExit()
    {
        if (attached)
        {
            bIsFixed = true;
        }
        else
        {
            data.BackToPool(gameObject);
        }

    }

    public void OnInteracted()
    {
    }
}
