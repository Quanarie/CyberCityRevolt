using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [HideInInspector] public UnityEvent Dying;
    [HideInInspector] public UnityEvent<int> ChangedHp;
    
    [SerializeField] protected int maxHitPoints;
    
    protected int currentHitPoints;
    
    private SpriteRenderer _spriteRenderer;
    private Shader _whiteShader;
    private Shader _standardShader;
    
    private const float BLINK_TIME = 0.125f;

    protected virtual void Start()
    {
        currentHitPoints = maxHitPoints;
        if (!TryGetComponent(out _spriteRenderer))
        {
            Debug.LogError("No SpriteRenderer on: " + gameObject.name);
        }

        _standardShader = _spriteRenderer.material.shader;
        _whiteShader = Shader.Find("GUI/Text Shader");
        if (_whiteShader == null)
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
        Material mat = _spriteRenderer.material;
        mat.shader = _whiteShader;
        yield return new WaitForSeconds(BLINK_TIME);
        mat.shader = _standardShader;
    }

    protected virtual void Death()
    {
        Dying?.Invoke();
    }

    public int GetMaxHp() => maxHitPoints;
}
