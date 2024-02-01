using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClass : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void FreezeYourself()
    {
        List<string> list = new List<string> { "mage", "warrior" };
        for (int i = 0; i < list.Count; i++)
        {
            list.Add(list[i].ToLower());
        }
  
    }
    
    
    public object Object;
    public event Action<object> ObjectReleasedCallback;
    public void AssignObject(object value)
    {
        Object = value;
    }
    public void ReleaseObject()
    {
        ObjectReleasedCallback(Object);
        Object = null;
    }
}
