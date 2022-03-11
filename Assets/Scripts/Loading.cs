// �Q�l���Fhttps://gametukurikata.com/program/nowloading
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
	//�@�񓯊�����Ŏg�p����AsyncOperation
	private AsyncOperation async;
	//�@�V�[�����[�h���ɕ\������UI���
	[SerializeField]
	private GameObject loadUI;
	//�@�ǂݍ��ݗ���\������X���C�_�[
	[SerializeField]
	private Slider slider;

	string g_SceneName;

	public void NextScene(string SceneName)
	{
		//�@���[�h���UI���A�N�e�B�u�ɂ���
		loadUI.SetActive(true);

		//�@�R���[�`�����J�n
		g_SceneName = SceneName;
		StartCoroutine("LoadData");
	}

	IEnumerator LoadData()
	{
		// �V�[���̓ǂݍ��݂�����
		async = SceneManager.LoadSceneAsync(g_SceneName);

		//�@�ǂݍ��݂��I���܂Ői���󋵂��X���C�_�[�̒l�ɔ��f������
		while (!async.isDone)
		{
			var progressVal = Mathf.Clamp01(async.progress / 0.9f);
			slider.value = progressVal;
			yield return null;
		}
	}
}
