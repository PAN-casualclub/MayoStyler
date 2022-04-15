using UnityEngine;
using UnityEngine.UI;

public class DressPerformer : MonoBehaviour
{
    public DressData dressData;

    private void Start()
    {
        InitData();
    }

    private void InitData()
    {
        if (dressData.ImageOfButton != null)
            GetComponent<Image>().sprite = dressData.ImageOfButton;
    }

    public void Dress()
    {
        dressData.IndexContainer.ChangeDressMeshs(dressData.IndexOfMesh);
    }
}
