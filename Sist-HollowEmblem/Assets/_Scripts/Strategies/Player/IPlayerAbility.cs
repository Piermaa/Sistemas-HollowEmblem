using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAbility
{
   KeyCode AbilityKey { get; }
   GameObject PlayerGameObject { get; }
   AudioSource AbilityAudioSource { get; }
   AudioClip AbilitySound { get; }
   float CooldownTimer { get; }
   float Cooldown { get; }
   void Use();
   void AbilityUpdate();
   void Init(GameObject playerGameObject, AudioSource audioSource);
   bool CanBeUsed();
}
