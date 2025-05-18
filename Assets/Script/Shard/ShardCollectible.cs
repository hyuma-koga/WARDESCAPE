using UnityEngine;

public class ShardCollectible : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip getSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            //効果音を再生
            if (audioSource != null && getSound != null)
            {
                audioSource.PlayOneShot(getSound);
            }

            //シャードを登録
            GameManager.Instance.CollectShard();

            //SE再生のため、少し遅れて削除(再生が途切れないように)
            //getSoundがnullでない場合getSOund.lengthを使う（nullだったらすぐ削除)
            Destroy(gameObject, getSound != null ? getSound.length : 0f);
        }
    }
}