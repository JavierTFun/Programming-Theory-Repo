using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ABSTRACTION
//ENCAPSULATION
public abstract class Shape : MonoBehaviour
{
    private bool isDestroyed;

    public bool IsDestroyed
    {
        get { return isDestroyed; }
        protected set { isDestroyed = value; }
    }

    public abstract void DestroyShape();
    public abstract bool FitsHole(Hole hole);
    public abstract System.Type GetHoleType();

}
