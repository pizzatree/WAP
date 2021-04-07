using Player;

namespace Bot.States
{
    public class Aggro : IState
    {
        private PlayerMove movement;

        // pro tip: when adding new fields in Rider
        // "ctorf" auto populates constructor with said fields
        public Aggro(PlayerMove movement)
        {
            this.movement = movement;
        }
    
        public void Tick()
        {
            // basically update
        }

        public void OnEnter()
        {
            // probably find target here 
        }

        public void OnExit()
        {
            // clean up?
        }

        // find target()
        // move()
        // etc. 
    }
}