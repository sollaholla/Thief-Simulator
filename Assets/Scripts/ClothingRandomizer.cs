using UnityEngine;

public class ClothingRandomizer : MonoBehaviour
{
    public ClothingSet[] clothingSets;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var set in clothingSets)
        {
            var mat = Instantiate(set.renderer.material);
            mat.color = set.possibleColors[Random.Range(0, set.possibleColors.Length - 1)];
            set.renderer.material = mat;
        }
    }
}

[System.Serializable]
public class ClothingSet
{
    public Renderer renderer;
    public Color[] possibleColors;
}