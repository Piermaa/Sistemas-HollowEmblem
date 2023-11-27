using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
   void Move(float move, bool jump);
   void StopMovement();
    bool CanMove { get; set; }
    bool CheckGround();
}
