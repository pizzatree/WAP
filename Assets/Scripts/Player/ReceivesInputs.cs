using Inputs;
using Mirror;

namespace Player
{
    public class ReceivesInputs : NetworkBehaviour
    {
        protected IInputs inputs;
        
        private void AssignInputs(IInputs inputs)
            => this.inputs = inputs;
    }
}
