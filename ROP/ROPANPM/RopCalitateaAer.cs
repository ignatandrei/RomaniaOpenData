using System.Collections.Generic;
using System.Threading.Tasks;
using ROPInfrastructure;

namespace ROPANPM
{
    public class RopCalitateaAer: RopLoader
    {
        private List<Statii> statii;
        public RopCalitateaAer()
        {
            statii=new List<Statii>();
            statii.Add(new Statii() { Statie = "AB-1", Localitate = "Alba Iulia",Judet = "Alba"});
            statii.Add(new Statii() { Statie = "AB-2", Localitate = "Sebes" ,Judet = "Alba"});
            statii.Add(new Statii() { Statie = "AB-3", Localitate = "Zlatna", Judet = "Alba" });
            statii.Add(new Statii() { Statie = "AG-1", Localitate = "Pitesti", Judet = "Arges" });
            statii.Add(new Statii() { Statie = "AG-2", Localitate = "Pitesti", Judet = "Arges" });
            statii.Add(new Statii() { Statie = "AG-3", Localitate = "Calinesti" });
            statii.Add(new Statii() { Statie = "AG-4", Localitate = "Budeasa" });
            statii.Add(new Statii() { Statie = "AG-5", Localitate = "Oarja" });
            statii.Add(new Statii() { Statie = "AG-6", Localitate = "Campulung" });
            statii.Add(new Statii() { Statie = "AR-1", Localitate = "Arad" });
            statii.Add(new Statii() { Statie = "AR-2", Localitate = "Arad" });
            statii.Add(new Statii() { Statie = "AR-3", Localitate = "Nadlac" });
            statii.Add(new Statii() { Statie = "B1 Lacul Morii", Localitate = "Bucuresti Sector 6" });
            statii.Add(new Statii() { Statie = "B2 Titan", Localitate = "Bucuresti Sector 3" });
            statii.Add(new Statii() { Statie = "B3 Mihai Bravu", Localitate = "Bucuresti Sector 2" });
            statii.Add(new Statii() { Statie = "B4 Berceni", Localitate = "Bucuresti Sector 4" });
            statii.Add(new Statii() { Statie = "B5 Drumul Taberei", Localitate = "Bucuresti Sector 5" });
            statii.Add(new Statii() { Statie = "B6 Cercul Militar", Localitate = "Bucuresti Sector 3" });
            statii.Add(new Statii() { Statie = "B7 Magurele", Localitate = "Magurele" });
            statii.Add(new Statii() { Statie = "B8 Balotesti", Localitate = "Balotesti" });
            statii.Add(new Statii() { Statie = "BC-1", Localitate = "Bacau" });
            statii.Add(new Statii() { Statie = "BC-2", Localitate = "Bacau" });
            statii.Add(new Statii() { Statie = "BC-3", Localitate = "Onesti" });
            statii.Add(new Statii() { Statie = "BH-1", Localitate = "Oradea" });
            statii.Add(new Statii() { Statie = "BH-2", Localitate = "Oradea" });
            statii.Add(new Statii() { Statie = "BH-3", Localitate = "Oradea" });
            statii.Add(new Statii() { Statie = "BH-4", Localitate = "Marghita" });
            statii.Add(new Statii() { Statie = "BN-1", Localitate = "Bistrita" });
            statii.Add(new Statii() { Statie = "BR-1", Localitate = "Braila" });
            statii.Add(new Statii() { Statie = "BR-2", Localitate = "Braila" });
            statii.Add(new Statii() { Statie = "BR-3", Localitate = "Cazacu" });
            statii.Add(new Statii() { Statie = "BR-4", Localitate = "Braila" });
            statii.Add(new Statii() { Statie = "BR-5", Localitate = "Chiscani" });
            statii.Add(new Statii() { Statie = "BT-1", Localitate = "Botosani" });
            statii.Add(new Statii() { Statie = "BV-1", Localitate = "Brasov" });
            statii.Add(new Statii() { Statie = "BV-2", Localitate = "Brasov" });
            statii.Add(new Statii() { Statie = "BV-3", Localitate = "Brasov" });
            statii.Add(new Statii() { Statie = "BV-4", Localitate = "Sanpetru" });
            statii.Add(new Statii() { Statie = "BV-5", Localitate = "Brasov" });
            statii.Add(new Statii() { Statie = "BZ-1", Localitate = "Buzau" });
            statii.Add(new Statii() { Statie = "BZ-2", Localitate = "Ramnicu Sarat" });
            statii.Add(new Statii() { Statie = "CJ-1", Localitate = "Cluj-Napoca" });
            statii.Add(new Statii() { Statie = "CJ-2", Localitate = "Cluj-Napoca" });
            statii.Add(new Statii() { Statie = "CJ-3", Localitate = "Cluj-Napoca" });
            statii.Add(new Statii() { Statie = "CJ-4", Localitate = "Cluj-Napoca" });
            statii.Add(new Statii() { Statie = "CJ-5", Localitate = "Dej" });
            statii.Add(new Statii() { Statie = "CL-1", Localitate = "Calarasi" });
            statii.Add(new Statii() { Statie = "CL-2", Localitate = "Calarasi" });
            statii.Add(new Statii() { Statie = "CS-1", Localitate = "Resita" });
            statii.Add(new Statii() { Statie = "CS-2", Localitate = "Otelu Rosu" });
            statii.Add(new Statii() { Statie = "CS-3", Localitate = "Moldova Noua" });
            statii.Add(new Statii() { Statie = "CS-4", Localitate = "Buchin" });
            statii.Add(new Statii() { Statie = "CT-1", Localitate = "Constanta" });
            statii.Add(new Statii() { Statie = "CT-2", Localitate = "Constanta" });
            statii.Add(new Statii() { Statie = "CT-3", Localitate = "Navodari" });
            statii.Add(new Statii() { Statie = "CT-4", Localitate = "Mangalia" });
            statii.Add(new Statii() { Statie = "CT-5", Localitate = "Constanta" });
            statii.Add(new Statii() { Statie = "CT-6", Localitate = "Navodari" });
            statii.Add(new Statii() { Statie = "CT-7", Localitate = "Medgidia" });
            statii.Add(new Statii() { Statie = "CV-1", Localitate = "Sfantu Gheorghe" });
            statii.Add(new Statii() { Statie = "DB-1", Localitate = "Targoviste" });
            statii.Add(new Statii() { Statie = "DB-2", Localitate = "Fieni" });
            statii.Add(new Statii() { Statie = "DJ-1", Localitate = "Craiova" });
            statii.Add(new Statii() { Statie = "DJ-2", Localitate = "Craiova" });
            statii.Add(new Statii() { Statie = "DJ-3", Localitate = "Craiova" });
            statii.Add(new Statii() { Statie = "DJ-4", Localitate = "Isalnita" });
            statii.Add(new Statii() { Statie = "DJ-5", Localitate = "Craiova" });
            statii.Add(new Statii() { Statie = "EM-1 ", Localitate = "Fundata-BV" });
            statii.Add(new Statii() { Statie = "EM-2 ", Localitate = "Semenic-CS" });
            statii.Add(new Statii() { Statie = "EM-3", Localitate = "Poiana Stampei-SV" });
            statii.Add(new Statii() { Statie = "GJ-1", Localitate = "Targu Jiu" });
            statii.Add(new Statii() { Statie = "GJ-2", Localitate = "Rovinari" });
            statii.Add(new Statii() { Statie = "GJ-3", Localitate = "Turceni" });
            statii.Add(new Statii() { Statie = "GL-1", Localitate = "Galati" });
            statii.Add(new Statii() { Statie = "GL-2", Localitate = "Galati" });
            statii.Add(new Statii() { Statie = "GL-3", Localitate = "Galati" });
            statii.Add(new Statii() { Statie = "GL-4", Localitate = "Galati" });
            statii.Add(new Statii() { Statie = "GL-5", Localitate = "Tecuci" });
            statii.Add(new Statii() { Statie = "GR-1", Localitate = "Giurgiu" });
            statii.Add(new Statii() { Statie = "GR-2", Localitate = "Giurgiu" });
            statii.Add(new Statii() { Statie = "GR-3", Localitate = "Giurgiu" });
            statii.Add(new Statii() { Statie = "GR-4", Localitate = "Oinacu" });
            statii.Add(new Statii() { Statie = "HD-1", Localitate = "Deva" });
            statii.Add(new Statii() { Statie = "HD-2", Localitate = "Deva" });
            statii.Add(new Statii() { Statie = "HD-3", Localitate = "Hunedoara" });
            statii.Add(new Statii() { Statie = "HD-4", Localitate = "Calan" });
            statii.Add(new Statii() { Statie = "HD-5", Localitate = "Vulcan" });
            statii.Add(new Statii() { Statie = "HR-1", Localitate = "Miercurea Ciuc" });
            statii.Add(new Statii() { Statie = "IL-1", Localitate = "Slobozia" });
            statii.Add(new Statii() { Statie = "IL-2", Localitate = "Urziceni" });
            statii.Add(new Statii() { Statie = "IS-1", Localitate = "Iasi" });
            statii.Add(new Statii() { Statie = "IS-2", Localitate = "Iasi" });
            statii.Add(new Statii() { Statie = "IS-3", Localitate = "Iasi" });
            statii.Add(new Statii() { Statie = "IS-4", Localitate = "Iasi" });
            statii.Add(new Statii() { Statie = "IS-5", Localitate = "Tomesti" });
            statii.Add(new Statii() { Statie = "IS-6", Localitate = "Ungheni" });
            statii.Add(new Statii() { Statie = "MH-1", Localitate = "Drobeta Turnu Severin" });
            statii.Add(new Statii() { Statie = "MM-1", Localitate = "Baia Mare" });
            statii.Add(new Statii() { Statie = "MM-2", Localitate = "Baia Mare" });
            statii.Add(new Statii() { Statie = "MM-3", Localitate = "Baia Mare" });
            statii.Add(new Statii() { Statie = "MM-4", Localitate = "Baia Mare" });
            statii.Add(new Statii() { Statie = "MM-5", Localitate = "Baia Mare" });
            statii.Add(new Statii() { Statie = "MS-1", Localitate = "Targu Mures" });
            statii.Add(new Statii() { Statie = "MS-2", Localitate = "Targu Mures" });
            statii.Add(new Statii() { Statie = "MS-3", Localitate = "Ludus" });
            statii.Add(new Statii() { Statie = "MS-4", Localitate = "Tarnaveni" });
            statii.Add(new Statii() { Statie = "NT-1", Localitate = "Piatra Neamt" });
            statii.Add(new Statii() { Statie = "NT-2", Localitate = "Roman" });
            statii.Add(new Statii() { Statie = "NT-3", Localitate = "Tasca" });
            statii.Add(new Statii() { Statie = "OT-1", Localitate = "Slatina" });
            statii.Add(new Statii() { Statie = "PH-1", Localitate = "Ploiesti" });
            statii.Add(new Statii() { Statie = "PH-2", Localitate = "Ploiesti" });
            statii.Add(new Statii() { Statie = "PH-3", Localitate = "Blejoi" });
            statii.Add(new Statii() { Statie = "PH-4", Localitate = "Brazi" });
            statii.Add(new Statii() { Statie = "PH-5", Localitate = "Ploiesti" });
            statii.Add(new Statii() { Statie = "PH-6", Localitate = "Ploiesti" });
            statii.Add(new Statii() { Statie = "SB-1", Localitate = "Sibiu" });
            statii.Add(new Statii() { Statie = "SB-2", Localitate = "Sibiu" });
            statii.Add(new Statii() { Statie = "SB-3", Localitate = "Copsa Mica" });
            statii.Add(new Statii() { Statie = "SB-4", Localitate = "Medias" });
            statii.Add(new Statii() { Statie = "SJ-1", Localitate = "Zalau" });
            statii.Add(new Statii() { Statie = "SM-1", Localitate = "Satu Mare" });
            statii.Add(new Statii() { Statie = "SM-2", Localitate = "Carei" });
            statii.Add(new Statii() { Statie = "SV-1", Localitate = "Suceava" });
            statii.Add(new Statii() { Statie = "SV-2", Localitate = "Suceava" });
            statii.Add(new Statii() { Statie = "SV-3", Localitate = "Siret" });
            statii.Add(new Statii() { Statie = "TL-1", Localitate = "Tulcea" });
            statii.Add(new Statii() { Statie = "TL-2", Localitate = "Tulcea" });
            statii.Add(new Statii() { Statie = "TL-3", Localitate = "Isaccea" });
            statii.Add(new Statii() { Statie = "TM-1", Localitate = "Timisoara" });
            statii.Add(new Statii() { Statie = "TM-2", Localitate = "Timisoara" });
            statii.Add(new Statii() { Statie = "TM-3", Localitate = "Sanandrei" });
            statii.Add(new Statii() { Statie = "TM-4", Localitate = "Timisoara" });
            statii.Add(new Statii() { Statie = "TM-5", Localitate = "Timisoara" });
            statii.Add(new Statii() { Statie = "TM-6", Localitate = "Moravita" });
            statii.Add(new Statii() { Statie = "TM-7", Localitate = "Lugoj" });
            statii.Add(new Statii() { Statie = "TR-1", Localitate = "Alexandria" });
            statii.Add(new Statii() { Statie = "TR-2", Localitate = "Turnu Magurele" });
            statii.Add(new Statii() { Statie = "VL-1", Localitate = "Ramnicu Valcea" });
            statii.Add(new Statii() { Statie = "VL-2", Localitate = "Ramnicu Valcea" });
            statii.Add(new Statii() { Statie = "VN-1", Localitate = "Focsani" });
            statii.Add(new Statii() { Statie = "VS-1", Localitate = "Vaslui" });
            statii.Add(new Statii() { Statie = "VS-2", Localitate = "Vaslui" });

        }
        public override Task FillDate()
        {
            
        }
    }
}