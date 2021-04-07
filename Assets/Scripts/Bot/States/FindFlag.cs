using Player;

namespace Bot.States
{
    public class FindFlag : IState
    {
        private PlayerMove movement;

        // pro tip: when adding new fields in Rider
        // "ctorf" auto populates constructor with said fields
        public FindFlag(PlayerMove movement)
        {
            this.movement = movement;
        }
    
        public void Tick()
        {
            // basically update
        }

        public void OnEnter()
        {
            // probably find target flag here
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