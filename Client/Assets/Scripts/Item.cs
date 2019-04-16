using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    None,
    Star,
    Skull,
    Dot, 
    Target_baico,
    Target_bangtuyet,
    Target_daiduong,
    Target_damlay,
    Target_samac,
    Target_khonggian,
}

public class Item: MonoBehaviour
{
    [SerializeField]
    ItemType itemType;

    [SerializeField]
    int score = 100;

    [SerializeField]
    GameObject renderObj;

    [SerializeField]
    GameObject breakEff;

    public ItemType ItemType
    {
        get
        {
            return itemType;
        }

        private set
        {
            itemType = value;
        }
    }

    public int Score
    {
        get
        {
            return score;
        }

        private set
        {
            score = value;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Head") {
            PaddleController.Instance.OnHeadTrigger(this);
        }

        if (other.gameObject.tag == "Bar") {
            PaddleController.Instance.OnBarTrigger(this);
        }        
    }
    
    public void Broken() {
        Collider2D col = GetComponent<Collider2D>();
        if (col!=null)
        {
            Destroy(col);
        }
        renderObj.SetActive(false);
        GameObjectUtils.LoadGameObject(transform, breakEff);
        StartCoroutine(IDestroy());
    }

    public void Eaten() {
        Destroy(gameObject);
    }

    IEnumerator IDestroy() {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
