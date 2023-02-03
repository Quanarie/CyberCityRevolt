using UnityEngine;
using UnityEngine.InputSystem;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (!TryGetComponent(out spriteRenderer))
        {
            Debug.LogError("No SpriteRenderer on crosshair object");
        }

        spriteRenderer.sprite = sprites[PlayerPrefs.GetInt("Crosshair")];
    }

    private void FixedUpdate()
    {
        Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = new Vector3(mousePosInWorld.x, mousePosInWorld.y, 0f);
    }
}
