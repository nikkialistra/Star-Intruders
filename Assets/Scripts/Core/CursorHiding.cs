using UnityEngine;

namespace Core
{
    public class CursorHiding : MonoBehaviour
    {
        private void Awake()
        {
            Cursor.visible = false;
        }
    }
}