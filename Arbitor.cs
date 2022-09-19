using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycloneChasers
{
    public class Arbitor
    {
        static Arbitor _arb = null;
        static bool isPresent = false;
        static bool hasGame = false;
        public static void WriteLog(String s)
        {
            if(!isPresent && !hasGame) //Sanity check
            {
                Console.WriteLine("Error, Arbitor isn't set. Play add an isntance of a Game");
                return;
            }

            _arb._game._WriteLog(s);   
        }


        Game1 _game;
        public void setGame(Game1 mg)
        {
            isPresent = true;
            hasGame = true;
            _game = mg;
        }
        public Arbitor()
        {

        }
        public static Arbitor instance
        {
            get
            {
                if (_arb == null)
                {
                    _arb = new Arbitor();
                    isPresent = true;
                }
                return _arb;
            }
        }
    }
}

