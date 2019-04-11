using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScreenFixSprite : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void SetSpriteSize()
    {
        if (spriteRenderer.sprite == null) return;
        float width = 0;
        float height = 0;
        if ((1.0f * Screen.width / Screen.height) >= (spriteRenderer.size.x / spriteRenderer.size.y))
        {
            width = Camera.main.orthographicSize * Screen.width * 2 / Screen.height;
            height = spriteRenderer.size.y * width / spriteRenderer.size.x;//Camera.main.orthographicSize * Screen.height * 2 / Screen.width;                    
        }
        else {
            height = Camera.main.orthographicSize * Screen.height * 2 / Screen.width;
            width = spriteRenderer.size.x * height / spriteRenderer.size.y;            
        }
        spriteRenderer.size = new Vector2(width, height);
    }

    // Update is called once per frame
    void Update()
    {
        SetSpriteSize();
        //Camera.main.orthographicSize = 19.2f * Screen.width / Screen.height * 0.5f;
    }
}
