using UnityEngine;
using StarWars.Actions;
using Infra.Utils;

namespace StarWars.Brains
{
    public class SuperWomanBrain : SpaceshipBrain
    {
        public override string DefaultName
        {
            get
            {
                return "SuperWoman";
            }
        }
        public override Color PrimaryColor
        {
            get
            {
                return new Color((float)0x33 / 0xFF, (float)0xCC / 0xFF, (float)0xCC / 0xFF, 1f);
            }
        }
        public override SpaceshipBody.Type BodyType
        {
            get
            {
                return SpaceshipBody.Type.TieFighter;
            }
        }

        private Spaceship target = null;

        public override Action NextAction()
        {
            // lookout for the closest target
            Vector2 distVec = spaceship.ClosestRelativePosition(spaceship);
            float dist = 1000;

            foreach (var ship in Space.Spaceships)
            {
                if (spaceship != ship && ship.IsAlive)
                {
                    if (target == null)
                    {
                        target = ship;
                        distVec = spaceship.ClosestRelativePosition(target);
                        dist = distVec.magnitude;
                    }
                    else
                    {
                        Vector2 dist2Vec = spaceship.ClosestRelativePosition(ship);
                        float dist2 = dist2Vec.magnitude;
                        if (dist2 < dist)
                        {
                            target = ship;
                            dist = dist2;
                            distVec = spaceship.ClosestRelativePosition(target);
                        }
                    }

                }
            }

            var forwardVector = spaceship.Forward;
            var angle = distVec.GetAngle(forwardVector);

            // Use Super Powers!
            if (dist <= 10)
            {
                if (spaceship.CanRaiseShield && !spaceship.IsShieldUp)
                {
                    return ShieldUp.action;
                }

                else if (angle >= 8)
                {
                    return TurnLeft.action;
                }

                else if (angle <= -8)
                {
                    return TurnRight.action;
                }

                else if(spaceship.CanShoot && !target.IsShieldUp)
                {
                    return Shoot.action;
                }
            }

            return DoNothing.action;

        }
    }
}
