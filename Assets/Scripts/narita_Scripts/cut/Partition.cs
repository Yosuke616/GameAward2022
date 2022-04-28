// ���Ǝ��̊Ԃ̎d�؂�𐶐�����N���X

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partition : MonoBehaviour
{

    static public void CreatePartition(GameObject parent, List<Vector3> outline, DrawMesh drawMesh, float z)
    {
        GameObject obj1 = drawMesh.CreateMesh(outline);
        // ---Components
        obj1.AddComponent<DrawMesh>();
        obj1.AddComponent<DivideTriangle>();
        var collider1 = obj1.AddComponent<MeshCollider>();
        var meshFilter1 = obj1.GetComponent<MeshFilter>();
        // ���O
        obj1.name = "partition";
        // �^�O
        obj1.tag = "partition";
        // �e�I�u�W�F�N�g�̐ݒ�
        obj1.transform.SetParent(parent.transform);
        // ���̎��̉��Ɏd�؂��u��
        obj1.transform.position += new Vector3(0, 0, z + 0.025f);
        // �}�e���A�����w�肷��
        obj1.GetComponent<Renderer>().material = GameManager.Instance.GetPartitionMaterial();
    }
}
