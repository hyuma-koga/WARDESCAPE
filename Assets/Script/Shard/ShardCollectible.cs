using UnityEngine;

public class ShardCollectible : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip getSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            //���ʉ����Đ�
            if (audioSource != null && getSound != null)
            {
                audioSource.PlayOneShot(getSound);
            }

            //�V���[�h��o�^
            GameManager.Instance.CollectShard();

            //SE�Đ��̂��߁A�����x��č폜(�Đ����r�؂�Ȃ��悤��)
            //getSound��null�łȂ��ꍇgetSOund.length���g���inull�������炷���폜)
            Destroy(gameObject, getSound != null ? getSound.length : 0f);
        }
    }
}