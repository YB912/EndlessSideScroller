

namespace InputManagement
{
    public class InputManager
    {
        InputSystem_Actions _inputSystemActions;

        public InputSystem_Actions inputSystemActions => _inputSystemActions;

        public InputManager()
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
    }
}
