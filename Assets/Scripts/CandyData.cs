using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Candy Data")]
public class CandyData : ScriptableObject
{
    public Mesh mesh;
    public Material material;
    Queue<GameObject> CandyStock = new Queue<GameObject>();

    const int Red_Line = 2;


    public void FillCandyStock(int produceCount = 5)
    {
        produceCount = produceCount - Red_Line;

        for (int i = 0; i < produceCount; i++)
        {
            GameObject candy = new GameObject("Candy");
            MeshFilter filterOfCandy = candy.AddComponent<MeshFilter>();
            filterOfCandy.sharedMesh = mesh;
            MeshRenderer rendererOfCandy = candy.AddComponent<MeshRenderer>();
            rendererOfCandy.material = material;
            candy.SetActive(false);
            CandyStock.Enqueue(candy);
        }

    }

    public GameObject GetCandy()
    {
        if (CandyStock.Count <= Red_Line)
        {
            FillCandyStock();
        }
        GameObject candyGo = CandyStock.Dequeue();
        return candyGo;
    }

    public void BackToPool(GameObject gameObject)
    {
        CandyStock.Enqueue(gameObject);
        gameObject.SetActive(false);

    }
}
