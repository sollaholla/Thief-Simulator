using UnityEngine;

public class HousePowerOutage : MonoBehaviour
{
    public Renderer houseRenderer;

    public void Activate()
    {
        foreach (var material in houseRenderer.materials)
        {
            material.color *= 0.25f;
        }
    }

    public void Deactivate()
    {
        foreach (var material in houseRenderer.materials)
        {
            material.color /= 0.25f;
        }
    }
}
