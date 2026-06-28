using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType {Multiplier, Magnetic, Invulnerable}

[CreateAssetMenu(fileName = "PowerUp", menuName = "Scriptable Objects/PowerUp")]
public class PowerUp : ScriptableObject
{
    public List<PowerUpType> effects;
    public float duration = 5f;
}
