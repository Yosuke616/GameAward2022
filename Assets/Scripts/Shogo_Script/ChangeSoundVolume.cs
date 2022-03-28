/*
�쐬���F2022/3/14 Shimizu Shogo
���e  �F���ʒ��߃o�[
*/

using UnityEngine;

// "AudioSource"�R���|�[�l���g���A�^�b�`����Ă��Ȃ��ꍇ�A�^�b�`
[RequireComponent(typeof(AudioSource))]
public class ChangeSoundVolume : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        // "AudioSource"�R���|�[�l���g���擾
        audioSource = gameObject.GetComponent<AudioSource>();

    }

    /// <summary>
    /// �X���C�h�o�[�l�̕ύX�C�x���g
    /// </summary>
    /// <param name="newSliderValue">�X���C�h�o�[�̒l(�����I�Ɉ����ɒl������)</param>
    public void SoundSliderOnValueChange(float newSliderValue)
    {
        // ���y�̉��ʂ��X���C�h�o�[�̒l�ɕύX
        audioSource.volume = newSliderValue;
    }
}