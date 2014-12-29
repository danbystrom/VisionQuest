using System;
using System.Linq;

namespace Larv.Hof
{
    public class HallOfFame
    {
        public class Entry
        {
            public string Name;
            public int Score;
            public DateTime When;

            public Entry(string name, int score, DateTime? when = null)
            {
                Name = name;
                Score = score;
                When = when.GetValueOrDefault(DateTime.Now);
            }
        }

        public readonly Entry[] Entries =
        {
            new Entry("LARV! by Dan Byström", 15000),
            new Entry("", 10000),
            new Entry("Use [Left], [Right] abd [Down]", 9000),
            new Entry("", 8000),
            new Entry("", 7000),
            new Entry("And when we fell together", 6000),
            new Entry("all our flesh was like a veil", 5000),
            new Entry("that I had to draw aside to see", 4000),
            new Entry("the serpent eat its tail", 3000),
            new Entry("-- Last Year's Man, Leonard Cohen", 2000)
        };

        public bool HasMadeIt(int score)
        {
            return score > Entries.Last().Score;
        }

        public int Insert(Entry entry)
        {
            if (!HasMadeIt(entry.Score))
                return -1;
            var index = Entries.Length - 1;
            for (; index > 0 && Entries[index].Score < entry.Score; index--)
                Entries[index] = Entries[index - 1];
            Entries[index] = entry;
            return index;
        }

    }

}
