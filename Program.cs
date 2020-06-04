using System;
using System.Linq;
using ClassLibrary10;
using System.Collections.Generic;

namespace Lab14
{
    class Program
    {
        static void Main(string[] args)
        {
            #region vars
            Organization org1 = new Organization("A", 1950);
            Organization org2 = new Organization("B", 1960);
            Organization org3 = new Organization("C", 1920);
            Library lib1 = new Library("D", 1970, 100);
            Library lib2 = new Library("E", 1800, 150);
            Library lib3 = new Library("F", 1860, 55);
            Plant plant1 = new Plant("G", 1920, 70);
            Plant plant2 = new Plant("H", 1900, 30);
            InsuranceComp incomp3 = new InsuranceComp("I", 1930, 150);
            ShipComp sp2 = new ShipComp("J", 1850, 400);
            ShipComp sp3 = new ShipComp("K", 1910, 300);

            Organization[] mass1 =  { org1, lib1, plant1 };
            Organization[] mass2 = { org2, lib2, plant2, sp2 };
            Organization[] mass3 = { org3, lib3, incomp3, sp3 };

            Dist dist1 = new Dist("Area-A", mass1);
            Dist dist2 = new Dist("Area-B", mass2);
            Dist dist3 = new Dist("Area-C", mass3);

            List<Dist> city = new List<Dist>();
            city.Add(dist1);
            city.Add(dist2);
            city.Add(dist3);
            #endregion

            Console.WriteLine("Наименование всех организаций заданного района города");
            OrgsNames(city);
            OrgsNamesExp(city);
            Console.WriteLine("Количество организаций в городе");
            NumLibs(city);
            NumLibsExp(city);
            Console.WriteLine("Суммарное количество инженеров на всех заводах города");
            EngAmount(city);
            EngAmountExp(city);
            Console.WriteLine("Наименования организаций в районах A и B города");
            ABOrgs(city);
            ABOrgsExp(city);
            Console.WriteLine("Вывод организаций района по году основания");
            FoundedOrgs(city);
            FoundedOrgsExp(city);

        }

        //
        //Наименование всех организаций заданного района города
        //
        static void OrgsNames(List<Dist> city)
        {
            var x = from dist in city where dist.Name == "Area-B" from org in dist.orgs select org;
            foreach (var org in x)
            {
                Console.Write(org.Name + "  "); 
            }
            Console.WriteLine();
        }

        static void OrgsNamesExp(List<Dist> city)
        {
            Func<Dist, bool> searchFilter = delegate (Dist dist ) { return dist.Name == "Area-B"; };
            Func<Dist, List<Organization>> itemToProcess = delegate (Dist dist) { return dist.orgs; };

            var x = city.Where(searchFilter).SelectMany(itemToProcess);
            foreach (var a in x)  Console.Write(a.Name + "  ");
            Console.WriteLine();
        }
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        //
        //Количество организаций в городе
        //
        static void NumLibs(List<Dist> city)
        {
            var numb = (from dist in city from org in dist.orgs select org).Count();
            Console.WriteLine(numb);
        }


        static void NumLibsExp(List<Dist> city)
        {
            Func<Dist, List<Organization>> allOrgs = delegate (Dist dist) { return dist.orgs; };
            Func<List<Organization>, int> counter = delegate (List<Organization> list) { return list.Count; };

            var x = city.SelectMany(allOrgs).Count();
            
            Console.WriteLine(x);
        }
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        //
        //Суммарное количество инженеров на всех заводах города
        //
        static void EngAmount(List<Dist> city)
        {
            var engs = (from dist in city 
                    from org in dist.orgs 
                    where org as Plant != null 
                    select (org as Plant).NumberOfEng).Aggregate((a,b) => a + b);
            
            Console.WriteLine(engs);
        }

        static void EngAmountExp(List<Dist> city)
        {
            Func<Dist, List<Organization>> allOrgs = delegate (Dist dist) { return dist.orgs; };
            Func<Organization, bool> searchFilter = delegate (Organization org) { return (org is Plant); };
            Func<Organization, int> item = delegate (Organization p) { return (p as Plant).NumberOfEng; };
            Func<int, int, int> sum = delegate (int a, int b) { return a + b; };

            var engs = city.SelectMany(allOrgs).Where(searchFilter).Select(item).Aggregate(sum);
            Console.WriteLine(engs);
        }
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        //
        //Наименования организаций в районах A и B города
        //
        static void ABOrgs(List<Dist> city)
        {
            var x =from list in (from dist in city where dist.Name == "Area-A" select dist.orgs).Union(from dist in city where dist.Name == "Area-B" select dist.orgs) 
                   from org in list select org;
            foreach(var org in x)
            {
                Console.Write(org.Name + "  ");
            }
            Console.WriteLine();
        }

        static void ABOrgsExp(List<Dist> city)
        {
            Func<Dist, List<Organization>> orgs = delegate (Dist dist) { return dist.orgs; };
            Func<Organization, Organization> org = delegate (Organization f) { return f; } ;
            Func<Dist, bool> filter1 = delegate (Dist dist) { return dist.Name == "Area-A"; };
            Func<Dist, bool> filter2 = delegate (Dist dist) { return dist.Name == "Area-B"; };

            var x = city.Where(filter1).SelectMany(orgs).Union(city.Where(filter2).SelectMany(orgs)).Select(org);
            foreach (var tekOrg in x)
            {
                 Console.Write(tekOrg.Name + "  ");
            }

            Console.WriteLine();
        }
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        //
        //Вывод организаций района (Area-C) по году основания
        //
        static void FoundedOrgs(List<Dist> city)
        {
            Console.WriteLine("------------------------------------------");
            var x = from dist in city where dist.Name == "Area-C" from org in dist.orgs orderby org select org;
            foreach (var org in x)
            {
                org.VShow();
                Console.WriteLine();
            }
            Console.WriteLine("------------------------------------------");
            Console.WriteLine();
        }

        static void FoundedOrgsExp(List<Dist> city)
        {
            Console.WriteLine("------------------------------------------");
            Func<Dist, List<Organization>> listItem = delegate (Dist dist) { return dist.orgs; };
            Func<Dist, bool> filter = delegate (Dist dist) { return dist.Name == "Area-C"; };
            Func<Organization, Organization> item = delegate (Organization org) { return org; };

            var x = city.Where(filter).SelectMany(listItem).OrderBy(item).Select(item);
            foreach(var org in x)
            {
                org.VShow();
                Console.WriteLine();
            }
            Console.WriteLine("------------------------------------------");
            Console.WriteLine();
        }

         //Группировка по районам
        static void DistGroups(List<Dist> city)
        {
            var x = from dist in city from org in dist.orgs group org by dist.Name;
            foreach (var y in x)
            {
                Console.Write(y.Key + ": ");
                foreach (var org in y)
                {
                     Console.Write(org.Name + "  ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static void DistGroupsExp(List<Dist> city)
        {
            Func<Dist, string> name = delegate (Dist dist) { return dist.Name; };
            Func<Dist, List<Organization>> list = delegate (Dist dist) { return dist.orgs; };

            var x = city.GroupBy(name);
            foreach (var g in x)
            {
                Console.Write(g.Key + ": ");
                var y = g.SelectMany(list);
                foreach (var org in y)
                {
                    Console.Write(org.Name + "  ");
                }
                Console.WriteLine();
            } 

        }
        
    }
}
