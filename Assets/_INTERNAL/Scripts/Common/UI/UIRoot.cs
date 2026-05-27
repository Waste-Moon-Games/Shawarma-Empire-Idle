using UnityEngine;

namespace Common.UI
{
    public class UIRoot : MonoBehaviour
    {
        [field: SerializeField] public Transform ScreenLayer { get; private set; }
        [field: SerializeField] public Transform PopupLayer { get; private set; }
        [field: SerializeField] public Transform OverlayLayer { get; private set; }
    }
}