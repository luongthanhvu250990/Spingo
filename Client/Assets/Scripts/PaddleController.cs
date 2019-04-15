using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;


public class PaddleController : MonoBehaviour
{
    const float EFFECTED_TIME = 5;
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

    bool isPause = true;
    bool isLeft = false;

    //Modify status-----------------    
    float modifiedSizeValue = 0;
    float modifiedSpeedValue = 0;

    float sizeModifyTime = 0;
    float speedModifyTime = 0;
    float powerUpTime = 0;
    //Modify status-----------------
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

    public bool PowerUp()
    {
        if (powerUpTime != 0)
            return false;
        powerUpTime = EFFECTED_TIME;
        return true;
    }


    public bool AddSize(float addVal)
    {
        if (modifiedSizeValue !=0)
            return false;
        IsPause = true;
        float lastVal = 0;
        DOTween.To(x =>
        {
            SetSize(size + x - lastVal);
            lastVal = x;
        }, 0, addVal, 1f).OnComplete(() => {
            IsPause = false;
            sizeModifyTime = EFFECTED_TIME;
        });
        modifiedSizeValue = addVal;        
        return true;
    }

    void ResetSize()
    {
        IsPause = true;
        float lastVal = 0;
        DOTween.To(x =>
        {
            SetSize(size + x - lastVal);
            lastVal = x;
        }, 0, -modifiedSizeValue, 1f).OnComplete(() =>
        {
            IsPause = false;
        });
        modifiedSizeValue = 0;
    }

    public bool AddSpeed(float addVal) {
        if (modifiedSpeedValue!=0)
            return false;
        speed += addVal;
        modifiedSpeedValue = addVal;
        speedModifyTime = EFFECTED_TIME;
        return true;
    }
    
    private void Start()
    {
        Shift();
    }
    // Update is called once per frame
    void Update()
    {
        if (IsPause) return;
        if (Input.GetMouseButtonUp(0)||Input.GetKeyUp(KeyCode.Space)) {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                isLeft = !isLeft;
                Shift();
            }
        }       
        Rotate();

        CheckSizeModify();
        CheckPowerUp();
        CheckSpeedModify();
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

    void CheckSizeModify() {
        if (modifiedSizeValue!=0)
        {
            if (IsPause) return;
            sizeModifyTime -= Time.deltaTime;
            if (sizeModifyTime <= 0)
            {
                ResetSize();                
            }
        }
    }

    void CheckPowerUp()
    {
        if (powerUpTime != 0)
        {
            if (IsPause) return;
            powerUpTime -= Time.deltaTime;
            if (powerUpTime <= 0)
            {
                powerUpTime = 0;
            }
        }
    }

    void CheckSpeedModify() {
        if (modifiedSpeedValue != 0)
        {
            if (IsPause) return;
            speedModifyTime -= Time.deltaTime;
            if (speedModifyTime <= 0)
            {
                speed -= modifiedSpeedValue;
                modifiedSpeedValue = 0;
            }
        }
    }
 }
