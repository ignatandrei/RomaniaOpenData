using System;
using System.Linq;
using ROPObjects;

namespace ROPInfrastructure
{
    public class JudetFinder
    {
        public Judet[] judete;
        public AlternateNamesJudet[] altNumeJudet;

        public Judet Find(string numeJudet)
        {
            if (string.IsNullOrWhiteSpace(numeJudet))
                return null;
            numeJudet = numeJudet.Trim().ToLower();
            var judet=judete.FirstOrDefault(it => it.Nume.ToLower() == numeJudet);
            if (judet == null)
            {
                var alt = altNumeJudet.FirstOrDefault(it => it.AlternateName.ToLower() == numeJudet);
                if(alt == null)
                    throw new ArgumentException("not found judet " + numeJudet);

                judet =judete.First(it => it.ID == alt.IDJudet);
            }
            return judet;
        }
    }
}