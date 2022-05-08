/*
    ���C���J�����Ɏʂ��Ă��鎆�̂Ƃ���ɂ����蔻���ݒ肷��
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StageBlock
{
    public string tag = "none";
    public Vector3 rotate = Vector3.zero;
    public int type = 0;
    // ���̂����蔻��u���b�N�̂��ƂƂȂ�I�u�W�F�N�g
    public GameObject sourceObject;
}

public class CollisionField : SingletonMonoBehaviour<CollisionField>
{
    // 0���琔����̂�
    // �R���̏ꍇ��2�ɐݒ�
    // �S���̏ꍇ��3�ɐݒ�
    public int MaxLayerNum = 2;


    // ���ۂ̂����蔻����s���Ă���V�[���̃J����
    private string cameraName = "SubCamera0";

    // �X�e�[�W(��)���
    [SerializeField] private List<StageBlock>[] StageInfo;
    // ���C���J�����Ō����Ă��镔���̂����蔻��̃��X�g
    [SerializeField] private List<GameObject> CollisionGrid = new List<GameObject>();
    [SerializeField] private List<int> layerList = new List<int>();
    List<int> LayerList{ get { return this.layerList; } }
    

    // �O���b�h�̕��A�����A��
    public GameObject MainGrid;
    private float gridSizeX;
    private float gridSizeY;
    private int gridNumX, gridNumY;

    // �`��J�n�ʒu
    private Vector2 StartPoint;

    [SerializeField] private List<GameObject> storeObject = new List<GameObject>();

    void Start()
    {
        StageInfo = new List<StageBlock>[MaxLayerNum + 1];

        // �O���b�h�̐��A�T�C�Y���擾
        gridSizeX = CreateGridScript.gridSizeX;
        gridSizeY = CreateGridScript.gridSizeY;
        gridNumX = CreateGridScript.horizon;
        gridNumY = CreateGridScript.virtical;
        // �����蔻�胊�X�g�̃T�C�Y���}�X�̐��Ɠ����������₷
        for (int i = 0; i < gridNumX * gridNumY; i++)
        {
            CollisionGrid.Add(null);
            LayerList.Add(0);
        }

    }


    // StageGrid.cs�̃X�e�[�W�i���j����ǉ�
    public void AddStageInfo(int layer, List<StageBlock> stage)
    {
        StageInfo[layer] = stage;
    }


    // StageGrid.cs�̏������ɂ����蔻����\�z����
    public void SetStage(int layer)
    {
        // �����̔ԍ��̃X�e�[�W(��)���
        List<StageBlock> _stageGrid = StageInfo[layer];

        //���O��ς��邽�߂ɃJ�E���g�����
        int nNameCnt = 0;

        int debugCnt = 0;

        //��ׂ��q�G�����L�[�����������o���Ă���
        GameObject Cameraobj = GameObject.Find(cameraName);
        // �`��J�n�ʒu
        StartPoint.x = Cameraobj.transform.position.x - gridSizeX * gridNumX / 2.0f + (gridSizeX * 0.5f);
        StartPoint.y = Cameraobj.transform.position.y + gridSizeY * gridNumY / 2.0f - (gridSizeY * 0.5f);

        // �}�X���Ƃɂ����蔻������p�̃I�u�W�F�N�g�𐶐�
        for (int y = 0; y < gridNumY; y++)
        {
            for (int x = 0; x < gridNumX; x++, nNameCnt++)
            {
                // �^�O��"none"��������null�����Ă���
                if (_stageGrid[(y * gridNumX) + x].tag == "none")
                {
                    CollisionGrid[(y * gridNumX) + x] = null;
                    continue;
                }


                //�X�N���v�g�ŃI�u�W�F�N�g��ǉ�����
                GameObject mass = CreateCollisionObject(
                    "Grid_No." + nNameCnt + ":" + x,    // ���O
                    new Vector3(
                        StartPoint.x + (gridSizeX * x), // ���W
                        StartPoint.y - (gridSizeY * y),
                        transform.position.z),
                    Vector3.zero,
                    _stageGrid[(y * gridNumX) + x].tag, // �^�O
                    Cameraobj                           // �e�I�u�W�F�N�g
                    );

                mass.transform.localScale = new Vector3(gridSizeX, gridSizeY , 1.0f);

                // �����蔻�胊�X�g�ɓo�^
                CollisionGrid[(y * gridNumX) + x] = mass;

                if (CollisionGrid[(y * gridNumX) + x].tag == "enemy")
                {
                    Enemy.AddEnemyFunction(CollisionGrid[(y * gridNumX) + x],
                       _stageGrid[(y * gridNumX) + x].sourceObject.GetComponent<Enemy>());

                    CollisionGrid[(y * gridNumX) + x].GetComponent<BoxCollider>().isTrigger = true;
                }

                debugCnt++;
            }


        }

        // �ŏ���1���ڂȂ̂�layer��0�ɂ��Ă���
        for (int i = 0; i < LayerList.Count; i++)
            LayerList[i] = 0;
    }


    // �j������ɍ��킹�Ă����蔻�胊�X�g���X�V����
    public void UpdateStage(List<bool> changes)
    {
        if(CollisionGrid.Count != changes.Count) { Debug.LogWarning("�T�C�Y���Ⴂ�܂�" + "   CollisionGrid" + CollisionGrid.Count + "changes" + changes.Count); return; }

        //���O��ς��邽�߂ɃJ�E���g�����
        int objCount = 0;

        for (int y = 0; y < gridNumY; y++)
        {
            for (int x = 0; x < gridNumX; x++, objCount++)
            {
                // �ύX�t���O�̊m�F
                if(changes[objCount])
                {
                    // ���C���[���Ōゾ�����ꍇ�A�����蔻��I�u�W�F�N�g������
                    if (layerList[objCount] == MaxLayerNum)
                    {
                        if(CollisionGrid[objCount])
                        {
                            Destroy(CollisionGrid[objCount]);
                            CollisionGrid[objCount] = null;
                        }
                        continue;
                    }

                    // ����
                    if(CollisionGrid[objCount])
                    {

                        if (StageInfo[layerList[objCount] + 1][objCount].tag != "none")
                        {
                            // ��������
                            // �^�O�̕ύX
                            CollisionGrid[objCount].tag = StageInfo[layerList[objCount] + 1][objCount].tag;
                            if (CollisionGrid[objCount].tag == "enemy")
                            {
                                Enemy.AddEnemyFunction(CollisionGrid[objCount],
                                    StageInfo[layerList[objCount] + 1][objCount].sourceObject.GetComponent<Enemy>());
                                //CollisionGrid[objCount].AddComponent<Enemy>();
                                CollisionGrid[objCount].GetComponent<BoxCollider>().isTrigger = true;
                            }

                            // ���̃��C���[�ɍX�V
                            layerList[objCount]++;
                        }
                        else
                        {
                            // ���͂Ȃ�

                            // �I�u�W�F�N�g����
                            if (CollisionGrid[objCount])
                            {
                                Destroy(CollisionGrid[objCount]);
                                CollisionGrid[objCount] = null;
                            }

                            // ���̃��C���[�ɍX�V
                            layerList[objCount]++;
                        }
                    }
                    // �Ȃ�
                    else
                    {
                        if (StageInfo[layerList[objCount] + 1][objCount].tag != "none")
                        {
                            // ���͂���
                            CollisionGrid[objCount] = CreateCollisionObject(
                                "chage_mass" + objCount,    // ���O
                                new Vector3(
                                    StartPoint.x + (gridSizeX * x), // ���W
                                    StartPoint.y - (gridSizeY * y),
                                    transform.position.z),
                                StageInfo[layerList[objCount] + 1][objCount].rotate,
                                StageInfo[layerList[objCount] + 1][objCount].tag,   // �^�O
                                gameObject                                          // �e�I�u�W�F�N�g
                                );

                            float rate = gridSizeX * Mathf.Tan(gridSizeY / gridSizeX);
                            CollisionGrid[objCount].transform.localScale = new Vector3(gridSizeX, gridSizeY, 1);

                            if (CollisionGrid[objCount].tag == "enemy")
                            {
                                Enemy.AddEnemyFunction(CollisionGrid[objCount],
                                    StageInfo[layerList[objCount] + 1][objCount].sourceObject.GetComponent<Enemy>());
                                //CollisionGrid[objCount].AddComponent<Enemy>();
                                CollisionGrid[objCount].GetComponent<BoxCollider>().isTrigger = true;
                            }

                            // ���̃��C���[�ɍX�V
                            layerList[objCount]++;
                        }
                        else
                        {
                            // ���͂Ȃ�
                            // �I�u�W�F�N�g����
                            if (CollisionGrid[objCount])
                            {
                                Destroy(CollisionGrid[objCount]);
                                CollisionGrid[objCount] = null;
                            }

                            // ���̃��C���[�ɍX�V
                            layerList[objCount]++;
                        }
                    }
                }
            }
        }
     }

    

    // ���̃O���b�h�̂��ׂẴ}�X�̒��S���W�����X�g�ɂ��ĕԂ�
    // ���Ԃ͍��ォ��E��
    public List<Vector2> cellPoints()
    {
        List<Vector2> vector2s = new List<Vector2>();

        // ���̃O���b�h�̑傫��
        float GridSizeX = CreateGridScript.paperGridSizeX;
        float GridSizeY = CreateGridScript.paperGridSizeY;
        // �c�Ɖ��̐�
        int gridNumX = CreateGridScript.horizon;
        int gridNumY = CreateGridScript.virtical;

        //��ׂ��q�G�����L�[�����������o���Ă���
        GameObject Cameraobj = GameObject.Find("MainCamera");

        // �`��J�n�ʒu = �J�������W - grid�̉��� * ���̔���
        Vector3 StartPoint;
        StartPoint.x = Cameraobj.transform.position.x - GridSizeX * gridNumX * 0.5f + (GridSizeX * 0.5f);
        StartPoint.y = Cameraobj.transform.position.y + GridSizeY * gridNumY * 0.5f - (GridSizeY * 0.5f);
        int objCount = 0;
        // �}�X���Ƃɂ����蔻������p�̃I�u�W�F�N�g�𐶐�
        for (int y = 0; y < gridNumY; y++)
            for (int x = 0; x < gridNumX; x++, objCount++)
                vector2s.Add(new Vector2(StartPoint.x + (GridSizeX * x), StartPoint.y - (GridSizeY * y)));

        return vector2s;
    }






    private GameObject CreateCollisionObject(string name, Vector3 pos, Vector3 rot, string tag, GameObject parent)
    {
        //---����
        GameObject mass = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //�e
        mass.transform.SetParent(parent.transform);
        //���W
        mass.transform.position = pos;
        //��]
        mass.transform.Rotate(rot);
        //���O
        mass.name = name;
        //�^�O��t����
        mass.tag = tag;

        //if(tag == "enemy")
        //{
        //    // �����̃J�����̃G�l�~�[�Ȃ̂� �� layer��0�Ȃ�SubCamera1, 1�Ȃ�SubCamera2
        //
        //
        //    mass.AddComponent<Enemy>();
        //}

        return mass;
    }
}
