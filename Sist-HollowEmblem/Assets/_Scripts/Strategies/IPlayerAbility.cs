using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAbility
{
   float CooldownTimer { get; }
   float Cooldown { get; }
   void Use();
}
