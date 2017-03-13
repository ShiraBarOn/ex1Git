using UnityEngine;
using StarWars.Actions;
using Infra.Utils;

namespace StarWars.Brains {
    public class UnicornBrain : SpaceshipBrain
    {
        public override string DefaultName
        {
            get
            {
                return "Unicorn";
            }
        }
        public override Color PrimaryColor
        {
            get
            {
                return new Color((float)0xFF / 0xFF, (float)0x99 / 0xFF, (float)0xCC / 0xFF, 1f);
            }
        }
        public override SpaceshipBody.Type BodyType
        {
            get
            {
                return SpaceshipBody.Type.TieFighter;
            }
        }

        private Spaceship danger = null;

        public override Action NextAction()
        {
            // lookout for the closest danger!
            Vector2 distVec = spaceship.ClosestRelativePosition(spaceship);
            float dist = 1000;

            foreach (var ship in Space.Spaceships)
            {
                if (spaceship != ship && ship.IsAlive)
                {
                    if  (danger == null)
                    {
                        danger = ship;
                        distVec = spaceship.ClosestRelativePosition(danger);
                        dist = distVec.magnitude;
                    }
                    else
                    {
                        Vector2 dist2Vec = spaceship.ClosestRelativePosition(ship);
                        float dist2 = dist2Vec.magnitude;
                        if (dist2 < dist)
                        {
                            danger = ship;
                            dist = dist2;
                            distVec = spaceship.ClosestRelativePosition(danger);
                        }
                    }

                }
            }

            // panic or runaway

            var forwardVector = spaceship.Forward;
            var angle = distVec.GetAngle(forwardVector);

            // panic
            if (dist <= 5)
            {
                if (spaceship.CanRaiseShield && !spaceship.IsShieldUp)
                {
                    return ShieldUp.action;
                }

                else if (angle >= 10)
                {
                    return TurnRight.action;
                }

                else if (angle <= -10)
                {
                    return TurnLeft.action;
                }
            }

            // feeling good
            else if (dist > 5 && dist <=10)
            {

                if (spaceship.IsShieldUp)
                {
                    return ShieldDown.action;
                }

                if (angle >= 5)
                {
                    return TurnLeft.action;
                }

                else if (angle <= -5)
                {
                    return TurnRight.action;
                }

                if (spaceship.CanShoot)
                {
                    return Shoot.action;
                }
                else
                {
                    return DoNothing.action;
                }
            }

            // runaway

            if (angle >= 10)
            {
                return TurnRight.action;
            }

            else
            {
                return DoNothing.action;
            }


        }
    }
}
