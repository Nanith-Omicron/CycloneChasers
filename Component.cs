using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace CycloneChasers
{
    internal class Component
    {
        public delegate void ComponentDelegate(Component x); // Students can add their special fonction when certain action happens
        public ComponentDelegate OnBreak, OnInitialized, OnTakeDamage, OnRepair;

        Component _parent;
        Component _powerSupply;
        List<Component> supplying = new List<Component>();

        String _componentName = "Basic Component";
        String _vendor = "Omicron Corp";
        String _desc = "Basic Omicron Corp Component. Usually Fragile and lightweight but efficient";

        Bot _owner;

        public enum componentType
        {
            none = 0,
            hull_base = 1,
            hull_turret = 2,
            wheel = 3,
            motor = 4,
            processor = 5,
            sensor = 6,
            hull_armor = 7,
            weapon = 8,
            fixture = 9,
            battery = 10

        }

        public Component(Bot owner, componentType ty, float we, Texture2D _img, String nae = "Basic Component", String desc = "", String vend = "")
        {
            setOwner(owner);
            img = _img;
            this._componentName = nae;
            this._vendor = vend;
            this._desc = desc;
            this.weight = we;
            this.type = ty;
            if (ty == componentType.wheel)
            {
                imgLayer = 0;
            }
            if (ty == componentType.hull_base)
            {
                imgLayer = 0.2f;
            }

            if (ty == componentType.hull_turret)
            {
                imgLayer = 0.5f;
            }

            if (ty == componentType.sensor)
            {
                imgLayer = 0.6f;
            }

            if (ty == componentType.battery)
            {
                imgLayer = 0.24f;
            }

        }
        public Texture2D img;
        public Vector2 pos, offset, spd;
        public float rotSpd, rot;
        public bool hasCollision = true;


        Vector2 size;
        public float imgLayer = 0;
        public bool imgflip = false;

        float _currentHP = 5;
        float HP = 5;
        float weight = 1; // in kg
        float voltageNeeded = 3.3f;

        float energyConsumption = 0;
        float energyGeneration = 0;

        float storedEnergy = 0;
        float maximumStoredEnergy = 100;
        public float getStoredEnergy { get { return storedEnergy; } }
        public float getEnergyGenerated { get { return energyGeneration; } }
        public float getEnergyConsumed { get { return energyConsumption; } }
        float voltageSupply = 3.3f;

        public bool isElectrized { get { return energyConsumption > 0; } }
        public bool workasBattery { get { return energyGeneration > 0; } }
        bool _broken = false;

        bool _hidden = false;
public bool hadCollision = false;

        public Vector2 getPos
        {
            get
            {
                if (_owner != null)
                {
                    pos = _owner.pos + offset;
                }
                return pos;
            }
        }
        public void setHidden(bool x)
        {
            _hidden = x;
        }

        public bool IsHidden { get { return _hidden; } }

        componentType type = componentType.none;
        String logPrefix = "";

        public void setOwner(Bot owner)
        {
            _owner = owner;
            logPrefix = "[" + _owner.getName + "] ";

        }

        public virtual void Initialized()
        {
            _currentHP = HP;
            if (OnInitialized != null)
            {
                OnInitialized(this);
            }
            Log("initialized.");

            OnBoot();
        }
        public virtual void OnBoot()
        {
            if (!isElectrized) { return; }
        }

        public void addForce(Vector2 force)
        {
            if (_owner != null)
            {
                _owner.AddForce(force);
            }
            else
            {
                //TODO: ADD FORCE TO COMPONENT BY ITSELF
            }
        }

        // Default collideWith is a Simple AABB
        public virtual bool CollideWith(Vector2 b)
        {
            return AABB(b);
        }

        bool AABB(Vector2 b)
        {
            var curPos = getPos;
            var posX = getPos.X;
            var posY = getPos.Y;

            if (b.X > posX && b.X < posX + size.X)
            {
                if (b.Y > posY && b.Y < posY + size.Y)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        void electronic()
        {

            this.storedEnergy += energyGeneration;
            if (storedEnergy < 0) storedEnergy = 0;
            if (storedEnergy > maximumStoredEnergy) storedEnergy = maximumStoredEnergy;

            if (workasBattery)
            {
                powerOther();
            }


        }
        float providePower(Component bat)
        {
            if (bat == null) return 0;
            if (bat.type != componentType.battery) return 0;
            if (type == componentType.battery) return 0;
            float drawO = 1;
            if (bat.storedEnergy < this.energyConsumption)
            {
                drawO = bat.storedEnergy / this.energyConsumption;
                if (drawO < 0)
                {
                    drawO = 0;
                }
            }
            float efficiency = drawO * bat.voltageSupply / bat.voltageNeeded;
            float draw = 0;
            if (efficiency > 0)
            {
                draw += this.energyConsumption * efficiency;
                ElectrizedUpdate(efficiency);
            }
            return drawO * this.energyConsumption;
        }
        void powerOther()
        {
            if (supplying.Count < 0)
            {
                return;
            }

            foreach (var item in supplying)
            {
                this.storedEnergy -= item.providePower(this);
            }
        }
        public virtual void ElectrizedUpdate(float efficiency)
        {

        }

        /// <summary>
        /// Call only on electrized component at its shutdown
        /// </summary>
        public virtual void OnShutdown()
        {
            if (!isElectrized) { return; }
        }

        /// <summary>
        /// Call every ticks
        /// </summary>
        public void Update()
        {
            electronic();
            pos += spd;
            rot += rotSpd;
            spd *= (1 - 0.4f);

        }


        public float maxHP
        {
            get { return HP; }
        }
        public float currentHP
        {
            get { return currentHP; }
        }
        public float getWeight
        {
            get { return weight; }
        }
        public componentType getType
        {
            get { return this.type; }
        }
        public String getTypeName
        {
            get
            {
                switch (type)
                {

                    case componentType.hull_base:
                        return "hull_base";
                    case componentType.hull_turret:
                        return "hull_turret";
                    case componentType.wheel:
                        return "wheel";
                    case componentType.motor:
                        return "motor";

                    case componentType.processor:
                        return "processor";

                    case componentType.sensor:
                        return "sensor";

                    case componentType.hull_armor:
                        return "hull_armor";

                    case componentType.weapon:
                        return "weapon";

                    case componentType.fixture:
                        return "fixture";

                    case componentType.battery:
                        return "battery";

                    case componentType.none:
                    default:
                        return "none";
                }
            }
        }
        public bool isBroken
        {
            get { return _broken; }
        }

        public String name { get { return _componentName; } }

        public bool Repair(float x)
        {
            if (isBroken)
            {
                return false;
            }
            _currentHP += Math.Abs(x); // :)

            if (_currentHP > HP)
            {
                _currentHP = HP;
            }
            if (OnRepair != null)
            {
                OnRepair(this);
            }
            Log("Repair Successful");
            return true;

        }
        public void Restore()
        {
            _broken = false;
            Repair(maxHP);
        }
        public void takeDamage(int x, UInt16 ID)
        {
            Log("Took " + x + " from " + "Bot [" + ID + "]");
            _currentHP -= Math.Abs(x);
            if (OnTakeDamage != null)
            {
                OnTakeDamage(this);
            }
            if (_currentHP <= 0)
            {
                _currentHP = 0;
                Break();
            }
        }
     
        public void Log(String x)
        {
            Arbitor.WriteLog(logPrefix + _componentName + ": " + x);
        }

        void Break()
        {
            Log("has break!");
            _broken = true;
            if (OnBreak != null)
            {
                OnBreak(this);
            }
        }





    }
}
