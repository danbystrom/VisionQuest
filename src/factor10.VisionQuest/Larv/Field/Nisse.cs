using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Larv.Util;

namespace Larv.Field
{
    public class PlayingFieldInfo
    {
        public PlayingFieldSquare[, ,] PlayingField;
        public Whereabouts PlayerSerpentStart;
        public Whereabouts EnemySerpentStart;
    }

    public class Nisse
    {
        public readonly List<PlayingFieldInfo> PlayingFields = new List<PlayingFieldInfo>();

        public Nisse(IEnumerable<string> description)
        {
            var cleaned = description.Select(_ => _.Trim()).Where(_ => _.Length > 0).ToArray();
            for (var i = 0; i < cleaned.Length;)
                decodeLevel(ref i, cleaned);
        }

        private void decodeLevel(ref int i, string[] description)
        {
            var rex = new Regex(@"^\*SCENE(\d+),(\d+)x(\d+)x(\d+)\*$");
            var q = rex.IsMatch(description[i]);
            var split = rex.Split(description[i]);
            var scenes = int.Parse(split[1]);
            var height = int.Parse(split[2]);
            var width = int.Parse(split[3]);
            var floors = int.Parse(split[4]);

            var fields = new List<string[]>();
            for (var f = 0; f < floors; f++)
                fields.Add(getFloor(i + 1, description, width, height));

            var pfi = new PlayingFieldInfo();
            pfi.PlayingField = PlayingFieldBuilder.Create(fields, width, height, ref pfi.PlayerSerpentStart, ref pfi.EnemySerpentStart);

            i += 1 + height;
        }

        private string[] getFloor(int i, string[] description, int width, int height)
        {
            var result = new string[height];
            for (var j = 0; j < height; j++)
            {
                var row = description[i];
                if (row[0] != '\"' || row.Last() != '\"' || row.Length != width + 2)
                    throw new Exception("Malformed playingfield row: " + row);
                result[j] = row.Substring(1, width);
            }
            return result;
        }
    }
}
