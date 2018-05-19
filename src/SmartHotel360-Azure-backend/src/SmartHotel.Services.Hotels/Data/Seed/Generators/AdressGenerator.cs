using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.Hotels.Data.Seed
{
    public class AddressGenerator
    {
        private static List<string> _postCodes = new List<string>
        {
            "60014",
            "48047",
            "98801",
            "08861",
            "13126",
            "35124",
            "52001",
            "27804",
            "37643",
            "30040",
            "60515",
            "23139",
            "29680",
            "60621",
            "58102",
            "33569",
            "48089",
            "29550",
            "91733",
            "38017"
        };

        private static List<string> _streets = new List<string>
        {
            "1127 Eraad Lane",
            "1720 Fogug Boulevard",
            "830 Wapcig View",
            "67 Wuhenu Street",
            "512 Tadta Pass",
            "1399 Hoca Key",
            "1201 Botdip Court",
            "750 Hafek Center",
            "483 Dunzek Road",
            "56 Zinun Trail",
            "655 Zivi Pike",
            "1367 Zicsu Boulevard",
            "1319 Abcus Lane",
            "502 Hulnek Street",
            "1132 Sakohe Boulevard",
            "1208 Bifi Plaza",
            "1966 Vuvwu Glen",
            "681 Fikkep Grove",
            "1916 Pinut Drive",
            "1374 Epoeca Center",
            "1285 Ijapa River",
            "1868 Fonnub Plaza",
            "737 Nabha Turnpike",
            "492 Osise Court",
            "56 Tokpuh Manor",
            "1835 Bobvu Grove",
            "972 Adleb Heights",
            "1593 Notuz Parkway",
            "1560 Nume Grove",
            "998 Humzi Square",
            "281 Ibama Path",
            "841 Wope Point",
            "1128 Jaspi Trail",
            "1047 Depi Parkway",
            "613 Pamic Pike",
            "982 Onhip Ridge",
            "876 Sovep Park",
            "281 Ijiife Turnpike",
            "661 Wafor Circle",
            "91 Raama Grove",
            "1694 Cavele Circle",
            "1260 Efmu Pike",
            "1248 Hazi Extension",
            "1640 Hinfuw Avenue",
            "494 Nigo Parkway",
            "517 Morfe Point",
            "2000 Bedev Road",
            "1988 Jukev River",
            "24 Sofani Parkway",
            "267 Esutid Ridge",
            "84 Juzav Boulevard",
            "1745 Matik Pike",
            "808 Mazob Circle",
            "903 Topobi Lane",
            "1096 Cufow Highway",
            "875 Givad Pike",
            "863 Ufasa Way",
            "588 Dici View",
            "1840 Getev Key",
            "1543 Tezve Way",
            "260 Zeaz Glen",
            "303 Waobi Court",
            "54 Samaj Ridge",
            "1921 Inbu Way",
            "500 Riduca Square",
            "845 Feri Loop",
            "534 Zujoza Drive",
            "23 Vazse Grove",
            "1491 Tino Key",
            "1508 Hepik Junction",
            "1919 Bijse Key",
            "1231 Huko Street",
            "320 Hacer Extension",
            "1335 Najte Mill",
            "1327 Getfeb Heights",
            "912 Dekgi Street",
            "154 Kupso View",
            "1327 Utlom Turnpike",
            "1739 Bagfac Heights",
            "825 Dotfo Park",
            "1394 Oluha Trail",
            "61 Utbol Circle",
            "109 Wijrud Point",
            "1072 Awja Heights",
            "1018 Obevir Extension",
            "10 Gobfil Place",
            "1080 Golac River",
            "1182 Pevtak Pike",
            "1138 Uzpu Road",
            "1076 Viero Key",
            "1468 Jifab Key",
            "1662 Vuhcen View",
            "941 Osotu Junction",
            "1171 Magika Trail",
            "1491 Okosun Road",
            "530 Vakep Path",
            "1447 Suco Trail",
            "1358 Ikeku Circle",
            "1810 Gejah Extension",
            "678 Apecuc Heights",
            "1869 Mavuga Trail",
            "882 Emevo Key",
            "562 Mokice View",
            "385 Akuehe Trail",
            "732 Wufi Drive",
            "439 Koplim Street",
            "1449 Majcu View",
            "1646 Oriro Loop",
            "378 Bihdu Highway",
            "501 Ucgaj Turnpike",
        };
        
        private Stack<string> _availableStreets = new Stack<string>(_streets);

        public string GetPostCode()
        {
            var random = new Random();
            var index = random.Next(_postCodes.Count - 1);
            return _postCodes[index];
        }

        public string GetStreet()
        {
            return _availableStreets.Pop();
        }
    }
}
