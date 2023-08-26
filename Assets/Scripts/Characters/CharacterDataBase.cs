using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterDataBase")]
public class CharacterDataBase : ScriptableObject   //スクリプタブルオブジェクト→インスペクタ―で管理
{ 
    public List<CharacterData> characterList;
}
