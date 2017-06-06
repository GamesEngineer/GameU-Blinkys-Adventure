// Copyright 2017 Game-U, Inc.
using System;

namespace BlinkysAdventure
{
    class Program
    {
        static public StateMachine stateMachine = new StateMachine();

        static void Main(string[] args)
        {
            BuildStates();

            Console.WriteLine(@"
WELCOME TO BLINKY'S AVDENTURE - The tale of an alien robot trying to go home.
Copyright 2017 Game-U, Inc.");

            while (stateMachine.currentState != null)
            {
                Console.WriteLine(stateMachine.currentState.description);
                if (stateMachine.currentState.transitions.Count > 0)
                {
                    Console.Write("> ");
                    string response = Console.ReadLine();
                    ProcessResponse(response);
                }
                else
                {
                    stateMachine.currentState = null;
                }
            }

            Console.WriteLine();
            Console.WriteLine("<press any key to end your adventure>");
            Console.ReadKey(true); //waits for a key press
        }

        static public void ProcessResponse(string response)
        {
            response = response.ToLower();
            if (response == "quit")
            {
                Environment.Exit(0);
            }
            else if (response == "help")
            {
                Console.WriteLine(@"
] Respond by entering simple commands. Commands always start with
] a verb. Typcial commands include:
]   go {direction}
]   examine {item|thing}
]   take {item}
]   place {item} on {thing}
]   open {thing}
]   close {thing}
] Type 'hint' to see you current options.
] Type 'quit' to end your adventure.");
            }
            else if (response == "hint")
            {
                Console.WriteLine("Your current options are:");
                foreach (var transition in stateMachine.currentState.transitions)
                {
                    Console.WriteLine("to '{0}'", transition.eventName);
                }
            }
            else
            {
                bool isValidResponse = stateMachine.HandleEvent(response);
                if (!isValidResponse)
                {
                    Console.WriteLine("'{0}' is not a valid option here.", response);
                }
            }
        }

        static public void BuildStates()
        {
            var landingSite_start = new State("Landing site: start");
            var landingSite_return = new State("Landing site: return");
            var landingSite_withLimestone = new State("Landing site: with limestone");
            var landingSite_primerPanelOpen = new State("Landing site: primer panel open");
            var landingSite_repairedButPanelOpen = new State("Landing site: repaired, panel open");
            var landingSite_repairedAndReadyToGo = new State("Landing site: repaired, ready to go");

            var clearing_meetBear = new State("Clearing: meet bear");
            var clearing_foundLimestone = new State("Clearing: found limestone");
            var clearing_withLimestone = new State("Clearing: with limestone");

            var bush_hideFromBear = new State("Bush: hiding from bear");

            var cliff_first = new State("Cliff: first");
            var cliff_examineRock = new State("Cliff: examining rock");
            var cliff_withLimestone = new State("Cliff: with limestone");

            var ignoreBear = new State("Ignore bear");
            var followBear = new State("Follow bear");

            landingSite_start.description = @"
Wow! That was a hard landing. Your spacecraft's catalytic primer is exhausted
and needs to be refreshed. Unfortunately, you don't have any spare calcium
carbonate with which to refresh the primer. Hopefully, there will be a suitable
substance nearby that can be used to refresh the damaged catalytic primer.
You appear to be standing in a pine forrest at sunrise. Your hear something.
There is a growling noise coming from the SOUTH.";
            landingSite_start.AddOption("go south", clearing_meetBear);

            landingSite_return.description = @"
You return to your damaged spaceship in the pine forrest.
This area is now earily quite. There is a clearing to the SOUTH.";
            landingSite_return.AddOption("go south", clearing_foundLimestone);

            landingSite_withLimestone.description = @"
You return to your damaged spaceship in the pine forrest. The ship's
catalytic primer is concealed behind a small PANEL. The clearing is
to the SOUTH.";
            landingSite_withLimestone.AddOption("go south", clearing_foundLimestone);
            landingSite_withLimestone.AddOption("open panel", landingSite_primerPanelOpen);

            landingSite_primerPanelOpen.description = @"
You open the PANEL that covers the catalytic primer. The damaged PRIMER
smells like burnt matches, but it could be refreshed with correct substance.
Maybe the LIMESTONE would be useful here.";
            landingSite_primerPanelOpen.AddOption("close panel", landingSite_withLimestone);
            landingSite_primerPanelOpen.AddOption("place limestone on primer", landingSite_repairedButPanelOpen);

            landingSite_repairedButPanelOpen.description = @"
You crush the limestone rock with your powerful grippers, and mix the
powdered rock with the catalytic primer. Within seconds, the foul odor
dissipates and the primer is refreshed. The PANEL is still open, which
will prevent the spaceship from launching.";
            landingSite_repairedButPanelOpen.AddOption("close panel", landingSite_repairedAndReadyToGo);

            landingSite_repairedAndReadyToGo.description = @"
You close the PANEL and eagerly enter your spaceship. You set the ship's
navigation coordinates to home, and fire up the propulsion drive.
WHOOSH! The ship launches with incredible speed. You're on your way home!
With a great sense of relief, you relax and turn on your favorite music
playlist for the jouney back to planet Gameu.
THE END!";

            clearing_meetBear.description = @"
You walk into a clearing and encounter a grizzly bear. Fortunately, it
hasn't noticed you. The bear appears to be hunting something else.
There is a densely foliated BUSH nearby, and a rushing river to the EAST.
Your damaged spaceship is NORTH in the pine forrest.";
            clearing_meetBear.AddOption("go north", landingSite_return);
            clearing_meetBear.AddOption("go east", ignoreBear);
            clearing_meetBear.AddOption("go bush", bush_hideFromBear);

            clearing_foundLimestone.description = @"
Everything is peaceful in this clearing. You see and a rushing river to the EAST.
Looking SOUTH, you spy a cliff of white rock alongside a stream of water.
You spot bear tracks heading EAST toward a river.
The pine forrest is NORTH of this clearing.";
            clearing_foundLimestone.AddOption("go north", landingSite_return);
            clearing_foundLimestone.AddOption("go south", cliff_first);
            clearing_foundLimestone.AddOption("go east", followBear);

            clearing_withLimestone.description = @"
In this clearing, there is a densely foliated BUSH nearby, and a river to the EAST.
Looking SOUTH, you spy a cliff of white rock alongside a stream of water.
You see bear tracks heading EAST toward a river.
The pine forrest is NORTH of this clearing.";
            clearing_withLimestone.AddOption("go north", landingSite_withLimestone);
            clearing_withLimestone.AddOption("go south", cliff_examineRock);
            clearing_withLimestone.AddOption("go east", followBear);

            bush_hideFromBear.description = @"
You duck into a nearby dense bush, hoping to hide from the bear. Fortunately,
since you're a robot and have no odor, the bear can't smell your presence.
After a minute, the fearsome beast lumbers off towards a river to the EAST.
Looking SOUTH, you spy a cliff of white rock alongside a stream of water.
The pine forrest is NORTH of this clearing.";
            bush_hideFromBear.AddOption("go north", landingSite_return);
            bush_hideFromBear.AddOption("go south", cliff_first);
            bush_hideFromBear.AddOption("go east", followBear);

            cliff_first.description = @"
You are standing next to a cliff of white ROCK. It looks very interesting.
The clearing is to the NORTH.";
            cliff_first.AddOption("go north", clearing_foundLimestone);
            cliff_first.AddOption("examine rock", cliff_examineRock);

            cliff_examineRock.description = @"
This white cliffside appears to be made of LIMESTONE. They chalky substance
contains numerous fossils. This is a fortuitous discovery, because you know
that LIMESTONE can be used to repair your spaceship's catalytic primer.
The clearing is to the NORTH.";
            cliff_examineRock.AddOption("go north", clearing_foundLimestone);
            cliff_examineRock.AddOption("take limestone", cliff_withLimestone);

            cliff_withLimestone.description = @"
You find a broken piece of limestone and remove it from the cliffside. It is
heavy, yet lighter than you expected. This rock will be useful for repairing
your spaceship. The clearing is to the NORTH.";
            cliff_withLimestone.AddOption("go north", clearing_withLimestone);

            ignoreBear.description = @"
You ignore the bear and leave the clearing. At first, the bear doesn't notice
you. But as you move, your shiny chrome exterior reflects the sunlight into
bear's eyes, accidentally alerting the bear to your presence. Instantly, the
ferrocious bear stands tall on its hind legs and roars at you. Swiftly, the
bear clobbers you with a powerful swipe of its clawed paw. You burst into
pieces. The catastrophic damage causes all systems to power off.
THE END!";

            followBear.description = @"
You give into your curiosity and decide to quitely follow the bear eastward,
toward the river. The bear doesn't notice you, until... CRACK! You stepped on
a dead tree branch, accidentally alerting the bear to your presence. The bear
turns toward the sound and spies your shiny chrome exterior. Instantly, the
ferrocious bear stands tall on its hind legs and roars at you. Swiftly, the
bear clobbers you with a powerful swipe of its clawed paw. You burst into
pieces. The catastrophic damage causes all systems to power off.
THE END!";

            stateMachine.currentState = landingSite_start;
        }
    }
}
