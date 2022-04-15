using UnityEngine;

public class Candy : MonoBehaviour, IInteractable
{
    public bool attached;
    public bool bIsFixed;
    internal CandyData data;
    Vector3 steelPos;

    private void Update()
    {
        if (bIsFixed)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, 10, 1 << 3))
        {
            Vector3 targetPos = new Vector3(hit.point.x,hit.point.y,hit.point.z - .01f);
            steelPos = targetPos;
            transform.position = Vector3.Lerp(transform.position, targetPos, .05f);
            transform.rotation = Quaternion.FromToRotation(Vector3.forward,hit.normal);
            attached = true;
            InputManager.ObjectInControl = true;
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
            MeshFilter filter = GetComponent<MeshFilter>();
            ModelBehaviour.MeshFilters.Add(filter);
            // Go To The Merge Center
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
