using System.Collections.Generic;

namespace Larv
{
    public static class PlayingFields
    {
        private static List<string[]> GetZ()
        {
            var list = new List<string[]>();
            list.Add(
                new[]
                    {
                        "AaUUU               ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "               UUUbB"
                    });

            list.Add(
                new[]
                    {
                        "     DXXXXXXXXXXXXXX",
                        "       X           X",
                        "XXXXXXXXXXXXXXXXXXXX",
                        "X X X  X           X",
                        "X X X  X           X",
                        "XXX XXXXUUU        X",
                        "X                  X",
                        "XUUU               X",
                        "X                  X",
                        "X                  X",
                        "XXXXXXXXXXXXXXXXXXXX",
                        "X               X  X",
                        "X               X  X",
                        "X               X  X",
                        "XXXXXXXXXXXXX  X   X",
                        "X           XXXXXXXX",
                        "X    U      X X     ",
                        "X    U      XXX     ",
                        "X    U       X      ",
                        "XXXXXXXXXXXXXXD     "
                    });
            list.Add(
                new[]
                    {
                        "                    ",
                        "                    ",
                        "                    ",
                        "  XXXXXXXXXXXXXXXX  ",
                        "  X              X  ",
                        "  XXXX     DXXXXXX  ",
                        "     X           X  ",
                        "    DXXXXXXXXXXXXX  ",
                        "     X       X U    ",
                        "  XXXXXXXXXXXX U    ",
                        "  X X X X      U    ",
                        "  XXX XXX           ",
                        "   X   X            ",
                        "   XX XX            ",
                        "    XXX             ",
                        "     D              ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                    });
            list.Add(
                new[]
                    {
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "                    ",
                        "               D    ",
                        "XXXXXXXXXXXXXXXXXXXX",
                        "   X   X   X   X   X",
                        "X  X XXX XXX XXX XXX",
                        "XX X   X   X   X   X",
                        "XXXX XXX   X   X XXX",
                        "X XX   X   X   X   X",
                        "X  X XXX   X   X XXX",
                        "                    "
                    });

            return list;
        }

        private static List<string[]> GetQ1()
        {
            var list = new List<string[]>();
            list.Add(
                new[]
                    {
"XXXXXXXXXXXXXXXXXXXXXXXXX",
"X X       X   X   X     X",
"X XXX XXX X XXXXX XXXXX X",
"X   X X X X X   X X   X X",
"X XXX X XXX XXX X XXX X X",
"X X   X   X   X X   X X X",
"XXX XXXXX XXXXX XXXXX XXX",
"X X X X X X     X       X",
"X X X X X XXXXXXXXXXXXX X",
"X X X X X X X X       X X",
"X XXX X XXX X XXXXXXXXXXX",
"X X X X X X X X       X X",
"X X X X X XXXXXXXXXXXXX X",
"X X X X X X     X       X",
"XXX XXXXX XXXXX XXXXX XXX",
"S X   X   X   X X   X X S",
"X XXX X XXX XXX X XXX X X",
"X   X X X X X   X X   X X",
"X XXX XXX X XXXXX XXXXX a",
"b X       X   X   X     a",
"B XXXXXXXXXXXXXXXXX     A",
                    });
            return list;
        }

        private static List<string[]> GetQ2()
        {
            var list = new List<string[]>();
            list.Add(
                new[]
                    {
"XXXXX XXXXXXXXXXXXX XXXXX",
"X   X X     X     X X   X",
"XXX X XXXXX X XXXXX X XXX",
"  X X X X X X X X X X X  ",
"XXXXXXX X XXXXX X XXXXXXX",
"X   X   X       X   X   X",
"XXXXXXXXXXXXXXXXXXXXXXXXX",
"  X X     X X X     X X  ",
"XXX X XXXXX X XXXXX X XXX",
"X   X X     X     X X   X",
"XXXXX X XXXXXXXXX X XXXXX",
"    X X X X   X X X X    ",
"XXXXXXXXX XXXXX XXXXXXXXX",
"X   X   X   X   X   X   X",
"XXX XXX XXXXXXXXX XXX XXX",
"S X   X X       X X   X S",
"X XXXXXXXXXXXXXXXXXXXXX X",
"X X   X X X X X X     X X",
"X X XXX X X X X X XXXXX X",
"b X X   X X X X X X     a",
"B XXXXXXX XXXXX XXX     A",
                    });
            return list;
        }

        private static List<string[]> GetQ3()
        {
            var list = new List<string[]>();
            list.Add(
                new[]
                    {
"XXXXXXX XXXXXXXXX XXXXXXX",
"X     X X   X   X X     X",
"X XXX XXX XXXXX XXX XXX X",
"X X X X X X X X X X X X X",
"X X XXX X X X X X XXX X X",
"X X   X XXX X XXX X   X X",
"X XXX XXX X X X XXX XXX X",
"X X X X X X X X X X X X X",
"X X X X X X X X X X X X X",
"XXX XXX XXX X XXX XXX XXX",
"X   X   X   X   X   X   X",
"XXXXXXXXXXXXXXXXXXXXXXXXX",
"X   X   X   X   X   X   X",
"XXX XXX XXX X XXX XXX XXX",
"SXXXX X X X X X X X XXXXS",
"X X   X X X X X X X   X X",
"X X XXX XXX X XXX XXX X X",
"X X X   X X X X X   X X X",
"X XXXXX X X X X X XXXXX X",
"b X   X X X X X X X     a",
"B XXXXXXXXXXXXXXXXX     A",

                    });
            return list;
        }

        private static List<string[]> GetQ4()
        {
            var list = new List<string[]>();
            list.Add(
                new[]
                    {
"XXXXXXXXXXX X XXXXXXXXXXX",
"X X   X   X   X   X   X X",
"X XXX XXX XXXXX XXX XXX X",
"X   X   X   X   X   X   X",
"XXX XXX XXX X XXX XXX XXX",
"X X   X   X X X   X   X X",
"X XXX XXX XXXXX XXX XXX X",
"X   X   X   X   X   X   X",
"XXX XXXXXXXXXXXXXXXXX XXX",
"  X X     X   X     X X  ",
"XXXXXXXXXXXXXXXXXXXXXXXXX",
"X X       X X X       X X",
"X XXXXX XXX X XXX XXXXX X",
"X     X X   X   X X     X",
"XXXXX XXX XXXXX XXX XXXXX",
"S X X X   X X X   X X X S",
"X X XXX XXX X XXX XXX X X",
"X X X   X   X   X   X X X",
"X XXX XXX XXXXX XXX XXX X",
"b X   X   X   X   X     a",
"B XXXXXXXXX X XXXXX     A"

                    });
            return list;
        }

        public static List<string[]> GetLevel(int level)
        {
            switch (level)
            {
                case 0:
                    return GetQ4();
                case 1:
                    return GetQ2();
                default:
                    return GetZ();
            }
        }

    }
}
