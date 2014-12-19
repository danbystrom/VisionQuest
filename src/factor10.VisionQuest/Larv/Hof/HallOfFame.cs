using System;
using System.Linq;

namespace Larv.Hof
{
    public class HallOfFame
    {
        public class Entry
        {
            public int Score;
            public DateTime When;
            public string Name;
        }

        public readonly Entry[] Entries = new Entry[10];

        public bool HasMadeIt(int score)
        {
            return score > Entries.Last().Score;
        }

        public void Insert(Entry entry)
        {
            if (!HasMadeIt(entry.Score))
                return;
            var index = Entries.Length - 1;
            for (; index > 0 && Entries[index].Score > entry.Score; index--)
                Entries[index] = Entries[index - 1];
            Entries[index] = entry;
        }

    }

}
