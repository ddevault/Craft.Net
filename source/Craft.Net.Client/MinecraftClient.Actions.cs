using System;
using System.Threading.Tasks;
using Craft.Net.Common;
using System.Threading;
using Craft.Net.Networking;

namespace Craft.Net.Client
{
    public partial class MinecraftClient
    {
        public void LookAt(Vector3 position)
        {
            var delta = position - new Vector3(this.Position.X, this.Position.Y + 1.62, this.Position.Z);
            var yaw = Math.Atan2(-delta.X, -delta.Z);
            var groundDistance = Math.Sqrt((delta.X * delta.X) + (delta.Z * delta.Z));
            var pitch = Math.Atan2(delta.Y, groundDistance);

            this._yaw = (float)MathHelper.ToNotchianYaw(yaw);
            this._pitch = (float)MathHelper.ToNotchianPitch(pitch);

            SendPacket(new PlayerLookPacket(this.Yaw, this.Pitch, false));
        }

        public Task<Vector3> MoveTo(Vector3 position)
        {
            var diff = position - Position;
            return Move((int)diff.X, (int)diff.Z);
        }

        private Task<Vector3> CurrentMoveTask { get; set; }
        private CancellationTokenSource TaskCancellationToken { get; set; }

        public Task<Vector3> Move(int distanceX, int distanceZ)
        {
            if (CurrentMoveTask != null)
            {
                if (!CurrentMoveTask.IsCompleted)
                    TaskCancellationToken.Cancel();
            }
            TaskCancellationToken = new CancellationTokenSource();
            CurrentMoveTask = Task.Factory.StartNew(() =>
                                                    {
                var pos = this.Position + new Vector3(distanceX, 0, distanceZ);

                int xDirection = 1;
                int zDirection = 1;
                if (distanceX < 0)
                {
                    xDirection = -1;
                    distanceX *= -1;
                }
                if (distanceZ < 0)
                {
                    zDirection = -1;
                    distanceZ *= -1;
                }

                int maxMove = Math.Max(distanceX, distanceZ);
                for (int i = 0; i < maxMove; i++)
                {
                    int newX = 0, newZ = 0;
                    if (i < distanceX) 
                        newX = xDirection;
                    if (i < distanceZ)
                        newZ = zDirection;

                    this.Position += new Vector3(newX, 0, newZ);
                    this.LookAt(pos + new Vector3(0, 1.62, 0));

                    Thread.Sleep(100);

                    if (TaskCancellationToken.IsCancellationRequested)
                        return pos;
                }

                return pos;
            }, TaskCancellationToken.Token);
            return CurrentMoveTask;
        }
    }
}

