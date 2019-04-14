using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class TwoStateButton : MonoBehaviour
{
    [SerializeField] GameObject onObj;
    [SerializeField] GameObject offObj;

    [SerializeField] bool isOn = false;
    [SerializeField] UnityEvent onValueChange;

    public bool IsOn
    {
        get
        {
            return isOn;
        }
        set
        {
            isOn = value;
            onObj.SetActive(isOn);
            offObj.SetActive(!isOn);
            if (onValueChange != null)
                onValueChange.Invoke();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        onObj.SetActive(isOn);
        offObj.SetActive(!isOn);
        var btn = GetComponent<Button>();
        btn.onClick.AddListener(()=> {
            IsOn = !IsOn;
        });
    }  
}
