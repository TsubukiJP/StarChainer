using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Spell List")]
public class SpellDataBase : ScriptableObject
{
    [SerializeField]
    private List<Spell> allSpellList = new();

    public List<Spell> GetSpellList()
    {
        return allSpellList;
    }


}
