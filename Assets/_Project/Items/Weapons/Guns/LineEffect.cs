using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineEffect : MonoBehaviour
{
    [SerializeField] private LineRenderer lr;
    private float speed;

    private Color endStartColor;
    private Color endFinalColor;


    public void Setup(Vector2 start, Vector2 end, Color color, float size = .1f, float duration = .5f)
    {
        lr.startColor = color;
        lr.endColor = color;
        
        lr.startWidth = size;
        lr.endWidth = size;
        
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        
        speed = 1 / duration;
        
        endStartColor = lr.startColor;
        endStartColor.a = 0;
        
        endFinalColor = lr.endColor;
        endFinalColor.a = 0;
        
        Destroy(gameObject, duration);
    }
    
    private void Update()
    {
        if(lr == null)
            lr = GetComponent<LineRenderer>();
        
        lr.startColor = Color.Lerp(lr.startColor, endStartColor, speed * Time.deltaTime * 2.5f);
        lr.endColor = Color.Lerp(lr.endColor, endFinalColor, speed * Time.deltaTime * 2.5f);
        
        lr.startWidth = Mathf.Lerp(lr.startWidth, 0, speed * Time.deltaTime * 2.5f);
        lr.endWidth = Mathf.Lerp(lr.endWidth, 0, speed * Time.deltaTime * 2.5f);
    }
}
