using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CycloneChasers
{

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _defaultSpriteFont;
        public Texture2D lol;
        public Arbitor arbiter;
        Queue<String> _bufferMsg = new Queue<String>();
        String bufferLog = "";

        List<Bot> bots = new List<Bot>();
        int _bufDelayFrames = 0;
        int bufferLogDelay = 10; // Wait 10 frames before updating the buffer
        int bufferLength = 0;
        int maxBufferSize = 8;

        Point mousePos;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            arbiter = Arbitor.instance;
            Arbitor.instance.setGame(this);
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            Bot caca = new Bot("Ghanais");
            var ba = new Component(caca, Component.componentType.hull_base, 20, Content.Load<Texture2D>("hull_base"), "Box Of Pain");

            var cap = new Component(caca, Component.componentType.hull_turret, 35, Content.Load<Texture2D>("hull_turrent"), "Cringe cap");

            var dog = new Component(caca, Component.componentType.sensor, 12, Content.Load<Texture2D>("simple_camera"), "Dog Nose");
            var trackleft = new Component(caca, Component.componentType.wheel, 35, Content.Load<Texture2D>("tanktrack-1"), "Tank wheel");
            var trackright = new Component(caca, Component.componentType.wheel, 35, Content.Load<Texture2D>("tanktrack-1"), "Tank wheel");
            var rct = new Component(caca, Component.componentType.battery, 65, Content.Load<Texture2D>("simple_battery"), "Rtc Battery");

            cap.offset = new Vector2(4f, 3f);
            rct.offset = new Vector2(17f, 17f);
            dog.offset = new Vector2(-6, -7);
            trackleft.offset = new Vector2(-25f, 0);
            trackright.offset = new Vector2(25f, 0);
            trackright.imgflip = true;

            caca.AddComponent(ba);
            caca.AddComponent(cap);
            caca.AddComponent(dog);

            caca.AddComponent(rct);
            caca.AddComponent(trackleft);
            caca.AddComponent(trackright);

            caca.pos = new Vector2(300, 300);
  
            bots.Add(caca);


            Bot caca1 = new Bot("Brama");
            var ba1 = new Component(caca, Component.componentType.hull_base, 20, Content.Load<Texture2D>("hull_base"), "Box Of Pain");

            var cap1 = new Component(caca, Component.componentType.hull_turret, 35, Content.Load<Texture2D>("Piss-mk1"), "Cringe cap");

            var dog1 = new Component(caca, Component.componentType.sensor, 12, Content.Load<Texture2D>("simple_camera"), "Dog Nose");
            var trackleft1 = new Component(caca, Component.componentType.wheel, 35, Content.Load<Texture2D>("tanktrack-1"), "Tank wheel");
            var trackright1 = new Component(caca, Component.componentType.wheel, 35, Content.Load<Texture2D>("tanktrack-1"), "Tank wheel");
            var rct1 = new Component(caca, Component.componentType.battery, 65, Content.Load<Texture2D>("simple_battery"), "Rtc Battery");

            cap1.offset = new Vector2(4f, 3f);
            rct1.offset = new Vector2(17f, 17f);
            dog1.offset = new Vector2(-6, -7);
            trackleft1.offset = new Vector2(-25f, 0);
            trackright1.offset = new Vector2(25f, 0);
            trackright1.imgflip = true;

            caca1.AddComponent(ba1);
            caca1.AddComponent(cap1);
            caca1.AddComponent(dog1);

            caca1.AddComponent(rct1);
            caca1.AddComponent(trackleft1);
            caca1.AddComponent(trackright1);

            caca1.pos = new Vector2(300, 100);
        
           /* caca1.rotation = 45;
            caca1.rotvel = 0.015f;*/
            bots.Add(caca1);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            lol = Content.Load<Texture2D>("hull_base");
            _defaultSpriteFont = Content.Load<SpriteFont>("defaultFont");
            // TODO: use this.Content to load your game content here
            _WriteLog("Hello World. o/");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mousePos.X = Mouse.GetState().X;
            mousePos.Y = Mouse.GetState().Y;

            foreach (var item in bots)
            {
                item.Update(gameTime);
            }

            var qq = Keyboard.GetState();
            // TODO: Add your update logic here

         /*   if (qq.IsKeyDown(Keys.D))
            {
                bots[0].rotation += .02f;
                bots[0].spd.Y *= 0.88f;
            }
            else
            if (qq.IsKeyDown(Keys.A))
            {
                bots[0].rotation -= .02f;
                bots[0].spd.Y *= 0.88f;
            }

            if (qq.IsKeyDown(Keys.W))
            {
                bots[0].spd.Y = -5f;
            }
            else
            if (qq.IsKeyDown(Keys.S))
            {
                bots[0].spd.Y = 5f;
            }






            */








            if (qq.IsKeyDown(Keys.W))
            {
                bots[0].spd.Y =- 10f;

            }

            if (qq.IsKeyDown(Keys.D))
            {   
                bots[0].rotvel  += .60f;
            }
            if (qq.IsKeyDown(Keys.A))
            {
                bots[0].rotvel  -= .6f;

            }
            if (qq.IsKeyDown(Keys.S))
            {
                bots[0].spd.Y = 5f;

            }

            if (qq.IsKeyDown(Keys.Up))
            {
                bots[1].spd.Y = -10f;

            }

            if (qq.IsKeyDown(Keys.Right))
            {
                bots[1].rotvel += .60f;
            }
            if (qq.IsKeyDown(Keys.Left))
            {
                bots[1].rotvel -= .6f;

            }
            if (qq.IsKeyDown(Keys.Down))
            {
                bots[1].spd.Y = 5f;

            }








            bots[1].spd.Y *= 0.6f;
            bots[0].spd.Y *= 0.6f;

            bots[1].spd.X *= 0.6f;
            bots[0].spd.X *= 0.6f;
            bots[0].rotvel /= 12f;
            bots[1].rotvel /= 12f;
            base.Update(gameTime);

        }
        public void _WriteLog(String x)
        {
            _bufferMsg.Enqueue(x);
            if (_bufferMsg.Count > maxBufferSize)
            {
                _bufferMsg.Dequeue();
            }

            var arrmsg = _bufferMsg.ToArray();
            bufferLog = "";
            for (int i = 0; i < _bufferMsg.Count; i++)
            {
                bufferLog += arrmsg[i] + "\n";
            }


        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            

            _spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);

            _spriteBatch.DrawString(_defaultSpriteFont, bufferLog, new Vector2(0, 300), Color.Green);

            foreach (var item in bots)
            {
                item.DrawBot(_spriteBatch);
            }
            _spriteBatch.End();
            

            base.Draw(gameTime);
        }
    }
}