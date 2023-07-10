using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// INHERITANCE
public class CircleHole : Hole
{
    // POLYMORPHISM
    public override void CheckCollision(Shape shape)
    {
       
        if (shape.FitsHole(this))
        {
            shape.DestroyShape();
            Counter.instance.IncrementCount();

            HoleManager holeManager = HoleManager.GetInstance();
            if (holeManager != null)
            {
                holeManager.SpawnShape();
                holeManager.ResetHoles();

                UIManager uiManager = FindObjectOfType<UIManager>();
                if (uiManager != null)
                {
                    uiManager.StartCountdown();
                    uiManager.UpdateCounterText(Counter.globalCount);
                }
            }
        }
    }
        // ABSTRACTION
        private void OnTriggerEnter(Collider other)
    {
        Shape shape = other.GetComponent<Shape>();
        if (shape != null)
        {
            CheckCollision(shape);
        }
    }
}