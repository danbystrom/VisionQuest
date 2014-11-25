using System;
using factor10.VisionThing;
using Larv.Serpent;
using Serpent;
using SharpDX;
using SharpDX.Toolkit;

namespace Larv.GameStates
{
    class BeginGameState : IGameState
    {
        private readonly Serpents _serpents;
        private MoveCamera _moveCamera;

        public BeginGameState(Serpents serpents)
        {
            _serpents = serpents;
            _serpents.PlayerSerpent.Restart(_serpents.PlayingField.PlayerWhereaboutsStart, 1);

            var currentPos = serpents.Camera.Position;

            var lookAtDirection = serpents.PlayerSerpent.Whereabouts.Direction.DirectionAsVector3();
            var lookAt = serpents.PlayerSerpent.LookAtPosition + lookAtDirection * 4;

            var finalNormal = Vector3.TransformNormal(
                lookAtDirection*SerpentCamera.CameraDistanceToHeadXz*1.3f,
                Matrix.RotationY(-MathUtil.Pi*0.2f));
            var finalPos = lookAt + finalNormal;
            finalPos.Y += SerpentCamera.CameraDistanceToHeadY;

            var x = new ArcGenerator(4);
            x.CreateArc(
                currentPos,
                finalPos,
                Vector3.Right,
                SerpentCamera.CameraDistanceToHeadXz);
            _moveCamera = MoveCamera.TotalTime(
                serpents.Camera,
                4,
                lookAt,
                x.Points);
        }

        public void Update(Camera camera, GameTime gameTime, ref IGameState gameState)
        {
            if (_moveCamera!=null)
            {
                if(_moveCamera.Move(gameTime))
                    return;
                //_moveCamera = null;
            }

            _serpents.PlayerSerpent.Update(_serpents.Camera, gameTime);
            if (_serpents.PlayingField.FieldValue(_serpents.PlayerSerpent.Whereabouts).Restricted != Direction.None)
                gameState = new PlayingState(_serpents,_moveCamera);
        }

        public void Draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            _serpents.Draw(camera, drawingReason, shadowMap);
        }

    }

}
