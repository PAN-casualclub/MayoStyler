using System.Collections;
using UnityEngine;

public class CandyButtons : MonoBehaviour
{
    public CandyData candyData;

    private void Awake()
    {
        candyData.FillCandyStock();
    }

    private void Start()
    {
        GetComponentInChildren<MeshFilter>().sharedMesh = candyData.mesh;
    }

    public void GetCandy()
    {
        GameObject currentCandy = candyData.GetCandy();
        currentCandy.transform.position = transform.GetChild(0).position;
        currentCandy.SetActive(true);
        Candy comp = currentCandy.AddComponent<Candy>();
        comp.data = candyData;
        InputManager.ForceControllObject(currentCandy);
    }

}
