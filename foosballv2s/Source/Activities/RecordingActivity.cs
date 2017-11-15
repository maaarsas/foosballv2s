using System;
using System.IO;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using RS = Android.Renderscripts;
using Android.Util;
using Emgu.CV;
using Emgu.CV.Structure;
using foosballv2s.Source.Services.FoosballWebService.Repository;
using Java.Interop;
using Java.IO;
using Java.Lang;
using Xamarin.Forms;
using Camera = Android.Hardware.Camera;
using Color = Android.Graphics.Color;
using Console = System.Console;
using Element = Xamarin.Forms.Element;
using Size = Xamarin.Forms.Size;
using Type = System.Type;
using View = Android.Views.View;

namespace foosballv2s
{
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

        [Export("Team1Goal")]
        public void Team1Goal(View view)
        {
            game.Team1Score++;
            team1ScoreView.Text = game.Team1Score.ToString();
            CheckGameEnd(game);
        }
        
        [Export("Team2Goal")]
        public void Team2Goal(View view)
        {
            game.Team2Score++;
            team2ScoreView.Text = game.Team2Score.ToString();
            CheckGameEnd(game);
        }

        private void CheckGameEnd(Game game)
        {
            if (game.HasEnded)
            {
                SaveGame(game);
                ShowGameEndScreen();
            }
        }

        private async void SaveGame(Game game)
        {
            ProgressDialog dialog = ProgressDialog.Show(this, "", 
                Resources.GetString(Resource.String.saving_game), true);

            await gameRepository.Create(game);
            
            dialog.Dismiss();
        }

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

        [Export("GameEndOkClick")]
        public void GameEndOkClick(View view)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }

        private void OnSurfaceViewTouch(object sender, View.TouchEventArgs e)
        {
            DrawRectangle(e.Event.GetX(), e.Event.GetY());
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            if (camera != null) {
                camera.StopPreview();
                camera.Release();
                camera = null;
            }
            return false;
        }

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
            Size bestSize = ActivityHelper.GetBestPreviewSize(camera.GetParameters(), textureView.Width, textureView.Height);
            tmp.SetPreviewSize((int) bestSize.Width, (int) bestSize.Height);
            tmp.FocusMode = Camera.Parameters.FocusModeContinuousPicture;
            camera.SetParameters(tmp);
            movementDetector.SetupBallDetector(textureView.Width, textureView.Height, game.BallColor);
            this.textureSetup = true;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height) { }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface) { }

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