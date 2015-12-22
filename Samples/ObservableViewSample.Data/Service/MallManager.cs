﻿using System.Collections.ObjectModel;

using ObservableViewSample.Data.Model;

namespace ObservableViewSample.Data.Service
{
    public class MallManager : IMallManager
    {
        private static readonly ObservableCollection<Mall> Malls;

        public ObservableCollection<Mall> GetMalls()
        {
            return Malls;
        }

        static MallManager()
        {
            Malls = new ObservableCollection<Mall>
                          {
                              new Mall("Abercrombie & Fitch / abercrombie kids", "Level-a | (480) 792-9275"),
                              new Mall("Abercrombie & Fitch / abercrombie kids", "Level-b | (480) 792-9275"),
                              new Mall("Abercrombie & Fitch / abercrombie kids", "Level-c | (480) 792-9275"),
                              new Mall("Abercrombie & Fitch / abercrombie adults", "Level-c | (480) 792-9275"),
                              new Mall("Abercrombie & Fitch / abercrombie grannies", "Level-c | (480) 792-9275"),
                              new Mall("ALDO", "Level-A | (480) 899-0803"),
                              new Mall("ALDO", "Level-B | (480) 899-0803"),
                              new Mall("ALDO", "Level-Z | (480) 899-0803"),
                              new Mall("All Mobile Matters Mobile Phone Repair & More", "Level 2 | (480) 228-9690"),
                              new Mall("Alterations By L", "Level 1 | (480) 786-8092"),
                              new Mall("AMERICAN EAGLE OUTFITTERS", "Level 2 | (480) 812-0090"),
                              new Mall("Ann Taylor", "Level 1 | (480) 726-6944"),
                              new Mall("Apex by sunglass hut", "Level 2 | (480) 855-1709"),
                              new Mall("Apple Store", "Level 1 | (480) 726-8082"),
                              new Mall("At UR Convenience", "Level 1 | (480) 246-4055"),
                              new Mall("at&t", "Level 2 | (480) 347-0334"),
                              new Mall("ATHLETA", "Level 1 | (480) 899-3560"),
                              new Mall("Auntie Anne's", "Level 1 | (480) 726-2700"),
                              new Mall("AVEDA", "Level 1 | (480) 726-1585"),
                              new Mall("Bad Boy Blades", "Level 2"),
                              new Mall("Banana Republic", "Level 1 | (480) 726-6476"),
                              new Mall("Barbizon", "Level 2"),
                              new Mall("bareMinerals", "Level 2 | (480) 855-5327"),
                              new Mall("BARNES & NOBLE", "Level 2 | (480) 792-1312"),
                              new Mall("Bath & Body Works", "Level 2 | (480) 821-7511"),
                              new Mall("Beauty Booth", "Level 1 | (480) 917-2372"),
                              new Mall("bebe", "Level 1 | (480) 812-2203"),
                              new Mall("Ben Bridge", "Level 2 | (480) 722-1100"),
                              new Mall("Benihana", "Boulevard Shops | (480) 812-4701"),
                              new Mall("Best Buy", "Level 1, Level 2 | (480) 792-1680"),
                              new Mall("BJ's Restaurant | Brewhouse", "Level 1 | (480) 917-0631"),
                              new Mall("Boba Tea Company", "Level 2 | (480) 639-1832"),
                              new Mall("Body Language", "Level 2 | (480) 786-4030"),
                              new Mall("Brighton Collectibles", "Level 1 | (480) 726-0263"),
                              new Mall("Brookstone", "Level 1 | (603) 318-4216"),
                              new Mall("Buckle", "Level 2 | (480) 726-3906"),
                              new Mall("Buffalo Wild Wings", "Level 1 | (480) 289-5200"),
                              new Mall("Build-A-Bear Workshop", "Level 2 | (480) 821-5800"),
                              new Mall("Calendar Club", "Level 1 | (480) 726-7100"),
                              new Mall("California Pizza Kitchen", "Level 1 | (480) 855-3301"),
                              new Mall("Capri Jewelers", "Level 2 | (480) 726-3470"),
                              new Mall("cathy jean", "Level 2 | (480) 899-1386"),
                              new Mall("Cellairis", "Level 2 | (480) 792-9706"),
                              new Mall("CHICO'S", "Level 1 | (480) 722-9076"),
                              new Mall("chili's", "Boulevard Shops | (480) 786-1289"),
                              new Mall("Chipotle Mexican Grill", "Boulevard Shops | (480) 499-2884"),
                              new Mall("claire's", "Level 2 | (480) 812-4870"),
                              new Mall("Clarks", "Level 1 | (480) 917-0293"),
                              new Mall("COACH", "Level 1 | (480) 792-9464"),
                              new Mall("Cold Stone Creamery", "Level 1 | (480) 722-7767"),
                              new Mall("Compass Bank", "Boulevard Shops | (480) 403-7820"),
                              new Mall("COTTON ON", "Level 2 | (480) 857-1474"),
                              new Mall("crazy 8", "Level 2 | (480) 917-6946"),
                              new Mall("crocs", "Level 2 | (480) 857-1037"),
                              new Mall("Dairy Queen / Orange Julius", "Level 2 | (480) 821-2664"),
                              new Mall("Diamond Wireless", "Level 1 | (480) 899-4701"),
                              new Mall("Dillard's", "Level 1, Level 2 | (480) 735-2060"),
                              new Mall("Disney Store", "Level 2 | (480) 812-1661"),
                              new Mall("Dolce Salon & Spa", "Boulevard Shops | (480) 722-0500"),
                              new Mall("DownEast basics", "Level 2 | (480) 857-7057"),
                              new Mall("Eddie Bauer", "Level 1 | (480) 855-6248"),
                              new Mall("ELEPHANT BAR RESTAURANT", "Boulevard Shops | (480) 899-8088"),
                              new Mall("ETHAN ALLEN", "Boulevard Shops | (480) 838-0404"),
                              new Mall("Evalectric", "Level 1"),
                              new Mall("Express", "Level 2 | (480) 917-3905"),
                              new Mall("Famous Dave's", "Level 1 | (480) 782-1212"),
                              new Mall("Fashion Nails and Spa", "Level 2 | (480) 963-9608"),
                              new Mall("FAST-FIX JEWELRY AND WATCH REPAIRS", "Level 2 | (480) 786-6132"),
                              new Mall("Finish Line", "Level 2 | (480) 722-2818"),
                              new Mall("Firebirds Wood Fired Grill", "Boulevard Shops | (480) 814-8003"),
                              new Mall("Fitness 4 Home Superstore", "Boulevard Shops | (480) 838-0555"),
                              new Mall("Five Guys", "Boulevard Shops | (480) 855-2839"),
                              new Mall("Focus Shoes", "Level 1"),
                              new Mall("FOSSIL", "Level 2 | (480) 722-9530"),
                              new Mall("francesca's", "Level 1 | (480) 899-1820"),
                              new Mall("Freddy's Frozen Custard & Steakburgers", "Level 1 | (480) 857-8815"),
                              new Mall("Free People", "Level 1 | (480) 899-7699"),
                              new Mall("Fuzziwig's Candy Factory", "Level 2 | (480) 726-2953"),
                              new Mall("GameStop", "Level 2 | (480) 899-2009"),
                              new Mall("GAP", "Level 2 | (480) 726-0426"),
                              new Mall("Gemsetter & Company", "Boulevard Shops | (480) 752-2017"),
                              new Mall("GNC Live Well.", "Level 1 | (480) 855-7085"),
                              new Mall("Go Wireless", "Boulevard Shops | (480) 821-4276"),
                              new Mall("Godiva Chocolatier", "Level 1 | (480) 855-3468"),
                              new Mall("GYMBOREE", "Level 2 | (480) 917-3769"),
                              new Mall("GYRO KING", "Level 2 | (480) 821-0255"),
                              new Mall("H&M", "Level 1 | (480) 857-1389"),
                              new Mall("Harkins Theatres", "Level 1, Level 2 | (480) 732-0110"),
                              new Mall("HAT CLUB", "Level 2 | (480) 855-3251"),
                              new Mall("HELZBERG DIAMONDS", "Level 2 | (480) 732-0055"),
                              new Mall("Hibachi-San", "Level 2 | (480) 857-0500"),
                              new Mall("HOLLISTER Co.", "Level 2 | (480) 855-7980"),
                              new Mall("HOT TOPIC", "Level 2 | (480) 722-7601"),
                              new Mall("HOUSE OF HOOPS by Foot Locker", "Level 2 | (480) 726-7172"),
                              new Mall("icing", "Level 2 | (480) 917-2996"),
                              new Mall("inDusTRial Rideshop", "Level 2 | (480) 812-8881"),
                              new Mall("Innovative Foto", "Level 2"),
                              new Mall("iPlay and Accessories", "Level 1 | (480) 812-8488"),
                              new Mall("J. Jill", "Level 1 | (480) 963-9770"),
                              new Mall("Jahan", "Level 1"),
                              new Mall("Jimmy John's", "Boulevard Shops | (480) 722-1860"),
                              new Mall("Johnny Rockets", "Level 1 | (480) 855-0000"),
                              new Mall("JOURNEYS", "Level 2 | (480) 899-7311"),
                              new Mall("Journeys Kidz", "Level 2 | (480) 855-9020"),
                              new Mall("Just Relax", "Level 2"),
                              new Mall("Just Sports", "Level 2 | (480) 855-9444"),
                              new Mall("Justice & Brother", "Level 2 | (480) 899-9490"),
                              new Mall("Kay Jewelers", "Level 2 | (480) 855-1422"),
                              new Mall("kids Foot Locker", "Level 2 | (480) 899-9026"),
                              new Mall("Kona Grill", "Level 2 | (480) 792-1711"),
                              new Mall("LaserAway", "Boulevard Shops | (602) 892-5006"),
                              new Mall("La-Z-Boy", "Boulevard Shops | (480) 831-1848"),
                              new Mall("LEGO", "Level 2 | (480) 899-0228"),
                              new Mall("LensCrafters", "Level 2 | (480) 726-9001"),
                              new Mall("Lids", "Level 2 | (480) 899-0771"),
                              new Mall("Loco-Motives, LLC", "Level 1 | (480) 812-8488"),
                              new Mall("LOFT", "Level 1 | (480) 899-3414"),
                              new Mall("LOVESAC", "Level 2 | (480) 899-0334"),
                              new Mall("LUCKY BRAND", "Level 1 | (480) 722-0760"),
                              new Mall("lululemon athletica", "Level 1 | (480) 899-0545"),
                              new Mall("LUSH", "Level 2 | (480) 857-2344"),
                              new Mall("Macy's", "Level 1, Level 2 | (480) 821-8611"),
                              new Mall("Madison Avenue Int'l Salon & Day Spa", "Level 1 | (480) 899-8905"),
                              new Mall("Majerle's Sports Grill", "Boulevard Shops | (480) 899-7999"),
                              new Mall("MasterCuts", "Level 1 | (480) 726-8885"),
                              new Mall("McDonald's", "Level 2 | (480) 961-3050"),
                              new Mall("MERLE NORMAN", "Boulevard Shops | (480) 782-8331"),
                              new Mall("Michael Kors", "Level 1 | (480) 722-7669"),
                              new Mall("Microsoft Store", "Level 1 | (602) 824-6090"),
                              new Mall("Modern Mommy Boutique", "Boulevard Shops | (480) 857-7187"),
                              new Mall("Motherhood Maternity", "Level 1 | (480) 722-0632"),
                              new Mall("N3L Optics", "Level 2 | (480) 855-1709"),
                              new Mall("Nationwide Insurance", "Boulevard Shops | (480) 586-3672"),
                              new Mall("NEW YORK & COMPANY", "Level 1 | (480) 792-6555"),
                              new Mall("Nick's Menswear", "Level 1 | (480) 857-7931"),
                              new Mall("Nordstrom", "Level 1, Level 2 | (480) 855-2500"),
                              new Mall("Nordstrom In House Cafe & Coffee Bar", "Level 1 | (480) 855-2500"),
                              new Mall("Oceane'", "Level 2"),
                              new Mall("Oil & Vinegar", "Level 2 | (480) 963-9490"),
                              new Mall("Omaha Steaks", "Boulevard Shops | (480) 792-1133"),
                              new Mall("ORIGINS", "Level 2 | (480) 857-8262"),
                              new Mall("P.F. Chang's", "Boulevard Shops | (480) 899-0472"),
                              new Mall("PACSUN", "Level 2 | (480) 782-5151"),
                              new Mall("PANDA EXPRESS", "Level 2 | (480) 812-0700"),
                              new Mall("Pandora", "Level 2"),
                              new Mall("Paradise Bakery & Café", "Level 2 | (480) 857-3523"),
                              new Mall("PARIS OPTIQUE", "Level 2 | (480) 855-4622"),
                              new Mall("Parker & Sons", "Level 2 | (602) 574-6523"),
                              new Mall("Payless ShoeSource", "Level 2 | (480) 899-6112"),
                              new Mall("PERFUMANIA", "Level 2 | (480) 855-0080"),
                              new Mall("Perfumology", "Level 1 | (480) 899-9446"),
                              new Mall("Pet Match", "Level 2 | (602) 781-3906"),
                              new Mall("PhantomSkinz", "Level 2 | (480) 855-6760"),
                              new Mall("Piercing Pagoda", "Level 2 | (480) 782-5196"),
                              new Mall("Popcornopolis", "Level 2 | (480) 855-0234"),
                              new Mall("Pottery Barn", "Level 1 | (480) 899-7055"),
                              new Mall("proactiv SOLUTION", "Level 2 | (877) 275-2561 x3078"),
                              new Mall("Q", "Level 2 | (480) 821-3577"),
                              new Mall("Qrew", "Level 1 | (480) 917-3809"),
                              new Mall("REGIS", "Level 2 | (480) 722-9666"),
                              new Mall("Rubio's Fresh Mexican Grill", "Level 2 | (480) 812-9460"),
                              new Mall("s.h.a.p.e.s Brow Bar", "Level 2 | (480) 917-3349"),
                              new Mall("Sears", "Level 1, Level 2 | (480) 855-2800"),
                              new Mall("Sears Automotive & Tire Center", "Level 1, Level 2 | (480) 855-2895"),
                              new Mall("Sears Optical", "Level 1 | (480) 782-8971"),
                              new Mall("See's Candies", "Level 1 | (480) 726-2825"),
                              new Mall("SEPHORA", "Level 1 | (480) 726-7733"),
                              new Mall("Shiekh", "Level 1 | (480) 821-9490"),
                              new Mall("Shoe Palace", "Level 1 | (480) 812-2099"),
                              new Mall("Sir Veza's Taco Garage", "Level 2 | (480) 899-8226"),
                              new Mall("SKECHERS", "Level 2 | (480) 963-8600"),
                              new Mall("sleep number by Select Comfort", "Level 1 | (480) 214-0244"),
                              new Mall("Soma Intimates", "Level 1 | (480) 812-3707"),
                              new Mall("SPENCER'S", "Level 2 | (480) 855-5736"),
                              new Mall("Sprint Store by Wireless Evolution", "Level 2 | (480) 963-2365"),
                              new Mall("STARBUCKS COFFEE", "Level 1 | (480) 917-1963"),
                              new Mall("Steak Escape Sandwich Grill", "Level 2 | (480) 857-7090"),
                              new Mall("Steel Shield Security Doors", "Level 2"),
                              new Mall("SUBWAY", "Level 2 | (480) 722-0506"),
                              new Mall("Sunglass Hut", "Level 1 | (480) 786-0193"),
                              new Mall("Sunglass Hut", "Level 2 | (480) 726-0720"),
                              new Mall("TEAVANA", "Level 2 | (480) 855-8400"),
                              new Mall("The ART of SHAVING", "Level 1 | (480) 499-9022"),
                              new Mall("THE BODY SHOP", "Level 2 | (480) 855-5941"),
                              new Mall("The Cheesecake Factory", "Level 2 | (480) 792-1300"),
                              new Mall("The Children's Place", "Level 2 | (480) 855-0632"),
                              new Mall("The Keg Steakhouse + Bar", "Boulevard Shops | (480) 899-7500"),
                              new Mall("The Limited", "Level 1 | (480) 786-9578"),
                              new Mall("The Mutual Fund Store", "Boulevard Shops | (480) 855-3322"),
                              new Mall("the old spaghetti factory", "Level 1 | (480) 786-5706"),
                              new Mall("The Walking Company", "Level 1 | (480) 917-9255"),
                              new Mall("THINGS REMEMBERED", "Level 2 | (480) 855-0871"),
                              new Mall("TiLLY'S", "Level 2 | (480) 722-1741"),
                              new Mall("T-Mobile", "Level 2 | (480) 855-8594"),
                              new Mall("TONI&GUY", "Level 2 | (480) 963-7171"),
                              new Mall("Tonsorial Barber", "Level 2 | (480) 855-7140"),
                              new Mall("Totally Chi Reflexology", "Level 2 | (480) 310-6096"),
                              new Mall("Travel Outfitters", "Level 1 | (480) 855-4327"),
                              new Mall("Vans", "Level 2 | (480) 821-4654"),
                              new Mall("Vaper's Paradise", "Level 2 | (480) 855-5369"),
                              new Mall("Vera Bradley", "Level 2 | (480) 821-9396"),
                              new Mall("verizon wireless DIGITELL / Premium Retailer", "Level 1 | (480) 858-0800"),
                              new Mall("Victoria's Secret", "Level 1 | (480) 224-6936"),
                              new Mall("Victoria's Secret Beauty", "Level 1 | (480) 224-6936"),
                              new Mall("Villa Pizza", "Level 2 | (480) 821-1672"),
                              new Mall("Vine Vera", "Level 2"),
                              new Mall("Visionworks", "Level 1 | (480) 782-9380"),
                              new Mall("WELLS FARGO", "Boulevard Shops | (480) 821-7474"),
                              new Mall("Wetzel's Pretzels", "Level 2 | (480) 899-6461"),
                              new Mall("White House | Black Market", "Level 1 | (480) 782-1850"),
                              new Mall("Wildflower Bread Company", "Level 1 | (480) 821-8200"),
                              new Mall("windsor", "Level 2 | (480) 786-9348"),
                              new Mall("Xtreme Toys", "Level 1 | (480) 855-0304"),
                              new Mall("XXI Forever", "Level 1 | (480) 963-4454"),
                              new Mall("YANKEE CANDLE", "Level 2 | (480) 821-9308"),
                              new Mall("Yogurtland", "Boulevard Shops | (480) 857-2515"),
                              new Mall("ZAGG InvisibleShield", "Level 2 | (480) 812-8488"),
                              new Mall("Zales", "Level 2 | (480) 857-4101"),
                              new Mall("zumiez", "Level 2")
                          };
        }
    }
}