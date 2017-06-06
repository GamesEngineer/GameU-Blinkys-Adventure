// Copyright 2017 Game-U, Inc.
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BlinkysAdventure
{
    class StateMachine
    {
        public State currentState;

        public bool HandleEvent(string eventName_)
        {
            foreach (var t in currentState.transitions)
            {
                if (t.eventName == eventName_)
                {
                    Debug.WriteLine(
                        "On event '{0}', transitioning from '{1}' to '{2}'",
                        eventName_,
                        currentState.name,
                        t.toState.name);
                    currentState = t.toState;
                    return true;
                }
            }
            Debug.WriteLine(
                "Ignoring unknown event '{0}' in state '{1}'.",
                eventName_,
                currentState.name);
            return false;
        }
    }

    public class State
    {
        public string name = "<n/a>";
        public string description = "<n/a>";
        public List<StateTransition> transitions = new List<StateTransition>();
        public State(string name_)
        {
            name = name_;
        }
        public void AddOption(string name, State goToState)
        {
            var optionStateTransition = new StateTransition(name, goToState);
            transitions.Add(optionStateTransition);
        }
    }

    public class StateTransition
    {
        public StateTransition(string eventName_, State toState_)
        {
            eventName = eventName_;
            toState = toState_;
        }
        public string eventName;
        public State toState;
    }
}
