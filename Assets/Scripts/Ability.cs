using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Scriptable Objects/Abilities", order = 1)]
public class Ability : ScriptableObject
{
    
    public string abilityName;
    public int abilityCost = 20;
    public List<KeyCode> keycodeCombinations;
    public int abilityDamage;
    public int abilityCost;
}
