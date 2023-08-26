using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType { Player, Enemy }; //�L�����N�^�[�^�C�v


public class Character : MonoBehaviour
{

    //�摜�E�X�e�[�^�X
    public int characterID;             //�L�����N�^�[ID
    public string characterName;        //���O
    public CharacterType characterType; //�^�C�v
    public Vector2Int fieldSize;        //�t�B�[���h�T�C�Y(�� x+1 �}�X, �c y+1 �}�X)
    public int maxHp;                   //�ő�HP
    public int maxMp;                   //�ő�MP
    public int maxCp;                   //�ő�CP (�`�F�C���|�C���g)
    public int hp;                      //���݂�HP
    public int mp;                      //���݂�MP
    public int cp;                      //���݂�CP
    public List<SpellSlot> spellSlot;   //�����X�y��


    public void AddSpell(Spell _spell, int _amount)     //�X�y�����X�y���X���b�g�ɒǉ����郁�\�b�h
    {
        bool hasSpell = false;  //�ǉ�����X�y���͏��L�ς݂�

        for(int i = 0; i < spellSlot.Count; i++)
        {
            if(spellSlot[i].spellName == _spell)        //���������Ă���Ȃ�
            {
                spellSlot[i].AddAmount(_amount);    //���𑝂₷
                hasSpell = true;
                break;
            }
        }
        if (!hasSpell)  //���������Ă��Ȃ��Ȃ�
        {
            spellSlot.Add(new SpellSlot(_spell, _amount));  //�V���ɃX�y����ǉ�
        }
    }
}


