using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Emgu.CV;
using Emgu.CV.Structure;
using foosballv2s.Source.Activities.Helpers;
using foosballv2s.Source.Entities;
using foosballv2s.Source.Services.FoosballWebService.Repository;
using foosballv2s.Source.Services.GameRecognition;
using Java.Interop;
using Xamarin.Forms;
using Camera = Android.Hardware.Camera;
using Color = Android.Graphics.Color;
using Size = Xamarin.Forms.Size;
using View = Android.Views.View;

namespace foosballv2s.Source.Activities
{
    /// <summary>
    /// An activity for the game recording, score calculating and recognition
    /// </summary>
    [Activity(
        ConfigurationChanges = ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Landscape
    )]
    public class RecordingActivity : Activity, TextureView.ISurfaceTextureListener, Camera.IPreviewCallback
    {
        private Camera camera;
        private TextureView textureView;
        private SurfaceView surfaceView;
        private ISurfaceHolder holder;
        private MovementDetector movementDetector;

        private TextView team1ScoreView;
        private TextView team2ScoreView;

        private Game game;
        private GameRepository gameRepository;


        private bool textureSetup;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Recording);

            textureView = (TextureView)FindViewById(Resource.Id.textureView);
            textureView.SurfaceTextureListener = this;

            surfaceView = (SurfaceView)FindViewById(Resource.Id.surfaceView);
            //set to top layer
            surfaceView.SetZOrderOnTop(true);
            //set the background to transparent
            surfaceView.Holder.SetFormat(Format.Transparent);
            holder = surfaceView.Holder;
            surfaceView.Touch += OnSurfaceViewTouch;

            movementDetector = new MovementDetector();
            
            game = DependencyService.Get<Game>();
            gameRepository = DependencyService.Get<GameRepository>();

            game.Start();
            
            this.Window.AddFlags(WindowManagerFlags.Fullscreen);
            
            // set up displayed texts on the screen
            TextView team1NameView = (TextView) FindViewById(Resource.Id.team1_name);
            team1NameView.Text = game.Team1.TeamName;
            
            TextView team2NameView = (TextView) FindViewById(Resource.Id.team2_name);
            team2NameView.Text = game.Team2.TeamName;

            team1ScoreView = (TextView) FindViewById(Resource.Id.team1_score);
            team2ScoreView = (TextView) FindViewById(Resource.Id.team2_score);

