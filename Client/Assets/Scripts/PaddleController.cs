using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer paddle;
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
        if (Input.GetMouseButtonUp(0)||Input.GetKeyUp(KeyCode.Space)) { 
            isLeft = !isLeft;
            Shift();
        }
        SetSize(size);
        Rotate();
    }   

    void Shift() {
        direction =  isLeft ? Vector3.forward : Vector3.back;
        rotationPoint = isLeft ? leftPoint : rightPoint;        
    }

    void Rotate() {        
        if (!IsPause)
            transform.RotateAround(rotationPoint.position, direction, speed * Time.deltaTime);
    }

    void SetSize(float size) {
        //if (this.size == size) return;        
        this.size = size;
        Vector3 delta = new Vector3((size - paddle.size.x) / 2, 0, 0);
        if (isLeft) {
            leftPoint.SetParent(paddle.transform.parent);
            paddle.transform.localPosition += delta;
            rightPoint.transform.localPosition += delta;            
            leftPoint.SetParent(paddle.transform);
        } else {
            rightPoint.SetParent(paddle.transform.parent);
            paddle.transform.localPosition -= delta;
            leftPoint.transform.localPosition -= delta;            
            rightPoint.SetParent(paddle.transform);
        }

        paddle.size = new Vector2(size, paddle.size.y);
        paddleCollider.size = new Vector2(size, paddle.size.y);        
    }
}
