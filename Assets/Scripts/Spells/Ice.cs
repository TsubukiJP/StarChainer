using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Ice")]
public class Ice : Spell
{
    //�A�C�X: �G��{damage}�_���[�W�^���āAHP��{heal}�񕜂���
    public int damage;
    public int heal;
    // Start is called before the first frame update
    protected override void SpellEffect(Character player, Character enemy)  //�X�y������
    {
        enemy.hp  -= damage;
        player.hp += heal;
    }
}
