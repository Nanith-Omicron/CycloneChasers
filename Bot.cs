using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycloneChasers
{
    internal class Bot
    {
        const float MINIMUM_HP = 0.5f;
        public static UInt16 TotalNumberOfBots = 0;
        public Vector2 pos;
        public Vector2 spd;
        public float vel = 1;
        public float rotation;
        public float rotvel = 0f;


        public float fricVel = 0.4f;
        public float fricRot = 0.0833333333f;

        String botName = "defaultBot";


        public String getName { get { return botName; } }
        public Bot(String bN)
        {
            botName = bN;
            ID = TotalNumberOfBots++;
            Arbitor.WriteLog(botName + " joined the fight!");
        }
        public void AddComponent(Component n)
        {
            Arbitor.WriteLog("Adding [" + n.getTypeName + "] " + n.name + " to BOT [" + ID + "]");
            if (n.getType == Component.componentType.battery)
            {
                _battery.Add(n);
            }
            _components.Add(n);
            n.setOwner(this);
        }
        public static Vector2 RotateVectorAround(Vector2 vector, Vector2 pivot, double angle)
        {
            //Get the X and Y difference
            float xDiff = vector.X - pivot.X;
            float yDiff = vector.Y - pivot.Y;

            //Rotate the vector
            float x = (float)((Math.Cos(angle) * xDiff) - (Math.Sin(angle) * yDiff) + pivot.X);
            float y = (float)((Math.Sin(angle) * xDiff) + (Math.Cos(angle) * yDiff) + pivot.Y);
            return new Vector2(x, y);

        }
        UInt16 ID = 0;
        /// <summary>
        /// Return this bot ID
        /// </summary>
        public UInt16 GetID
        {
            get { return ID; }
        }


        List<Component> _components = new List<Component>();
        List<Component> _battery = new List<Component>();

        public void Initialized()
        {
            foreach (Component n in _components)
            {
                n.Initialized();
            }
        }

        public void AddForce(Vector2 f)
        {
            spd += f;
        }
        public void checkForCollision(Bot other)
        {
            // This is the routine for collision.
            // Currently, this check for every component on this bots
            // and check if there's a contact. For now component aren;'t
            // pixelPerfect
            foreach (var mine in _components)
            {
                if (!mine.hasCollision)
                {
                    // If collision isn't enabled on this component
                    // skip.
                    continue;
                }


                foreach (var others in other._components)
                {
                    //Sanity Check just in case
                    // We shouldn't have our own components check against each other                   
                    if (others == mine)
                    {
                        continue;
                    }
                    bool collisionOccured = mine.CollideWith(others.getPos);
                    others.hadCollision = collisionOccured;
                    mine.hadCollision = collisionOccured;
                    if (collisionOccured)
                    {
                        Vector2 delta = mine.getPos - others.getPos;
                        mine.addForce(delta);
                        others.addForce(-delta);
                    }


                }
            }

        }

        public void Update(GameTime gameTime)
        {

            foreach (var item in _components)
            {
                item.Update();
            }

            spd *= (1 - fricVel);
            rotvel *= fricRot;

            pos += RotateVectorAround(spd, Vector2.Zero, rotation);
            rotation += rotvel;

            // Friction. May change based on the ground

        }

        public void DrawBot(SpriteBatch sb)
        {

            foreach (var item in _components)
            {

                if (item.img != null)
                {
                    Vector2 w = item.getPos;
                    var rot = rotation;
                    Vector2 rr = RotateVectorAround(w, pos + new Vector2(50, 45), item.rot);
                    item.rot = rotation;
                    // sb.Draw(item.img, new Rectangle(position.X + item.offset.X, position.Y + item.offset.Y, 100, 100), Color.White,);
                    Color toUse = Color.White;

                    if (item.hadCollision)
                    {
                        toUse = Color.Red;
                    }

                    sb.Draw(item.img, rr, null, Color.White, item.rot,
                        new Vector2(0, 0), 3f
                        , item.imgflip ? SpriteEffects.FlipHorizontally
                        : SpriteEffects.None, item.imgLayer);

                }
            }
        }

        public List<Component> Lookat()
        {
            var w = new List<Component>();
            if (_components.Count == 0)
            {
                return null;
            }

            foreach (var component in _components)
            {
                if (component.IsHidden)
                {
                    continue;
                }
                w.Add(component);
            }
            return w;
        }
        /// <summary>
        /// Return totalHP
        /// </summary>
        public float totalHP
        {
            get
            {
                // A minimum of .5f HP to start. This give lightweight bots some
                // measure of faireness
                float thp = MINIMUM_HP;
                foreach (var comp in _components)
                {
                    thp += comp.maxHP;
                }
                return thp;
            }

        }

        public float currentHP
        {
            get
            {
                float thp = MINIMUM_HP;
                foreach (var comp in _components)
                {
                    thp += comp.currentHP;
                }
                return thp;
            }
        }

        public float getWeight
        {
            get
            {
                float w = 0;
                foreach (var item in _components)
                {
                    w += item.getWeight;
                }
                return w;
            }
        }

        public float totalEnergyGeneration
        {
            get
            {
                float x = 0;
                foreach (var item in _battery)
                {
                    x += item.getEnergyGenerated;

                }
                return x;
            }
        }

        public float totalEnergyStored
        {
            get
            {
                float x = 0;
                foreach (var item in _battery)
                {
                    x += item.getStoredEnergy;

                }
                return x;
            }
        }

        public float totalEnergyConsumed
        {
            get
            {
                float x = 0;
                foreach (var item in _battery)
                {
                    x += item.getEnergyConsumed;
                }
                return x;
            }
        }

    }
}
