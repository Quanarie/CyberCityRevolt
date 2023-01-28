using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [HideInInspector] public UnityEvent Dying;
    [HideInInspector] public UnityEvent<int> ChangedHp;
    
    [SerializeField] protected int maxHitPoints;
    
    protected int currentHitPoints;
    
    private SpriteRenderer spriteRenderer;
    private Shader whiteShader;
    private Shader standardShader;
    
    private const float BLINK_TIME = 0.125f;

    protected virtual void Start()
    {
        currentHitPoints = maxHitPoints;
        if (!TryGetComponent(out spriteRenderer))
        {
            Debug.LogError("No SpriteRenderer on: " + gameObject.name);
        }

        standardShader = spriteRenderer.material.shader;
        whiteShader = Shader.Find("GUI/Text Shader");
        if (whiteShader == null)
        {
            Debug.LogError("Not found WhiteShader for: " + gameObject.name);
        }
    }

    public virtual void ReceiveDamage(int dmg)
    {
        currentHitPoints -= dmg;
        if (currentHitPoints <= 0)
        {
            Death();
        }

        ChangedHp?.Invoke(currentHitPoints);
        StartCoroutine(BlinkWhenHurt());
    }

    IEnumerator BlinkWhenHurt()
    {
        var mat = spriteRenderer.material;
        mat.shader = whiteShader;
        yield return new WaitForSeconds(BLINK_TIME);
        mat.shader = standardShader;
    }

    protected virtual void Death()
    {
        Dying?.Invoke();
    }

    public int GetMaxHp() => maxHitPoints;
}
