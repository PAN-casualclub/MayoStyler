using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static Transform CurrentDragged;
    public static IInteractable CurrentIntegrated;
    public static bool LockInputs;
    public LayerMask InteracLayers;
    public LayerMask InjectionLayers;
    public float FixedPosition_Z;
    public float InputSensivity;
    public RaycastHit[] hitted = new RaycastHit[1];

    Vector3 offsetOnControl;
    float fixedZ;

    private void Start()
    {
        EventListener.PhaseFinishedActions += CloseInputs;
    }

    void Update()
    {
        if (LockInputs)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            InjectRay(Input.mousePosition);

            if (CurrentDragged == null)
            {
                if (EventListener.CurrentPhase == GameplayPhase.MesureDia)
                    UIPerformer.RotateRight();
                return;
            }

            fixedZ = Camera.main.WorldToScreenPoint(CurrentDragged.position).z;
            offsetOnControl = CurrentDragged.position - GetInputPosition();
        }

        if (Input.GetMouseButton(0))
        {
            if (CurrentDragged == null)
            {
                return;
            }

            CurrentDragged.position = GetInputPosition() + offsetOnControl;
        }


        if (Input.GetMouseButtonUp(0))
        {
            if (CurrentDragged == null)
            {
                if (EventListener.CurrentPhase == GameplayPhase.MesureDia)
                    UIPerformer.StopRotate();
                return;
            }

            ForceReleaseInput();
        }
    }

    public Vector3 GetInputPosition()
    {
        Vector3 inputPos = Input.mousePosition;
        inputPos.z = fixedZ;
        return Camera.main.ScreenToWorldPoint(inputPos);
    }


    private void InjectRay(Vector3 inputPosition)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(inputPosition);
        if (Physics.Raycast(ray, out hit, 100, InteracLayers))
        {
            CurrentDragged = hit.transform;

            if (CurrentDragged.TryGetComponent(out CurrentIntegrated))
            {
                CurrentIntegrated.OnInteracted();
            }
        }

    }

    public Transform HitInjections(Vector3 inputPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(inputPosition);
        hitted =  Physics.RaycastAll(ray,10, InjectionLayers);
        return hitted[0].transform;
    }

    public static  void ForceReleaseInput()
    {
        CurrentDragged = null;
        CurrentIntegrated = null;
    }

    public static void CloseInputs(GameplayPhase gameplayPhase)
    {
        LockInputs = true;
    }
}


