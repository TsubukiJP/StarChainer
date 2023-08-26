using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Fire")]
public class Fire : Spell
{
    //�t�@�C�A: �G��{damage}�_���[�W�^����
    public int damage;
    // Start is called before the first frame update
    protected override void SpellEffect(Character player, Character enemy)  //�X�y������
    {
        enemy.hp -= damage;
    }
}

