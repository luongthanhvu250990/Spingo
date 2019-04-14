using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.Events;

[DisallowMultipleComponent]
[RequireComponent(typeof(ScrollRect))]
public class ScrollSnap : MonoBehaviour {
    [SerializeField] RectTransform panel;
    [SerializeField] RectTransform[] scrollItem;
    [SerializeField] RectTransform center;
    [SerializeField] float snapSpeed = 5f;

    float[] distance;  
    bool dragging = false;
    float itemDistance;
    int nearestItemIndex;

    private void Start()
    {
        int itemLength = scrollItem.Length;
        distance = new float[itemLength];
        itemDistance = 808.5f - 175;// Mathf.Abs(scrollItem[1].transform.position.x - scrollItem[0].transform.position.x);
    }

    private void Update()
    {
        float minDistance = float.MaxValue;
        for (int i = 0; i < scrollItem.Length; i++)
        {            
            distance[i] = Mathf.Abs(center.transform.position.x - scrollItem[i].transform.position.x);
            if (distance[i] < minDistance)
            {
                minDistance = distance[i];
                nearestItemIndex = i;
            }
        }

        if (!dragging)
            LerpToBtton(nearestItemIndex * -itemDistance);
    }

    private void LerpToBtton(float position)
    {
        float newX = Mathf.Lerp(panel.anchoredPosition.x, position, Time.deltaTime * snapSpeed);
        Vector2 newPosition = new Vector2(newX, panel.anchoredPosition.y);

        panel.anchoredPosition = newPosition;
    }

    public void StartDrag() {
        dragging = true;
    }

    public void EndDrag()
    {
        dragging = false;
    }
}
