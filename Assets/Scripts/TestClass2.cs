using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClass2 : MonoBehaviour
{
    private TestClass myOtherClass;
    
    void Start()
    {
        myOtherClass.ObjectReleasedCallback += (releasedObject) => Debug.Log( releasedObject + "was released.");
    }
}
