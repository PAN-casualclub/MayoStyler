using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static Transform CurrentDragged;
    public static IInteractable CurrentIntegrated;
    public static bool ObjectInControl;
    static IInteractable lastInteractable;
    public static bool LockInputs;
    public LayerMask InteracLayers;
    public LayerMask InjectionLayers;
    public float InputSensivity;
    public RaycastHit[] hitted = new RaycastHit[1];
    Vector3 offsetOnControl;
    float fixedZ;

    private void Start()
    {

        EventListener.PhaseEndedActions += CloseInputs;
        EventListener.CameraMoveEndActions += OpenInputs;
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
            if (CurrentDragged == null || EventListener.CurrentPhase == GameplayPhase.Painting || ObjectInControl)
            {
                return;
            }

            CurrentDragged.position = GetInputPosition() + offsetOnControl;
        }


        if (Input.GetMouseButtonUp(0))
        {
            offsetOnControl = Vector3.zero;

            if (CurrentDragged == null )
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

                if (lastInteractable != null && lastInteractable != CurrentIntegrated)
                    lastInteractable.OnChanged();

                lastInteractable = CurrentIntegrated;
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
        if (CurrentIntegrated != null)
            CurrentIntegrated.OnExit();

        if (EventListener.CurrentPhase != GameplayPhase.Painting)
        {
            CurrentDragged = null;
            CurrentIntegrated = null;
            lastInteractable = null;
        }
        ObjectInControl = false;
    }

    public static void ForceControllObject(GameObject toControll)
    {
        ForceReleaseInput();
        CurrentDragged = toControll.transform;
        if (CurrentDragged.TryGetComponent(out CurrentIntegrated))
        {
            CurrentIntegrated.OnInteracted();

            if (lastInteractable != null && lastInteractable != CurrentIntegrated)
                lastInteractable.OnChanged();

            lastInteractable = CurrentIntegrated;
        }
    }

    public static void CloseInputs(GameplayPhase gameplayPhase)
    {
        LockInputs = true;
    }

    public static void OpenInputs()
    {
        LockInputs = false;
    }
}


