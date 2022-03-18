/*
 2022/3/19 ShimizuYosuke 
 GlidStage�����������̂��ڐA��������

 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGlidScript : MonoBehaviour
{
    //==============================
    //�O���b�h�z�u�p�̗񋓑��錾
    public enum e_Face
    {
        xy,
        zx,
        yz,
    }

    //�O���b�h1�}�X���Ƃ̑傫��
    public float gridSize = 1f;
    //������傫�����ȁH�悭�킩���
    public int size = 8;
    //�F�̐ݒ蔒�F����
    public Color color = Color.white;
    //�ǂ̕����Ɍ�����悤�ɂ��邩�i�����xy���ʁj
    public e_Face face = e_Face.xy;
    //�����̐^�U�̔���(�����莟��L��)
    public bool back = true;

    //�X�V���o�p
    float preGridSize = 0;
    int preSize = 0;
    Color preColor = Color.red;
    e_Face preFace = e_Face.zx;
    bool preBack = true;

    //���b�V���p�̕ϐ�
    Mesh mesh;

    //==============================

    // Start is called before the first frame update
    void Start()
    {
        //==========================
        //�O���b�h�p�̏�����
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        //���b�V���̕ύX�����邽�߂̊֐�
        mesh = ReGrid(mesh);
        //==========================
    }

    // Update is called once per frame
    void Update()
    {
        //�O���b�h�̂��߂̂��
        //�֌W�l�̍X�V�����o�����烁�b�V�����X�V
        if (gridSize != preGridSize || size != preSize || preColor != color || preFace != face || preBack != back)
        {
            if (gridSize < 0) { gridSize = 0.000001f; }
            if (size < 0) { size = 1; }
            ReGrid(mesh);
        }
    }


    //�O���b�h�ׂ̈̊֐�
    private Mesh ReGrid(Mesh mesh)
    {
        if (back)
        {
            //�f�t�H���g�̕\��
            GetComponent<MeshRenderer>().material = new Material(Shader.Find("Sprites/Default"));
        }
        else
        {
            //�ύX�����������Ƃ��p�̕\��
            GetComponent<MeshRenderer>().material = new Material(Shader.Find("GUI/Text Shader"));
        }

        //��񃁃b�V���̏����폜����
        mesh.Clear();

        //���b�V���쐬����ɓ������ĕK�v�ɂȂ��Ă���ϐ�����
        //�`�掞�̑傫��
        int drawSize;
        //���ɉ�����傫����
        float width;
        //�𑜓x�̐ݒ�
        int resolution;
        //��Ȃ̂Ō㏑��
        float diff;
        //���_���
        Vector3[] vertices;
        //�����e�N�X�`���I�ȓz
        Vector2[] uvs;
        //���̂��
        int[] lines;
        //�F
        Color[] colors;

        //�傫��������
        drawSize = size * 2;
        //���̐��̌���
        width = gridSize * drawSize / 4.0f;
        //�J�n���W�����߂�
        Vector2 startPosition = new Vector2(-width, -width);
        //�I���ꏊ�����߂�
        Vector2 endPosition = new Vector2(width, width);
        //���ς�炸�悭�킩���
        diff = width / drawSize;
        //�ǂ̂��炢���ꂢ�ɂ��邩
        resolution = (drawSize + 2) * 2;
        //�Ŋ��̂Q�ӂ�ǉ����Ă���

        //���_�̏����Z�b�g����
        vertices = new Vector3[resolution];
        //�e�N�X�`�����̃Z�b�g
        uvs = new Vector2[resolution];
        //�����h������ǂ��܂ň�����
        lines = new int[resolution];
        //���̐F���Z�b�g
        colors = new Color[resolution];

        //�ҏW��������ݒ肵�Ă���(���_���Ƃ�)
        for (int i = 0; i < vertices.Length; i += 4)
        {
            vertices[i] = new Vector3(startPosition.x + (diff * (float)i), startPosition.y, 0);
            vertices[i + 1] = new Vector3(startPosition.x + (diff * (float)i), endPosition.y, 0);
            vertices[i + 2] = new Vector3(startPosition.x, endPosition.y - (diff * (float)i), 0);
            vertices[i + 3] = new Vector3(endPosition.x, endPosition.y - (diff * (float)i), 0);
        }

        //�F�Ƃ��̐ݒ�(������)
        for (int i = 0; i < resolution; i++)
        {
            uvs[i] = Vector2.zero;
            lines[i] = i;
            colors[i] = color;
        }

        //��]����ݒ肷��
        Vector3 rotDirection;
        //�ǂ��������Ă��邩�ɂ���ĕς����
        switch (face)
        {
            case e_Face.xy:
                rotDirection = Vector3.forward;
                break;
            case e_Face.zx:
                rotDirection = Vector3.up;
                break;
            case e_Face.yz:
                rotDirection = Vector3.right;
                break;
            default:
                rotDirection = Vector3.forward;
                break;
        }

        //��]��K�������邽�߂̊֐�
        mesh.vertices = RotationVertices(vertices, rotDirection);
        //�e�N�X�`������K��������
        mesh.uv = uvs;
        //�F�𔽉f������
        mesh.colors = colors;
        //�������Z�b�g���Ă���
        mesh.SetIndices(lines, MeshTopology.Lines, 0);

        ///�悭�킩��Ȃ��ύX�R
        preGridSize = gridSize;
        preSize = size;
        preColor = color;
        preFace = face;
        preBack = back;

        return mesh;
    }


    //�O���b�h����]�����邽�߂̊֐�
    private Vector3[] RotationVertices(Vector3[] vertices, Vector3 rotDirection)
    {
        Vector3[] ret = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            ret[i] = Quaternion.LookRotation(rotDirection) * vertices[i];
        }
        return ret;
    }
}
