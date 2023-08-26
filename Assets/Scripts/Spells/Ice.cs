using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Ice")]
public class Ice : Spell
{
    //アイス: 敵に{damage}ダメージ与えて、HPを{heal}回復する
    public int damage;
    public int heal;
    // Start is called before the first frame update
    protected override void SpellEffect(Character player, Character enemy)  //スペル効果
    {
        enemy.hp  -= damage;
        player.hp += heal;
    }
}
