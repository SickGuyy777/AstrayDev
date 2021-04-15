using UnityEngine;

public class ChangeMaterialSelectionResponse : MonoBehaviour, ISelectionResponse
{
    [SerializeField] private Material material;
    private Renderer thisRenderer;
    private Material defaultMaterial;

    
    private void Awake()
    {
        thisRenderer = GetComponentInChildren<Renderer>();
        defaultMaterial = thisRenderer.material;
    }

    public void OnSelect()
    {
        thisRenderer.material = material;
    }

    public void OnDeselect()
    {
        thisRenderer.material = defaultMaterial;
    }
}
