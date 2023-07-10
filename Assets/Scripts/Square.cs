using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// INHERITANCE
public class Square : Shape
{
    // POLYMORPHISM
    public override void DestroyShape()
    {
        IsDestroyed = true;
        Destroy(gameObject);  
    }

    public override System.Type GetHoleType()
    {
        return typeof(SquareHole);
    }

    // POLYMORPHISM
    public override bool FitsHole(Hole hole)
    {
        return hole is SquareHole;
    }
}