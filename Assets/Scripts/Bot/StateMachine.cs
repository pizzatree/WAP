/* StateMachine.cs
 * Base code: Jason Weimann
 * https://www.youtube.com/watch?v=V75hgcsCGOM
*/

using System;
using System.Collections.Generic;

public class StateMachine
{
    private IState currentState;

    private Dictionary<Type, List<Transition>> transitions_All
        = new Dictionary<Type, List<Transition>>();

    private List<Transition> transitions_Current = new List<Transition>();
    private List<Transition> transitions_Interrupt = new List<Transition>();

    private static List<Transition> NoTransitions = new List<Transition>(0);

    /* Tick
     * Check all valid conditions to enter next state
     * If one of these conditions is valid, enter that state
     * Tick the current active state
    */
    public void Tick()
    {
        var transition = GetTransition();

        if (transition != null)
            SetState(transition.To);

        currentState?.Tick();
    }

    /* Set State
     * If the oncoming state is new, call OnExit on the current state
     * then switch the active state. Attempt to load transitions
     * to leave the newly active state. Then call OnEnter on the newly active state.
    */
    public void SetState(IState state)
    {
        if (state == currentState)
            return;

        currentState?.OnExit();
        currentState = state;

        transitions_All.TryGetValue(currentState.GetType(), out transitions_Current);
        if (transitions_Current == null)
            transitions_Current = NoTransitions;

        currentState.OnEnter();
    }

    /* Add Transition
     * Attempt to find a list of transitions FROM a state
     * Add a new transition TO another state under a condition
    */
    public void AddTransition(IState from, IState to, Func<bool> condition)
    {
        // If a list of conditions doesn't exist, create one
        if (transitions_All.TryGetValue(from.GetType(), out var localTransitions) == false)
        {
            localTransitions = new List<Transition>();
            transitions_All[from.GetType()] = localTransitions;
        }

        localTransitions.Add(new Transition(to, condition));
    }

    /* Add Interrupt
     * Adds a state that can override all others based on this condition
    */
    public void AddInterrupt(IState state, Func<bool> condition)
    {
        transitions_Interrupt.Add(new Transition(state, condition));
    }

    private class Transition
    {
        public Func<bool> Condition { get; }
        public IState To { get; }

        public Transition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }

    private Transition GetTransition()
    {
        foreach (var transition in transitions_Interrupt)
            if (transition.Condition())
                return transition;

        foreach (var transition in transitions_Current)
            if (transition.Condition())
                return transition;

        return null;
    }
}