            Task.Run(async () => FeedMovementDetector());
            var clockTimer = new Timer(new TimerCallback(UpdateGameTimer), null,  TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1));
        }

        /// <summary>
        /// Called on the first team's goal
        /// </summary>
        /// <param name="view"></param>
        [Export("Team1Goal")]
        public void Team1Goal(View view)
        {
            game.Team1Score++;
            team1ScoreView.Text = game.Team1Score.ToString();
            CheckGameEnd(game);
        }
        
        /// <summary>
        /// Called on the second team's goal
        /// </summary>
        /// <param name="view"></param>
        [Export("Team2Goal")]
        public void Team2Goal(View view)
        {
            game.Team2Score++;
            team2ScoreView.Text = game.Team2Score.ToString();
            CheckGameEnd(game);
        }

        /// <summary>
        /// Checks if the maximum score is and the end of the game is reached
        /// </summary>
        /// <param name="game"></param>
        private void CheckGameEnd(Game game)
        {
            if (game.HasEnded)
            {
                SaveGame(game);
                ShowGameEndScreen();
            }
        }

        /// <summary>
        /// Saves the game asynchronously
        /// </summary>
        /// <param name="game"></param>
        private async void SaveGame(Game game)
        {
            ProgressDialog dialog = ProgressDialog.Show(this, "", 
                Resources.GetString(Resource.String.saving_game), true);


            await gameRepository.Create(game);
            
            dialog.Dismiss();
        }

        /// <summary>
        /// Prepares the shows the end of the game view
        /// </summary>
        private void ShowGameEndScreen()
        {
            // set up the text of winning team
            TextView winningTeamTextView = (TextView) FindViewById(Resource.Id.game_end_team_won);
            string winningTeamText = Resources.GetString(Resource.String.game_end_team_won);
            winningTeamTextView.Text = System.String.Format(winningTeamText, game.WinningTeam.TeamName);
            
            // set up the text of the result
            TextView resultTextView = (TextView) FindViewById(Resource.Id.game_end_result);
            string resultText = Resources.GetString(Resource.String.game_end_result);
            resultTextView.Text = System.String.Format(resultText, 
                game.Team1.TeamName, game.Team2.TeamName, game.Team1Score, game.Team2Score);
            
            // show game end layout
            LinearLayout gameEndLayout = (LinearLayout) FindViewById(Resource.Id.game_end_layout);
            gameEndLayout.Visibility = ViewStates.Visible;
        }

        /// <summary>
        /// Called when the Ok button is clicked in the end of the game view
        /// </summary>
        /// <param name="view"></param>
        [Export("GameEndOkClick")]
        public void GameEndOkClick(View view)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }

        /// <summary>
        /// Draws a rectangle onto the touched place
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSurfaceViewTouch(object sender, View.TouchEventArgs e)
        {
            DrawRectangle(e.Event.GetX(), e.Event.GetY());
        }

        /// <summary>
        /// Called when the surface view is destroed
        /// Closes the camera
        /// </summary>
        /// <param name="surface"></param>
        /// <returns></returns>
        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            if (camera != null) {
                camera.StopPreview();
                camera.Release();
                camera = null;
            }
            return false;
        }

        /// <summary>
        /// Called when the surface view is available
        /// Opens and setups the camera
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            camera = Camera.Open();
            try
            {
                camera.SetPreviewTexture(surface);
                camera.StartPreview();
                camera.SetPreviewCallback(this);
            }
            catch (Java.IO.IOException ex) { }

            Camera.Parameters tmp = camera.GetParameters();
            Camera.Size size = tmp.PreviewSize;

            tmp.SetPreviewSize((int) size.Width, (int) size.Height);
            tmp.FocusMode = Camera.Parameters.FocusModeContinuousPicture;
            camera.SetParameters(tmp);
            movementDetector.SetupBallDetector(textureView.Width, textureView.Height, game.BallColor);
            this.textureSetup = true;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height) { }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface) { }

        /// <summary>
        /// Draws a rectangle onto the screen
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void DrawRectangle(float x, float y)
        {
            //define the paintbrush
            Paint mpaint = new Paint();
            mpaint.Color = Color.Red;
            mpaint.SetStyle(Paint.Style.Stroke);
            mpaint.StrokeWidth = 2f;

            //draw
            Canvas canvas = holder.LockCanvas();
            //clear the paint of last time
            canvas.DrawColor(Color.Transparent, PorterDuff.Mode.Clear);
            //draw a new one, set your ball's position to the rect here
            Rect r = new Rect((int)x, (int)y, (int)x + 100, (int)y + 100);
            canvas.DrawRect(r, mpaint);
            holder.UnlockCanvasAndPost(canvas);

        }
        
        /// <summary>
        /// Draws a circle onto the screen
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="radius"></param>
        private void DrawCircle(float x, float y, float radius)
        {
            //define the paintbrush
            Paint mpaint = new Paint();
            mpaint.Color = Color.Red;
            mpaint.SetStyle(Paint.Style.Stroke);
            mpaint.StrokeWidth = 5f;

            //draw
            Canvas canvas = holder.LockCanvas();
            //clear the paint of last time
            canvas.DrawColor(Color.Transparent, PorterDuff.Mode.Clear);
            //draw a new one, set your ball's position to the rect here
            canvas.DrawCircle(x, y, radius, mpaint);
            holder.UnlockCanvasAndPost(canvas);

        }

        public void OnPreviewFrame(byte[] data, Camera camera)
        {
        }

        /// <summary>
        /// Passes the camera frames to the movement detector and draws the detected ball circles
        /// </summary>
        private void FeedMovementDetector()
        {
            while (!game.HasEnded)
            {
                if (this.textureSetup)
                {
                    Bitmap frameBitmap = textureView.Bitmap;
                                                 
                    Image<Hsv, System.Byte> hsvFrame = new Image<Hsv, byte>(frameBitmap);
                    Bitmap bitmap = hsvFrame.Bitmap;
                    
                    CircleF[] circles = movementDetector.DetectBall(hsvFrame, textureView.Height, textureView.Width);
                    
                    foreach (CircleF circle in circles)
                    {
                        DrawCircle(circle.Center.X, circle.Center.Y, circle.Radius);
                        break;
                    }
                    frameBitmap.Recycle();
                }
            }
           
        }

        /// <summary>
        /// Updates the game timer
        /// </summary>
        /// <param name="stateInfo"></param>
        private void UpdateGameTimer(object stateInfo)
        {
            if (!game.HasEnded)
            {
                string timeString = GameTimeHelper.GetTimeString(game.StartTime, game.EndTime);
                RunOnUiThread(() =>
                {
                    TextView timeTextView = (TextView) FindViewById(Resource.Id.game_time);
                    timeTextView.Text = timeString;
                });
            }
        }
    }
}