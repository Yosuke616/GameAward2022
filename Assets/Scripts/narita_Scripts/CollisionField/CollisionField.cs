using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StageBlock
{
    public string tag = "none";
    public Vector3 rotate = Vector3.zero;
    public int type = 0;
}

public class CollisionField : SingletonMonoBehaviour<CollisionField>
{
    



    // ���ۂ̂����蔻����s���Ă���V�[���̃J����
    private string cameraName = "SubCamera0";

    // �X�e�[�W(��)���
    [SerializeField] private List<StageBlock>[] StageInfo = new List<StageBlock>[3];
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
        // �O���b�h�̐��A�T�C�Y���擾
        gridSizeX = CreateGridScript.gridSizeX;
        gridSizeY = CreateGridScript.gridSizeY;
        gridNumX = CreateGridScript.horizon;
        gridNumY = CreateGridScript.virtical;
        Debug.Log(gridNumX * gridNumY);
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

                //float rate = Mathf. Mathf.Pow(gridSizeX, 2) + Mathf.Pow(gridSizeY, 2)
                //mass.transform.localScale = new Vector3(gridSizeX*1.414f, gridSizeY * 1.414f, 1.0f);
                mass.transform.localScale = new Vector3(gridSizeX, gridSizeY , 1.0f);

                // �����蔻�胊�X�g�ɓo�^
                CollisionGrid[(y * gridNumX) + x] = mass;

                //Debug.Log(mass.name + "   " + mass.tag + "   " + _stageGrid[x + y].tag);
            }


        }
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
                    if (layerList[objCount] == 2)
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
                            //CollisionGrid[objCount].transform.position += new Vector3(gridSizeX * 0.5f, gridSizeY * 0.5f, 0);

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

    // ���̐؂�[�Ƃ̂����蔻������p�̃I�u�W�F�N�g����
    public void CheckCollisionwasteOfPaper(Transform trans)
    {
        float GridSizeX = CreateGridScript.paperGridSizeX;
        float GridSizeY = CreateGridScript.paperGridSizeY;
        int gridNumX = CreateGridScript.horizon;
        int gridNumY = CreateGridScript.virtical;
        Material[] mats = new Material[1];

        //��ׂ��q�G�����L�[�����������o���Ă���
        GameObject Cameraobj = GameObject.Find("MainCamera");

        //���O��ς��邽�߂ɃJ�E���g�����
        int nNameCnt = 0;

        // �`��J�n�ʒu = �J�������W - grid�̉��� * ���̔���
        Vector3 StartPoint;
        StartPoint.x = Cameraobj.transform.position.x - GridSizeX * gridNumX * 0.5f + (GridSizeX * 0.5f);
        StartPoint.y = Cameraobj.transform.position.y + GridSizeY * gridNumY * 0.5f - (GridSizeY * 0.5f);

        // �}�X���Ƃɂ����蔻������p�̃I�u�W�F�N�g�𐶐�
        for (int y = 0; y < gridNumY; y++)
        {
            for (int x = 0; x < gridNumX; x++, nNameCnt++)
            {
                //�X�N���v�g�ŃI�u�W�F�N�g��ǉ�����
                //GameObject mass = StageGrid.CreateMesh(GridSizeX, GridSizeY, mats);
                GameObject mass = CreateCollisionObject(
                    "chage_mass" + nNameCnt,            // ���O
                    new Vector3(
                        StartPoint.x + (GridSizeX * x), // ���W
                        StartPoint.y - (GridSizeY * y),
                        trans.position.z),
                    Vector3.zero,
                    "none",                             // �^�O
                    Cameraobj                           // �e�I�u�W�F�N�g
                    );

                mass.transform.localScale = new Vector3(CreateGridScript.paperGridSizeX, CreateGridScript.paperGridSizeY, 1);

                mass.GetComponent<BoxCollider>().isTrigger = true;

                // rigidbody
                var rb = mass.AddComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezePosition;
                rb.freezeRotation = true;
                rb.useGravity = false;

                //---�I�u�W�F�N�g�̐ݒ�

                // �����������ǂ̂悤�Ȃӂ�܂������邩
                mass.AddComponent<collsion_test>();

                storeObject.Add(mass);
            }
        }
    }


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
        {
            for (int x = 0; x < gridNumX; x++, objCount++)
            {
                vector2s.Add(new Vector2(StartPoint.x + (GridSizeX * x), StartPoint.y - (GridSizeY * y)));
            }
        }

        return vector2s;
    }


    private void CheckCollisionFlags()
    {
        List<bool> changes = new List<bool>();

        for (int i = 0; i < storeObject.Count; i++)
        {
            if (storeObject[i] == null)
            {
                changes.Add(false);
                continue;
            }

            // �^�O�ɕύX����������t���O�𗧂Ă�
            if (storeObject[i].tag != "waste")
            {
                changes.Add(true);
                Destroy(storeObject[i]);
            }
            else
            {
                changes.Add(false);
            }

        }


        storeObject.Clear();

        UpdateStage(changes);
    }


    private GameObject CreateCollisionObject(string name, Vector3 pos, Vector3 rot, string tag, GameObject parent)
    {
        //---����
        GameObject mass = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //�e
        mass.transform.SetParent(parent.transform);
        //���W
        mass.transform.position = pos;
        // ��]
        mass.transform.Rotate(rot);
        //���O
        mass.name = name;
        //�^�O��t����
        mass.tag = tag;

        return mass;
    }
}
