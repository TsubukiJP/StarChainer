using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spell : ScriptableObject
{
    public string spellName;                                //�X�y����
    public List<Vector2Int> spellPattern = new();           //�X�y���̃p�^�[���i���̃x�N�g���j
    [HideInInspector] public Vector2 startPoint = new();    //�X�y���̎n�_�i�i�q���W�j
    public int mpCost;                                      //�}�i�R�X�g
    [HideInInspector] public int cpCost;                    //�`�F�[���R�X�g
    public bool isRotatable;                                //��]�\��
    public int tier;                                        //�e�B�A
    [TextArea]
    public string spellDiscription;                         //�X�y���̐���

    public Spell()                          //�R���X�g���N�^
    {
        /*if (spellPattern.Count > 0)
        {
            startPoint.x = spellPattern[0].x;   //�n�_��x���W�̐ݒ�
            startPoint.y = spellPattern[0].y;   //�n�_��y���W�̐ݒ�
            cpCost = spellPattern.Count;        //���̒�����CP�Ƃ���
        }*/
    }
    
    public void CastSpell(Character player, Character enemy)   //�X�y���̌Ăяo��
    {
        SpellEffect(player, enemy);     //�X�y���̌��ʂ𔭓�
    }

    protected virtual void SpellEffect(Character player, Character enemy)  //�X�y�����ʁi�e�X�y���ŃI�[�o�[���C�h�j
    {
        Debug.LogWarning("�X�y�����ʂ��ݒ肳��Ă��܂���");
    }

}

[System.Serializable]
public class SpellSlot      //�X�y���X���b�g�i�X�y���Ƃ��̐������j
{
    public Spell spellName; //�X�y��
    public int amount;      //��
    public SpellSlot(Spell _spell, int _amount) //�R���X�g���N�^
    {
        spellName = _spell;
        amount    = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}

