using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherComponent : MonoBehaviour
{
   
   public SpriteRenderer SpriteRenderer;
   public Color ColorOn = Color.cyan;
   public Color ColorOff = Color.red;

   private int _missileInStock;

   public int MissileInStock
   {
      get => _missileInStock;
      set {
         _missileInStock = value;
         if (value > 0) {
            SpriteRenderer.color = ColorOn;
         }
         else {
            SpriteRenderer.color = ColorOff;
         }
      }
   }
}
