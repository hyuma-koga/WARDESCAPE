using UnityEngine;
using UnityEngine.EventSystems;

public class SelfDestructIfDuplicateEventSystem : MonoBehaviour
{
    [SerializeField] private HintUIController hintUIController;

    private void Awake()
    {
        // HintUIController �����蓖�Ă��Ă���΃q���g��\��
        if (hintUIController != null)
        {
            hintUIController.ShowHints();
        }

        // EventSystem �����łɑ��݂��A�������ł͂Ȃ��ꍇ�͎���i�d���h�~�j
        if (EventSystem.current != null && EventSystem.current != GetComponent<EventSystem>())
        {
            Destroy(gameObject);
        }
    }
}
