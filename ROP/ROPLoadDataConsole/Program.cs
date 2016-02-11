using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROPSiruta;

namespace ROPLoadDataConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var sl = new SirutaLoader();
            var jud = sl.InitJudete().Result;
            foreach (var judet in jud)
            {
                Console.WriteLine(judet.Nume +"---" + judet.Cod);
            }
        }
    }
}
