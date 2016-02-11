namespace ROPObjects
{
    public enum UATTip
    {
        CJ =11 ,//consiliu judetean
        M=12,//Municipiu
        O=13,//Oras
        C=14,//Comuna
        B=15,//Bucuresti
        S=16,//Primaria de sector al M. Buc.
    }
    public class UAT
    {
        public string Nume { get; set; }
        public UATTip UatTip { get; set; }
        public Judet Judet { get; set; }

    }
}