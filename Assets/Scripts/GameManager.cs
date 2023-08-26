using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public FightAreaManager fightAreaManager;
    public CharacterManager characterManager;
    public StarFieldManager starFieldManager;

    private GameObject player;                  //�v���C���[
    private GameObject enemy;                   //���݂̓G

    public const int MAXSTAGE = 6;              //�X�e�[�W��
    private int currentStageNumber = 1;         //���݂̃X�e�[�W

    public enum TurnType { Player, Enemy };     //�ǂ���̃^�[�����i�^�[���̕\[�v���C���[]�E��[�G]�j
    private TurnType turn;                       //�^�[��
    private int turnNumber;                      //�^�[����

    public SpellDataBase spellDataBase;                                    //�X�y���f�[�^�x�[�X
    public CharacterDataBase characterDataBase;                            //�L�����N�^�[�f�[�^�x�[�X
    public List<int> enemyIDList = new List<int> { 1, 2, 3, 4, 5, 6 };     //�X�e�[�W���Ƃ̓G��ID���X�g


    
    //�t�F�[�Y
    public enum Phases
    {
        StartPhase,             //�X�e�[�W�J�n���̃t�F�[�Y�i�G�o���̉��o�A�t�B�[���h�E��D�̏������Ȃǁj
        PlayerChainPhase,       //�v���C���[�������q���ŃX�y�������߂�t�F�[�Y
        PlayerAttackPhase,      //�X�y���𔭓����A�G�Ƀ_���[�W��^�����肷��t�F�[�Y
        EnemyChainPhase,        //�G�������q���t�F�[�Y
        EnemyAttackPhase,       //�G���X�y���𔭓�����t�F�[�Y
        WinnersShop,            //���������ꍇ�̃V���b�v
        Result,                 //�s�k�����ꍇ�̃��U���g
    }

    private Phases phase;

    //�v���p�e�B
    public GameObject Player { get => player; set => player = value; }
    public GameObject Enemy { get => enemy; set => enemy = value; }
    public int CurrentStageNumber { get => currentStageNumber; set => currentStageNumber = value; }
    public Phases Phase { get => phase; set => phase = value; }
    public TurnType Turn { get => turn; set => turn = value; }
    public int TurnNumber { get => turnNumber; set => turnNumber = value; }

    void Awake()
    {
        phase = Phases.StartPhase;  //�X�^�[�g�t�F�[�Y
        
        Player = characterManager.InstantiateCharacter(characterDataBase.characterList[0]); //�v���C���[����
        Enemy = characterManager.InstantiateCharacter(characterDataBase.characterList[1]);  //�G����
        
        fightAreaManager.ArrangementCharacter(player);  //�v���C���[�z�u
        fightAreaManager.ArrangementCharacter(enemy);   //�G�z�u
    }
    
    void Start()
    {

        fightAreaManager.RefreshStatusUI(Player);       //�v���C���[�̃X�e�[�^�X���Z�b�g
        fightAreaManager.RefreshStatusUI(Enemy);        //�G�̃X�e�[�^�X���Z�b�g
        starFieldManager.Initialize();                  //�X�^�[�t�B�[���h�̏�����

        StartCoroutine("Battle");                       //�R���[�`���J�n
    }

    public IEnumerator Battle()
    {
        while (true)
        {
            yield return null;
            Debug.Log(phase);
            switch (phase)
            {
                case Phases.StartPhase:     //�X�^�[�g�t�F�[�Y
                    
                    yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

                    phase = Phases.PlayerChainPhase;    //�v���[���[�̃`�F�[���t�F�[�Y��
                    
                    break;

                case Phases.PlayerChainPhase:   //�v���C���[���X�^�[���q����t�F�[�Y

                    turn = TurnType.Player;                 //�v���C���[�̃^�[���ɂ���
                    turnNumber++;                           //�^�[������1���₷
                    fightAreaManager.RefreshTurnIndicator(Turn, TurnNumber);    //�^�[���C���W�P�[�^�[���X�V

                    //����ς��郁�\�b�h

                    yield return new WaitUntil(() => starFieldManager.isAcceptButtonPushed);

                    starFieldManager.isAcceptButtonPushed = false;
                    phase = Phases.PlayerAttackPhase;        //�v���C���[�̃A�^�b�N�t�F�[�Y��

                    break;

                case Phases.PlayerAttackPhase:  //�v���C���[�̍U���t�F�[�Y

                    yield return new WaitForSeconds(1.0f);

                    starFieldManager.PuchResetButton();     //�X�^�[�t�B�[���h�����Z�b�g
                    phase = Phases.EnemyChainPhase;         //�G�̃`�F�[���t�F�[�Y��

                    break;

                case Phases.EnemyChainPhase:    //�G���X�^�[���q����t�F�[�Y

                    turn = TurnType.Enemy;                  //�G�̃^�[���ɂ���
                    fightAreaManager.RefreshTurnIndicator(Turn, TurnNumber);    //�^�[���C���W�P�[�^�[���X�V

                    //����ς��郁�\�b�h

                    yield return new WaitForSeconds(1.0f);

                    phase = Phases.EnemyAttackPhase;        //�G�̃A�^�b�N�t�F�[�Y��

                    break;

                case Phases.EnemyAttackPhase:   //�G�̍U���t�F�[�Y

                    yield return new WaitForSeconds(1.0f);

                    phase = Phases.PlayerChainPhase;        //�v���C���[�̃`�F�[���t�F�[�Y�֖߂�

                    break;
                
            }

        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public Character GetEnemy()
    {
        Character currentEnemy = null;

        foreach (CharacterData enemy in characterDataBase.characterList)    //���ׂĂ̓G�ɂ��Č���
        {
            if (enemy.characterID == CurrentStageNumber)                //�������݂̃X�e�[�W�ԍ��Ɠ���ID�����G��������
            {
                //currentEnemy = enemy;                                   //���݂̃X�e�[�W�̓G�ɐݒ�
            }
        }

        return currentEnemy;    //�ݒ肵���G��Ԃ�
    }

}
