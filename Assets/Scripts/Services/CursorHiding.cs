using UnityEngine;

namespace Services
{
    public class CursorHiding : MonoBehaviour
    {
        private void Awake()
        {
            Cursor.visible = false;
        }
    }
}