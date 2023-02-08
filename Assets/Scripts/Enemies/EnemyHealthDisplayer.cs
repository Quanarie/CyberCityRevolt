using System;
using System.Collections;
using UnityEngine;

public class EnemyHealthDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject fillingOfSlider;
    
    private EnemyHealth health;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer fillingSpriteRenderer;

    private float elapsedTimeSinceLastUpdate = 0f;
    private bool isHiding = false;
    private const float TIME_TO_HIDE_HEALTHBAR = 1f;
    private const int FRAMES_TO_HIDE = 180;

    private void Start()
    {
        if (!transform.parent.TryGetComponent(out health))
        {
            Debug.LogError("No health component on the parent of: " + gameObject.name);
        }
        
        if (!TryGetComponent(out spriteRenderer))
        {
            Debug.LogError("No SpriteRenderer on: " + gameObject.name);
        }
        
        if (!fillingOfSlider.TryGetComponent(out fillingSpriteRenderer))
        {
            Debug.LogError("No SpriteRenderer on: " + fillingOfSlider.gameObject.name);
        }
        
        health.ChangedHp.AddListener(UpdateDispalyedHealth);
    }

    private void Update()
    {
        FlipSide();
        elapsedTimeSinceLastUpdate += Time.deltaTime;
        if (elapsedTimeSinceLastUpdate > TIME_TO_HIDE_HEALTHBAR && !isHiding)
        {
            StartCoroutine(Hide());
        }
    }

    private void UpdateDispalyedHealth(int currentHp)
    {
        StopAllCoroutines();
        isHiding = false;
        elapsedTimeSinceLastUpdate = 0f;
        Color currColor = spriteRenderer.color;
        spriteRenderer.color = new Color(currColor.r, currColor.g, currColor.b, 1f);
        currColor = fillingSpriteRenderer.color;
        fillingSpriteRenderer.color = new Color(currColor.r, currColor.g, currColor.b, 1f);
        
        Vector3 scale = fillingOfSlider.transform.localScale;
        float from0To1 = currentHp / (float)health.GetMaxHp();
        fillingOfSlider.transform.localScale = new Vector3(from0To1, scale.y, scale.z);
        
        float sizeX = spriteRenderer.bounds.size.x;
        float offsetX = sizeX * (1 - from0To1) / 2;
        fillingOfSlider.transform.localPosition = new Vector3(-offsetX, 0f, 0f);
    }
    
    private void FlipSide()
    {
        float parentScaleXSign = Mathf.Sign(transform.parent.localScale.x);
        Vector3 scale = transform.localScale;
        if ((parentScaleXSign < 0 && scale.x > 0) || (parentScaleXSign > 0 && scale.x < 0))
        {
            transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        }
    }

    private IEnumerator Hide()
    {
        isHiding = true;
        float step = spriteRenderer.color.a / FRAMES_TO_HIDE;
        for (int i = 0; i < FRAMES_TO_HIDE; i++)
        {
            Color currColor = spriteRenderer.color;
            spriteRenderer.color = new Color(currColor.r, currColor.g, currColor.b, currColor.a - step);
            currColor = fillingSpriteRenderer.color;
            fillingSpriteRenderer.color = new Color(currColor.r, currColor.g, currColor.b, currColor.a - step);
            yield return 0;
        }
        isHiding = false;
    }
}