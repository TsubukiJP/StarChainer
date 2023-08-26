using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Fire")]
public class Fire : Spell
{
    //ファイア: 敵に{damage}ダメージ与える
    public int damage;
    // Start is called before the first frame update
    protected override void SpellEffect(Character player, Character enemy)  //スペル効果
    {
        enemy.hp -= damage;
    }
}

