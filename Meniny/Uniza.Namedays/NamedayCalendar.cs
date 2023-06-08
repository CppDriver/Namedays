using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Uniza.Namedays
{
    /// <summary>
    /// Class <c>NamedayCalendar</c> trieda reprezentujúca kalendár menín
    /// </summary>
    public record class NamedayCalendar : IEnumerable<Nameday>
    {
        private List<Nameday> namedays = new List<Nameday>();
        /// <summary>
        /// Property <c>NameCount</c> vlastnosť len na čítanie, vráti celkový počet všetkých mien
        /// </summary>
        public int NameCount { get { return namedays.Count(); } }
        /// <summary>
        /// Property <c>DayCount</c> vlastnosť len na čítanie, vráti celkový počet všetkých dní v roku, v ktorých má niekto meniny
        /// </summary>
        public int DayCount { get { return (from nameday in namedays select nameday.DayMonth).Distinct().Count(); } }
        /// <summary>
        /// indexer vráti deň a mesiac oslavy zadaného mena (name), ak bolo meno nájdene.Ak nebolo meno v kalendári mien nájdené, vráti hodnotu null.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>int</returns>
        public DayMonth? this[string name] { get { var x = namedays.Find(a => a.Name == name);
                return x == null ? null : x.DayMonth; } }
        /// <summary>
        /// indexe vráti pole reťazcov reprezentujúce mená podľa zadaného dátumu
        /// </summary>
        /// <param name="dayMonth"></param>
        /// <returns>DayMonth?</returns>
        public string[] this[DayMonth dayMonth] { get { var dm = namedays.FindAll(d => d.DayMonth == dayMonth);
                var output = new string[dm.Count];
                for (int i = 0; i < dm.Count; i++)
                {
                    output[i] = dm[i].Name;
                }
                return output;
            } }
        /// <summary>
        /// indexe vráti pole reťazcov reprezentujúce mená podľa zadaného dátumu
        /// </summary>
        /// <param name="date"></param>
        /// <returns>string[]</returns>
        public string[] this[DateOnly date] { get { return this[new DayMonth(date.Day, date.Month)]; } }
        /// <summary>
        /// indexe vráti pole reťazcov reprezentujúce mená podľa zadaného dátumu
        /// </summary>
        /// <param name="date"></param>
        /// <returns>string[]</returns>
        public string[] this[DateTime date] { get { return this[new DayMonth(date.Day, date.Month)]; } }
        /// <summary>
        /// indexe vráti pole reťazcov reprezentujúce mená podľa zadaného dátumu
        /// </summary>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <returns>string[]</returns>
        public string[] this[int day, int month] { get { return this[new DayMonth(day, month)]; } }
        /// <summary>
        /// Method <c>GetEnumerator</c> implicitne implementovaná metóda z generického rozhrania IEnumerable<Nameday>, vráti objekt implementujúci IEnumerator<Nameday> vracajúci všetky meniny v kalendári
        /// </summary>
        /// <returns>IEnumerator<Nameday></returns>
        public IEnumerator<Nameday> GetEnumerator()
        {
            return namedays.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        /// <summary>
        /// Method <c>GetNamedays</c> metóda vráti všetky meniny v kalendári
        /// </summary>
        /// <returns>IEnumberable<Nameday></returns>
        public IEnumerable<Nameday> GetNamedays()
        {
            return namedays;
        }
        /// <summary>
        /// Method <c>GetNamedays</c> metóda vráti všetky meniny v zadanom mesiaci
        /// </summary>
        /// <param name="month"></param>
        /// <returns>IEnumerable<Nameday></returns>
        public IEnumerable<Nameday> GetNamedays(int month)
        {
            return namedays.FindAll(a => a.DayMonth.Month == month);
        }
        /// <summary>
        /// Method <c>GetNamedays</c> metóda vráti všetky meniny, ktoré zodpovedajú zadanému reťazcu regulárneho výrazu(pattern), ktorý sa bude aplikovať na mená v kalendári
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns>IEnumerable<Nameday></returns>
        public IEnumerable<Nameday> GetNamedays(string pattern)
        {
            return namedays.FindAll(a => Regex.IsMatch(a.Name, pattern));
        }
        /// <summary>
        /// Method <c>Add</c> metóda pridá meniny do kalendára
        /// </summary>
        /// <param name="nameday"></param>
        public void Add(Nameday nameday)
        {
            namedays.Add(nameday);
        }
        /// <summary>
        /// Method <c>Add</c> metóda pridá jedno alebo viacero mien so zadaným dňom a mesiacom oslavy do kalendára
        /// </summary>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="names"></param>
        public void Add(int day, int month, params string[] names)
        {
            foreach (string name in names) { namedays.Add(new Nameday(name, new DayMonth(day, month))); }
        }
        /// <summary>
        /// Method <c>Add</c> – metóda pridá jedno alebo viacero mien so zadaným dňom a mesiacom oslavy do kalendára
        /// </summary>
        /// <param name="dayMonth"></param>
        /// <param name="names"></param>
        public void Add(DayMonth dayMonth, params string[] names)
        {
            foreach (string name in names) { namedays.Add(new Nameday(name, dayMonth)); }
        }
        /// <summary>
        /// Method <c>Remove</c> metóda odstráni meno z kalendára mien. Ak ho nájde a odstráni, vráti hodnotu true. Ak ho nenájde, nevyhodí žiadnu výnimku, ale vráti hodnotu false
        /// </summary>
        /// <param name="name"></param>
        /// <returns>bool</returns>
        public bool Remove(string name)
        {
            var temp = namedays.Find(a => a.Name == name);
            if (temp != null)
            {
                namedays.Remove(temp);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Method <c>Contains</c> metóda vráti true, ak zadané meno v kalendári existuje. Ak neexistuje, vráti hodnotu false
        /// </summary>
        /// <param name="name"></param>
        /// <returns>bool</returns>
        public bool Contains(string name)
        {
            return namedays.Find(a => a.Name == name) != null ? true : false;
        }
        /// <summary>
        /// Method <c>Clear</c> metóda vymaže všetky údaje z kalendára
        /// </summary>
        public void Clear()
        {
            namedays.Clear();
        }
        /// <summary>
        /// Method <c>Load</c> metóda načíta kalendár mien zo súboru s príponou CSV
        /// </summary>
        /// <param name="csvFile"></param>
        public void Load(FileInfo csvFile)
        {
            var lines = File.ReadLines(csvFile.FullName);
            foreach (var line in lines)
            {
                var content = line.Trim().Split(';');
                DayMonth dayMonth = ParseDayMonth(content[0]);
                for (int i = 1; i < content.Length; i++)
                {
                    if (content[i] != null && content[i] != string.Empty && !content[i].Contains("-"))
                        namedays.Add(new Nameday(content[i], dayMonth));
                }
            }
        }
        /// <summary>
        /// Method <c>Save</c> metóda zapíše kalendár mien do súboru s príponou CSV
        /// </summary>
        /// <param name="csvFile"></param>
        public void Save(FileInfo csvFile)
        {
            string[] lines = new string[366];
            for (int i = 0; i < lines.Length; i++)
            {
                var date = new DateTime(2020, 1, 1).AddDays(i);
                var names = this[new DayMonth(date.Day, date.Month)];
                lines[i] = $"{date.ToString("d. M.")};{(names[0] != null ? names[0] : "-")};{(names[1] != null ? names[1] : "")};{(names[2] != null ? names[2] : "")}";
            }
            File.WriteAllLines(csvFile.FullName, lines);
        }

        private DayMonth ParseDayMonth(string input)
        {
            int[] date = input.Split(new char[] { '.', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            return new DayMonth(date[0], date[1]);
        }

    }
}