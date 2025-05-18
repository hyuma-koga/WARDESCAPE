using UnityEngine;
using UnityEngine.EventSystems;

public class SelfDestructIfDuplicateEventSystem : MonoBehaviour
{
    [SerializeField] private HintUIController hintUIController;

    private void Awake()
    {
        // HintUIController が割り当てられていればヒントを表示
        if (hintUIController != null)
        {
            hintUIController.ShowHints();
        }

        // EventSystem がすでに存在し、かつ自分ではない場合は自壊（重複防止）
        if (EventSystem.current != null && EventSystem.current != GetComponent<EventSystem>())
        {
            Destroy(gameObject);
        }
    }
}
