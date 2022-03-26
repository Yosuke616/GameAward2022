/*
 2022/3/19 ShimizuYosuke 
 GridStage�����������̂��ڐA��������

 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGridScript : MonoBehaviour
{
    //�O���b�h�z�u�p�̗񋓑��錾
    public enum e_Face
    {
        xy,
        zx,
        yz,
    }

    // false:�J�����Ɏʂ��Ă���͈͂������A�����Ƃ���, true, ���̉����A����
    public bool paperGrid = false;

    // �}�X�̐�
    static public int horizon = 50;
    static public int virtical = 40;
    //�O���b�h1�}�X���Ƃ̑傫��
    static public float gridSizeX;
    static public float gridSizeY;
    // ���̃O���b�h�̃T�C�Y
    static public float paperGridSizeX;
    static public float paperGridSizeY;

    // �O���b�h�\���t���O
    public bool visible = true;
    // �`��͈͂����߂�J����
    public Camera dispCamera;

    //�F�̐ݒ蔒�F����
    public Color color = Color.white;
    //�ǂ̕����Ɍ�����悤�ɂ��邩�i�����xy���ʁj
    public e_Face face = e_Face.xy;
    //�����̐^�U�̔���(�����莟��L��)
    public bool back = true;

    // �J�����Ɏʂ��Ă��鉡���A����
    [SerializeField] private float worldWidth;
    [SerializeField] private float worldHeight;

    //�X�V���o�p
    float preGridSize = 0;
    int preSizeX = 0;
    int preSizeY = 0;
    Color preColor = Color.red;
    e_Face preFace = e_Face.zx;
    bool preBack = true;

    //���b�V���p�̕ϐ�
    Mesh mesh;


    // Start is called before the first frame update
    void Awake()
    {
        if(paperGrid == false)
        {
            // �J�����Ɏʂ��Ă���͈͂��v�Z����
            Vector3 rightTop = dispCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, transform.position.z));
            Vector3 leftBottom = dispCamera.ScreenToWorldPoint(new Vector3(0, 0, transform.position.z));
            worldWidth = rightTop.x - leftBottom.x;
            worldHeight = rightTop.y - leftBottom.y;
            // �O���b�T�C�Y = �� / ��
            gridSizeX = worldWidth  / horizon;
            gridSizeY = worldHeight / virtical;
        }
        else
        {
            // �O���b�T�C�Y = �� / ��
            paperGridSizeX = CreateTriangle.paperSizeX * 2 / horizon;
            paperGridSizeY = CreateTriangle.paperSizeY * 2 / virtical;
        }


        //�O���b�h�쐬
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh = ReGrid(mesh);

        // �`�悷�邩���Ȃ���
        if (!visible)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }


    //�O���b�h�ׂ̈̊֐�
    private Mesh ReGrid(Mesh mesh)
    {
        if (back)
        {
            //�f�t�H���g�̕\��
            GetComponent<MeshRenderer>().material = new Material(Shader.Find("GUI/Text Shader"));
        }
        else
        {
            //�ύX�����������Ƃ��p�̕\��
            GetComponent<MeshRenderer>().material = new Material(Shader.Find("GUI/Text Shader"));
        }

        //��񃁃b�V���̏����폜����
        mesh.Clear();

        

        

        //���ɉ�����傫����
        float width, height;
        //���_��
        int resolutionX;    // �c���̒��_��
        int resolutionY;    // �����̒��_��

        // �}�X�ƃ}�X�̊Ԋu
        float diffX, diffY;

        // ���_���
        Vector3[] vertices;
        Vector2[] uvs;
        int[] lines;
        Color[] colors;

        //�`��̊J�n���W�����߂�
        if (paperGrid == false)
        {
            width  = gridSizeX * horizon * 0.5f;
            height = gridSizeY * virtical * 0.5f;

            //�`��̊Ԋu
            diffX = gridSizeX / 4.0f;
            diffY = gridSizeY / 4.0f;
        }
        else
        {
            width  = paperGridSizeX * horizon * 0.5f;
            height = paperGridSizeY * virtical * 0.5f;

            //�`��̊Ԋu
            diffX = paperGridSizeX / 4.0f;
            diffY = paperGridSizeY / 4.0f;
        }
        Vector2 startPosition = new Vector2(-width, -height);
        Vector2 endPosition = new Vector2(width, height);

        

        //���_�������߂�i�{�� �~ 2)
        resolutionX = (horizon + 1) * 2;
        resolutionY = (virtical + 1) * 2;

        //���_�������v�f���m��
        vertices = new Vector3[resolutionX + resolutionY];  //���W
        uvs = new Vector2[resolutionX + resolutionY];       //uv
        lines = new int[resolutionX + resolutionY];         //���_�̂Ȃ���
        colors = new Color[resolutionX + resolutionY];      //�F

        // ���_���W
        for (int i = 0; i < resolutionX; i += 2)
        {
            // �c��
            vertices[i]     = new Vector3(startPosition.x + (diffX * (float)i * 2), startPosition.y, 0);
            vertices[i + 1] = new Vector3(startPosition.x + (diffX * (float)i * 2), endPosition.y, 0);
        }

        for (int i = 0; i < vertices.Length - resolutionX; i += 2)
        {
            // ����
            vertices[resolutionX + i]     = new Vector3(startPosition.x, endPosition.y - (diffY * (float)i * 2), 0);
            vertices[resolutionX + i + 1] = new Vector3(endPosition.x,   endPosition.y - (diffY * (float)i * 2), 0);
        }


        //�F�Ƃ��̐ݒ�(������)
        for (int i = 0; i < resolutionX + resolutionY; i++)
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
        preGridSize = gridSizeX;
        preSizeX = horizon;
        preSizeY = virtical;
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
