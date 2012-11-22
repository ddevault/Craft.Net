using Craft.Net.Data.NbtSerialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Craft.Net.Data
{
   [TagName("Data")]
   public class SavedLevel
   {
      [TagName("raining")]
      public bool IsRaining { get; set; }

      [TagName("generatorVersion")]
      public int GeneratorVersion { get; set; }

      public long Time { get; set; }

      [TagName("GameType")]
      public int GameMode { get; set; }

      public bool MapFeatures { get; set; }

      [TagName("generatorName")]
      public string GeneratorName { get; set; }

      [TagName("initialized")]
      public bool Initialized { get; set; }

      [TagName("hardcore")]
      public bool Hardcore { get; set; }

      [TagName("RandomSeed")]
      public long Seed { get; set; }

      [NbtIgnore]
      public Vector3 SpawnPoint { get; set; }

      #region SpawnPoint Members
      [EditorBrowsable(EditorBrowsableState.Never)]
      public int SpawnX
      {
         get
         {
            return (int)SpawnPoint.X;
         }
         set
         {
            var point = SpawnPoint;
            point.X = value;
            SpawnPoint = point;
         }
      }

      [EditorBrowsable(EditorBrowsableState.Never)]
      public int SpawnY
      {
         get
         {
            return (int)SpawnPoint.Y;
         }
         set
         {
            var point = SpawnPoint;
            point.Y = value;
            SpawnPoint = point;
         }
      }

      [EditorBrowsable(EditorBrowsableState.Never)]
      public int SpawnZ
      {
         get
         {
            return (int)SpawnPoint.Z;
         }
         set
         {
            var point = SpawnPoint;
            point.Z = value;
            SpawnPoint = point;
         }
      }
      #endregion

      public long SizeOnDisk { get; set; }

      [TagName("thunderTime")]
      public int ThunderTime { get; set; }

      [TagName("rainTime")]
      public int RainTime { get; set; }

      [TagName("version")]
      public int Version { get; set; }

      [TagName("thundering")]
      public bool Thundering { get; set; }

      public string LevelName { get; set; }

      public long LastPlayed { get; set; }

      [IgnoreOnNull]
      public string GeneratorOptions { get; set; }
   }
}