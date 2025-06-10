
using UnityEngine;

namespace InputManagement
{
    public class InputManager
    {
        InputSystem_Actions _inputSystemActions;

        public InputSystem_Actions inputSystemActions => _inputSystemActions;

        internal InputManager()
        {
            _inputSystemActions = new InputSystem_Actions();
            _inputSystemActions.Enable();
        }
    }

    public interface IInputManagementFactory
    {
        public static InputManager CreateInputManager()
        {
            return new InputManager();
        }

        public static TouchInputManager CreateTouchInputManager()
        {
            return new TouchInputManager();
        }
    }
}
