using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public enum StarType { Normal, Mana, Heal, Damage }  //�X�^�[�̃^�C�v
    public StarType type;                               //�^�C�v
    public enum StarOwner { Player, Enemy }             //�X�^�[�̏��L��
    public StarOwner owner;                             //���L��

    public bool isActive;                               //�L�����
    public bool isTypeChangeable;                       //�^�C�v�ύX�\��
    public bool isStartingPointOfSpell;                 //�X�y���̎n�_��
    public Vector2Int gridPosition;                     //�X�^�[�̊i�q���W

    //�X�^�[�̃v���n�u
    public static GameObject playerNormalStarPrefab = (GameObject)Resources.Load("Prefabs/PlayerNormalStar");     //�v���C���[�̃X�^�[
    public static GameObject enemyNormalStarPrefab = (GameObject)Resources.Load("Prefabs/EnemyNormalStar");      //�G�̃X�^�[
    public static GameObject manaStarPrefab;                   //�}�i�X�^�[
    public static GameObject healStarPrefab;                   //�q�[���X�^�[
    public static GameObject DamageStarPrefab;                 //�_���[�W�X�^�[

    void Start()
    {

    }

    public void InitializeStar(int x, int y)            //�X�^�[�̏�����
    {
        type = StarType.Normal;                         //������Ԃ̓m�[�}��
        isActive = false;                               //������Ԃ͔�A�N�e�B�u
        isTypeChangeable = true;                        //������Ԃ̓^�C�v�ύX�\
        gridPosition.x = x;                             //�z�u����i�q��x���W
        gridPosition.y = y;                             //�z�u����i�q��y���W
    }


    public void ChangeType(StarType newType)    //�^�C�v�̕ύX
    {
        type = newType;
    }

    public void ActivateStar()                  //�X�^�[�̗L����
    {
        isActive = true;
    }

    public void DeactivateStar()                //�X�^�[�̖�����
    {
        isActive = false;
    }
}
