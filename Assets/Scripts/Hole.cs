using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

// INHERITANCE
public abstract class Hole : MonoBehaviour
{
    // ABSTRACTION
    public abstract void CheckCollision(Shape shape);

    public static Counter counter;
}
