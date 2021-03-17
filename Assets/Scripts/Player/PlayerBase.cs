using Mirror;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerMove),
                         typeof(PlayerCursor),
                         typeof(Rigidbody))]
    [SelectionBase]
    public class PlayerBase : NetworkBehaviour
    {
    }
}