
using UnityEngine;

namespace InputManagement
{
    public class InputManager
    {
        InputSystem_Actions _inputSystemActions;

        public InputSystem_Actions inputSystemActions => _inputSystemActions;

        public InputManager()
        {
            _inputSystemActions = new InputSystem_Actions();
        }
    }
}
