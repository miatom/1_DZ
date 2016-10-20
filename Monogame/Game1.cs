using IGenericList;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;


namespace Monogame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 900,
                PreferredBackBufferWidth = 500

            };
            Content.RootDirectory = "Content";
        }

        

        public Paddle PaddleBottom { get; private set;}
        /// <summary >
        /// Top paddle object
        /// </ summary >
        public Paddle PaddleTop { get; private set; }
        /// <summary >
        /// Ball object
        /// </ summary >
        public Ball Ball { get; private set; }
        /// <summary >
        /// Background image
        /// </ summary >
        public Background Background { get; private set; }

        public SoundEffect HitSound { get; private set; }

        public Song Music { get; private set; }

        private IGenericList<Sprite> SpritesForDrawList = new GenericList<Sprite>();
       

        public List <Wall> Walls { get ; set ; }
        public List<Wall> Goals { get; set; }
        

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            var screenBounds = GraphicsDevice.Viewport.Bounds;
            
            
           

            PaddleBottom = new Paddle(GameConstants.PaddleDefaultWidth, GameConstants.PaddleDefaulHeight, GameConstants.PaddleDefaulSpeed);
            PaddleBottom.X = 100;
            PaddleBottom.Y = 700;

            PaddleTop = new Paddle(GameConstants.PaddleDefaultWidth,GameConstants.PaddleDefaulHeight, GameConstants.PaddleDefaulSpeed);
            PaddleTop.X = 100;
            PaddleTop.Y = 200;

            Ball = new Ball(GameConstants.DefaultBallSize, GameConstants.DefaultInitialBallSpeed, GameConstants.DefaultBallBumpSpeedIncreaseFactor)
            {
                X = 250,
                Y = 450
            };
            Background = new Background( 500 , 900);
            SpritesForDrawList.Add(Background);
            SpritesForDrawList.Add(PaddleBottom);
            SpritesForDrawList.Add(PaddleTop);
            SpritesForDrawList.Add(Ball);
           

            Walls = new List<Wall>()

            {

                // try with 100 for default wall size !
                new Wall ( -GameConstants.WallDefaulSize ,0 , GameConstants.WallDefaulSize , screenBounds.Height ) ,
                new Wall ( screenBounds.Right ,0 , GameConstants.WallDefaulSize ,screenBounds.Height ) ,

            };
            Goals = new List<Wall>()

            {

                new Wall (0 , screenBounds.Height , screenBounds.Width , GameConstants.WallDefaulSize ) ,
                new Wall ( screenBounds.Top , -GameConstants.WallDefaulSize , screenBounds.Width , GameConstants.WallDefaulSize ) ,

            };
            base.Initialize();
        }

    

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            // Set textures
            Texture2D paddleTexture = Content.Load<Texture2D>("paddle");
            PaddleBottom.Texture = paddleTexture;
            PaddleTop.Texture = paddleTexture;
            Ball.Texture = Content.Load<Texture2D>("ball");
            Background.Texture = Content.Load<Texture2D>("background");
            // Load sounds
            // Start background music
            HitSound = Content.Load<SoundEffect>("hit");
            //Music = Content.Load<Song>("music");
            //MediaPlayer.IsRepeating = true;
            // MediaPlayer.Play(Music);
           
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
           
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var touchState = Keyboard.GetState();

            if (touchState.IsKeyDown(Keys.Left))
            {
                PaddleBottom.X = PaddleBottom.X - (float)(PaddleBottom.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
                PaddleTop.X = PaddleTop.X - (float)(PaddleTop.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
            }

            if (touchState.IsKeyDown(Keys.Right))
            {
                PaddleBottom.X = PaddleBottom.X + (float)(PaddleBottom.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
                PaddleTop.X = PaddleTop.X + (float)(PaddleTop.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
            }
                       
            PaddleBottom.X = MathHelper.Clamp(PaddleBottom.X, 0, 500 -PaddleBottom.Width);
            PaddleTop.X = MathHelper.Clamp(PaddleTop.X, 0, 500-PaddleTop.Width);

            var ballPositionChange = Ball.Direction * (float)(gameTime.ElapsedGameTime.TotalMilliseconds * Ball.Speed);
           
            Ball.X += ballPositionChange.X;
            Ball.Y += ballPositionChange.Y;
            //var smjerx = Ball.Direction.X;
           // var smjery = Ball.Direction.Y;

            var bounds = GraphicsDevice.Viewport.Bounds;
            
                        // Ball - side walls
                        if (Walls.Any(w => CollisionDetector.Overlaps(Ball, w)))
                        {
                            Ball.Direction = new Vector2(Ball.Direction.X*(-1),Ball.Direction.Y);   
                            Ball.Speed = GameConstants.DefaultInitialBallSpeed; //Ball.Speed * Ball.BumpSpeedIncreaseFactor;

                        }
         
                        // Ball - winning walls
                        if (Goals.Any(w => CollisionDetector.Overlaps(Ball, w)))
                        {
                            Ball.X = bounds.Center.ToVector2().X;
                            Ball.Y = bounds.Center.ToVector2().Y;
                            Ball.Speed = GameConstants.DefaultInitialBallSpeed;
                            HitSound.Play();
                        }
            
                           // Paddle - ball collision
                           if (CollisionDetector.Overlaps(Ball, PaddleTop) && Ball.Direction.Y < 0 || (CollisionDetector.Overlaps(Ball, PaddleBottom) && Ball.Direction.Y > 0))
                           {
                               Ball.Direction =new Vector2(Ball.Direction.X,Ball.Direction.Y*(-1));
                               Ball.Speed = GameConstants.DefaultInitialBallSpeed; //*=Ball.BumpSpeedIncreaseFactor;
                           }

                   
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            // Start drawing .
            spriteBatch.Begin();
            for (int i = 0; i < SpritesForDrawList.Count; i++)
            {
                SpritesForDrawList.GetElement(i).DrawSpriteOnScreen(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }



    public abstract class Sprite: IPhysicalObject2D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Texture2D Texture { get; set; }
        protected Sprite(int width, int height, float x = 0, float y = 0)
        {
            X = x;
            Y = y;
            Height = height;
            Width = width;
        }
        /// <summary >
        /// Base draw method
        /// </ summary >
        public virtual void DrawSpriteOnScreen(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Vector2(X, Y), new Rectangle(0, 0, Width, Height), Color.White);
        }
    }

    public class Background : Sprite
    {
        public Background(int width, int height) : base(width, height)
        {
        }
    }

    public class Ball : Sprite
    {
        
        public float Speed { get; set; }
        public float BumpSpeedIncreaseFactor { get; set; }
       
        public Vector2 Direction { get; set; }
        public Ball(int size, float speed, float defaultBallBumpSpeedIncreaseFactor) : base(size, size)
        {
            Speed = speed;
            BumpSpeedIncreaseFactor = defaultBallBumpSpeedIncreaseFactor;
            // Initial direction
            Direction = new Vector2(1, 1);
            
        }
    }

    public class Paddle : Sprite
    {
        /// <summary >
        /// Current paddle speed in time
        /// </ summary >
        public float Speed { get; set; }
        public Paddle(int width, int height, float initialSpeed) : base(width,
        height)
        {
            Speed = initialSpeed;
        }
        public override void DrawSpriteOnScreen(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Vector2(X, Y), new Rectangle(0, 0, Width, Height), Color.GhostWhite);
        }
    }

    public class GameConstants
    {
        public const float PaddleDefaulSpeed = 1f ;
        public const int PaddleDefaultWidth = 250;
        public const int PaddleDefaulHeight = 20;
        public const float DefaultInitialBallSpeed = 1f ;
        public const float DefaultBallBumpSpeedIncreaseFactor = 1.05f ;
        public const int DefaultBallSize = 40;
        internal static int WallDefaulSize = 100;
}

    public interface IPhysicalObject2D
    {
        float X { get; set; }
        float Y { get; set; }
        int Width { get; set; }
        int Height { get; set; }
    }

    public class CollisionDetector
    {
        public static bool Overlaps(IPhysicalObject2D a, IPhysicalObject2D b)
        {
            float x1a, y1a, x2a, y2a;
            float x1b, y1b, x2b, y2b;
            x1a = a.X;
            x2a = a.X + a.Width;
            y1a = a.Y;
            y2a = a.Y + a.Height;

            x1b = b.X;
            x2b = b.X + b.Width;
            y1b = b.Y;
            y2b = b.Y + b.Height;

            if (x1a < x2b && x2a > x1b && y1a < y2b && y2a > y1b)
            {
                return true;
            }
            return false;

        }
    }

    public class Wall : IPhysicalObject2D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
       
        public Wall(float x, float y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }

   

}

