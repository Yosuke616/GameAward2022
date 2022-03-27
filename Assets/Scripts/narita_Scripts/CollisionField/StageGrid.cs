/*
 2022/3/18 ShimizuYosuke 
 ��ʂ����ς��ɓ����蔻��̂��铧���l�p���I�u�W�F�N�g�𐶐�����(��400�A�c300)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(CreateGridScript))]
public class StageGrid : MonoBehaviour
{
    //�F�z�u����}�e���A���̐ݒ�
    public Material[] ColorSet = new Material[2];

    // �S�}�X�̂����蔻��̎��
    [SerializeField] private List<string>   collisionGrid = new List<string>();
    // �S�}�X�̃I�u�W�F�N�g
    [SerializeField] private List<GameObject> Grid = new List<GameObject>();
    // �ǂ̃J�����̑O�ɃO���b�h��\�������邩
    [SerializeField] string cameraName = "MainCamera";
    // �`��J�n�ʒu
    private Vector2 StartPoint;

    // ���̔ԍ�
    public int layer = 0;

    public GameObject MainGrid;
    //�c�̐��Ɖ��̐���ݒ肷��ׂ̕ϐ�
    private int gridNumX;
    private int gridNumY;
    // �O���b�h�̉����ƍ���
    private float GridSizeX;
    private float GridSizeY;



    // ������
    void Start()
    {
        GridSizeX = CreateGridScript.gridSizeX;
        GridSizeY = CreateGridScript.gridSizeY;
        gridNumX = CreateGridScript.horizon;
        gridNumY = CreateGridScript.virtical;

        //��ׂ��q�G�����L�[�����������o���Ă���
        GameObject Cameraobj = GameObject.Find(cameraName);

        //���O��ς��邽�߂ɃJ�E���g�����
        int nNameCnt = 0;

        // �`��J�n�ʒu = �J�������W - grid�̉��� * ���̔���
        StartPoint.x = Cameraobj.transform.position.x - GridSizeX * gridNumX * 0.5f + (GridSizeX * 0.5f);
        StartPoint.y = Cameraobj.transform.position.y + GridSizeY * gridNumY * 0.5f - (GridSizeY * 0.5f);

        // �}�X���Ƃɂ����蔻������p�̃I�u�W�F�N�g�𐶐�
        for (int i = 0; i < gridNumY; i++)
        {
            for (int u = 0; u < gridNumX; u++, nNameCnt++)
            {
                //�X�N���v�g�ŃI�u�W�F�N�g��ǉ�����
                GameObject mass = CreateMesh(GridSizeX, GridSizeY, ColorSet); 

                //�e�Ǝq�̐ݒ������(�����łͶ�ׂ�e�ɂ���)
                mass.transform.SetParent(Cameraobj.transform);

                //---�I�u�W�F�N�g�̐ݒ�
                // ���W
                mass.transform.position = new Vector3(
                    StartPoint.x + (GridSizeX * u),
                    StartPoint.y - (GridSizeY * i),
                    transform.position.z);

                //���O�̐ݒ�(�Ō��i�͉��s�ڂɂ��邩)
                mass.name = "Grid_No." + nNameCnt + ":" + i;

                //�F�̐ݒ�(�����l�͐�)
                mass.GetComponent<MeshRenderer>().material = ColorSet[1];

                //�}�E�X�̓����蔻��p�̃R���|�[�l���g��ǉ�����
                mass.AddComponent<BoxCollider>();
                var Coll = mass.GetComponent<BoxCollider>();
                Coll.enabled = true;

                // �����������ǂ̂悤�Ȃӂ�܂������邩
                mass.AddComponent<collsion_test>();

                mass.GetComponent<BoxCollider>().isTrigger = true;

                //�^�O��t����
                mass.tag = "none";

                // �I�u�W�F�N�g���X�g�ɒǉ�
                Grid.Add(mass);
                // �^�O��ǉ�
                collisionGrid.Add(mass.tag);
            }
        }

        // ��u���Ԃ�u����DelayMethod()���Ă�
        Invoke("DelayMethod", 0.001f);
    }

    // �����x�点�Ď��s������
    private void DelayMethod()
    {
        for (int i = 0; i < Grid.Count; i++)
        {
            if (Grid[i] == null) continue;

            // �Փˏ����̎�ނ��Z�b�g����
            collisionGrid[i] = Grid[i].tag;

            Destroy(Grid[i]);
        }

        Grid.Clear();

        // CollisionSystem�̃}�X�ɔ��f������
        CollisionField.Instance.AddStageInfo(layer, collisionGrid);
        // layer��0��������Z�b�g����
        if(layer == 0)
        {
            CollisionField.Instance.SetStage(layer);
        }
    }


   

    // Quad�쐬
    static public GameObject CreateMesh(float GridSizeX, float GridSizeY, Material[] mats)
    {
        var uvs1 = new List<Vector2>();         // �V������������I�u�W�F�N�g��UV���W�̃��X�g
        var vertices1 = new List<Vector3>();   // �V������������I�u�W�F�N�g�̒��_�̃��X�g
        var triangles1 = new List<int>();       // �V������������I�u�W�F�N�g�̒��_���̃��X�g
        var normals1 = new List<Vector3>();     // �V������������I�u�W�F�N�g�̖@�����̃��X�g

        // ���_���W
        vertices1.Add(new Vector3(-GridSizeX * 0.5f,  GridSizeY * 0.5f, 0.0f));  // ����
        vertices1.Add(new Vector3( GridSizeX * 0.5f,  GridSizeY * 0.5f, 0.0f));  // �E��
        vertices1.Add(new Vector3( GridSizeX * 0.5f, -GridSizeY * 0.5f, 0.0f));  // �E��
        vertices1.Add(new Vector3(-GridSizeX * 0.5f, -GridSizeY * 0.5f, 0.0f));  // ����
        // uv
        uvs1.Add(new Vector2(0, 1));
        uvs1.Add(new Vector2(1, 1));
        uvs1.Add(new Vector2(1, 0));
        uvs1.Add(new Vector2(0, 0));
        // �@��
        normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
        normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
        normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
        normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
        // ���_�C���f�b�N�X
        triangles1.Add(0);
        triangles1.Add(1);
        triangles1.Add(3);
        triangles1.Add(1);
        triangles1.Add(2);
        triangles1.Add(3);

        //�J�b�g��̃I�u�W�F�N�g�����A���낢��Ƃ����
        GameObject obj = new GameObject("colliderBlock",
            typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
        var mesh = new Mesh();
        mesh.vertices = vertices1.ToArray();    // ���_���
        mesh.triangles = triangles1.ToArray();  // ���_�̐�
        mesh.uv = uvs1.ToArray();               // uv
        mesh.normals = normals1.ToArray();      // �@��
        obj.GetComponent<MeshRenderer>().materials = mats;
        obj.GetComponent<MeshFilter>().mesh = mesh;                // ���b�V���t�B���^�[�Ƀ��b�V�����Z�b�g
        //obj.GetComponent<MeshCollider>().sharedMesh = mesh;        // ���b�V���R���C�_�[�Ƀ��b�V�����Z�b�g 

        return obj;
    }

    // getter
    public List<string> GetStageInfo()
    {
        return collisionGrid;
    }
}
