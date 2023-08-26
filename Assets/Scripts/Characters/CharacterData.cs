using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterData //�L�����N�^�[�̃f�[�^
{
    public int characterID;             //�L�����N�^�[ID
    public string characterName;        //���O
    public GameObject characterPrefab;  //�v���n�u
    public CharacterType characterType; //�^�C�v
    public Vector2Int fieldSize;        //�t�B�[���h�T�C�Y(�� x+1 �}�X, �c y+1 �}�X)
    public int maxHp;                   //�ő�HP
    public int maxMp;                   //�ő�MP
    public int maxCp;                   //�ő�CP (�`�F�C���|�C���g)
    public List<SpellSlot> spellSlot;   //�����X�y��
}
