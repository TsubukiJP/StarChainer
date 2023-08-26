using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterData //キャラクターのデータ
{
    public int characterID;             //キャラクターID
    public string characterName;        //名前
    public GameObject characterPrefab;  //プレハブ
    public CharacterType characterType; //タイプ
    public Vector2Int fieldSize;        //フィールドサイズ(横 x+1 マス, 縦 y+1 マス)
    public int maxHp;                   //最大HP
    public int maxMp;                   //最大MP
    public int maxCp;                   //最大CP (チェインポイント)
    public List<SpellSlot> spellSlot;   //所持スペル
}
