using UnityEngine;

namespace Core.Services
{
    public class CursorHiding : MonoBehaviour
    {
        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
    }
}