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
        public void Update(GameTime gameTime)
        {
            pos += RotateVectorAround(spd, Vector2.Zero, rotation) * vel;
            rotation += rotvel;
        }
        public void DrawBot(SpriteBatch sb)
        {

            foreach (var item in _components)
            {

                if (item.img != null)
                {
                    Vector2 w = (pos + item.offset);
                    var rot = rotation;
                    Vector2 rr = RotateVectorAround(w, pos + new Vector2(50, 45), rot);

                    // sb.Draw(item.img, new Rectangle(position.X + item.offset.X, position.Y + item.offset.Y, 100, 100), Color.White,);
                    sb.Draw(item.img, rr, null, Color.White, rot, new Vector2(0, 0), 3f, item.imgflip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, item.imgLayer);

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
