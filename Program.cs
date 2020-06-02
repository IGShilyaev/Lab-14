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
            var x = from dist in city where dist.Name == "Area-B" select dist.orgs;
            foreach (var a in x) foreach (Organization org in a) Console.Write(org.Name + "  ");
            Console.WriteLine();
        }

        static void OrgsNamesExp(List<Dist> city)
        {
            Func<Dist, bool> searchFilter = delegate (Dist dist ) { return dist.Name == "Area-B"; };
            Func<Dist, List<Organization>> itemToProcess = delegate (Dist dist) { return dist.orgs; };

            var x = city.Where(searchFilter).Select(itemToProcess);
            foreach (var a in x) foreach (Organization org in a) Console.Write(org.Name + "  ");
            Console.WriteLine();
        }
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        //
        //Количество организаций в городе
        //
        static void NumLibs(List<Dist> city)
        {
            int numb = 0;
            var list = from dist in city select dist.orgs;
            foreach(var x in list)
            {
                numb += (from org in x select org).Count();
            }
            Console.WriteLine(numb);
        }



        static void NumLibsExp(List<Dist> city)
        {
            int numb = 0;
            Func<Dist, List<Organization>> allOrgs = delegate (Dist dist) { return dist.orgs; };
            Func<List<Organization>, int> counter = delegate (List<Organization> list) { return list.Count; };

            var x = city.Select(allOrgs);
            numb += x.Select(counter).Aggregate((a,b) => a + b);
            Console.WriteLine(numb);
        }
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        //
        //Суммарное количество инженеров на всех заводах города
        //
        static void EngAmount(List<Dist> city)
        {
            int engs = 0;
            var orgs = from dist in city select dist.orgs;
            foreach(var x in orgs)
            {
                engs += (from org in x where org as Plant != null select (org as Plant).NumberOfEng).Sum();
            }

            Console.WriteLine(engs);
        }

        static void EngAmountExp(List<Dist> city)
        {
            int engs = 0;
            Func<Dist, List<Organization>> allOrgs = delegate (Dist dist) { return dist.orgs; };
            Func<Organization, bool> searchFilter = delegate (Organization org) { return (org is Plant); };
            Func<Organization, Plant> item = delegate (Organization p) { return p as Plant; };
            Func<Plant, int> value = delegate (Plant p) { return p.NumberOfEng; };

            var x = city.Select(allOrgs);
            foreach (var y in x)
            {
                var tekOrg = y.Where(searchFilter).Select(item);
                engs += tekOrg.Select(value).Sum();
            }
            Console.WriteLine(engs);
        }
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        //
        //Наименования организаций в районах A и B города
        //
        static void ABOrgs(List<Dist> city)
        {
            var x = (from dist in city where dist.Name == "Area-A" select dist.orgs).Union(from dist in city where dist.Name == "Area-B" select dist.orgs);
            foreach(var y in x)
            {
                foreach (Organization org in y) Console.Write(org.Name + "  ");
            }
            Console.WriteLine();
        }

        static void ABOrgsExp(List<Dist> city)
        {
            Func<Dist, List<Organization>> orgs = delegate (Dist dist) { return dist.orgs; };
            Func<Dist, bool> filter1 = delegate (Dist dist) { return dist.Name == "Area-A"; };
            Func<Dist, bool> filter2 = delegate (Dist dist) { return dist.Name == "Area-B"; };

            var x = city.Where(filter1).Select(orgs);
            var y = city.Where(filter2).Select(orgs);
            var un = x.Union(y);
            foreach (var a in un)
            {
                foreach (Organization tekOrg in a) Console.Write(tekOrg.Name + "  ");
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
            var x = from dist in city where dist.Name == "Area-C" select dist.orgs;
            foreach (var a in x)
            {
                var orgs = from org in a orderby org select org;
                foreach (Organization org in orgs) { org.VShow(); Console.WriteLine(); }
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

            var x = city.Where(filter).Select(listItem);
            foreach(var a in x)
            {
                var y = a.OrderBy(item).Select(item);
                foreach (Organization org in y) { org.VShow(); Console.WriteLine(); }
            }
            Console.WriteLine("------------------------------------------");
            Console.WriteLine();
        }

    }
}
