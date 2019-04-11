using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaddleRotation : MonoBehaviour
{
    [SerializeField]
    RectTransform paddle;
    [SerializeField]
    BoxCollider2D paddleCollider;

    [SerializeField]
    Transform leftPoint;
    [SerializeField]
    Transform rightPoint;

    [SerializeField]
    float speed = 20;

    [SerializeField]
    float size = 1;

    bool isPause = false;
    bool isLeft = false;
    
    Transform rotationPoint;
    Vector3 direction;
    public bool IsPause
    {
        get
        {
            return isPause;
        }

        set
        {
            isPause = value;
        }
    }


    private void Start()
    {
        Shift();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space)) { 
            isLeft = !isLeft;
            Shift();
        }
        SetSize(size);
        Rotate();
    }   

    void Shift() {
        direction = Vector3.forward;// isLeft ? Vector3.forward : Vector3.back;
        rotationPoint = isLeft ? leftPoint : rightPoint;
        SetPivot(paddle, isLeft ? new Vector2(0, 0.5f) : new Vector2(1, 0.5f));
    }

    void Rotate() {        
        if (!IsPause)
            transform.RotateAround(rotationPoint.position, direction, speed * Time.deltaTime);
    }

    void SetSize(float size) {
        //if (this.size == size) return;
        float oldSize = this.size;
        this.size = size;
        paddle.sizeDelta = new Vector2(size, paddle.sizeDelta.y);
        paddleCollider.size = new Vector2(size, paddle.sizeDelta.y);
        paddleCollider.offset = isLeft ? new Vector2(size / 2, 0) : new Vector2(-size / 2, 0);
    }

    public void SetPivot(RectTransform rectTransform, Vector2 pivot)
    {
        Vector3 deltaPosition = rectTransform.pivot - pivot;    // get change in pivot
        deltaPosition.Scale(rectTransform.rect.size);           // apply sizing
        deltaPosition.Scale(rectTransform.localScale);          // apply scaling
        deltaPosition = rectTransform.rotation * deltaPosition; // apply rotation

        rectTransform.pivot = pivot;                            // change the pivot
        rectTransform.localPosition -= deltaPosition;           // reverse the position change

    }
}
