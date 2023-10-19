//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Text;

//namespace Game_Multi_Play_Server
//{
//    enum function : byte
//    {
//        ONMATCH = 0,
//        MATCHFOUND = 1,
//        MEREADY = 2,
//        NOTREADY = 3,
//        ALLREADY = 4,
//        USESKILL = 5,
//        MAKETOWER = 6,
//    }
//    static class Form
//    {
//        public abstract class form_Manage
//        {
//            public abstract EndPoint excute(User_Identity uId);

//        }
//        class NOTREADY : form_Manage
//        {
//            public override EndPoint excute(User_Identity uId)
//            {
//                return null;
//            }
//        }

//        class ALLREADY : form_Manage
//        {
//            public override EndPoint excute(User_Identity uId)
//            {
//                return null;
//            }
//        }
//        class USESKILL : form_Manage
//        {
//            public override EndPoint excute(User_Identity uId)
//            {
//                return null;
//            }
//        }
//        class MAKETOWER : form_Manage
//        {
//            public override EndPoint excute(User_Identity uId)
//            {
//                return Player_Table.send(uId.data[1], uId.data[2]);
//            }
//        }

//        private static readonly byte form_Manage_Length = 10;
//        public static form_Manage[] fm = new form_Manage[form_Manage_Length];
//        public static void boot()
//        {
//            try
//            {
//                fm[(byte)function.NOTREADY] = new NOTREADY();
//                fm[(byte)function.ALLREADY] = new ALLREADY();
//                fm[(byte)function.USESKILL] = new USESKILL();
//                fm[(byte)function.MAKETOWER] = new MAKETOWER();
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("!!!Form Loading Failed!!!");
//                Console.WriteLine("<Form Load Error>");
//                Console.WriteLine(e.Message);
//                Console.WriteLine("<Form Load Error>");
//            }
//            finally
//            {

//            }

//        }
//    }
//}