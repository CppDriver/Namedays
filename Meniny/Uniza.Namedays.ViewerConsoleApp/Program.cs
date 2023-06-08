using System.CodeDom.Compiler;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Uniza.Namedays;

NamedayCalendar calendar = new Uniza.Namedays.NamedayCalendar();
calendar.Load(new FileInfo("namedays-sk.csv"));


bool notEnd = true;
while (notEnd)
{
    Console.Clear();
    Console.WriteLine("KALENDÁR MIEN");
    var dnesMaMeniny = calendar[DateTime.Now.Day, DateTime.Now.Month];
    var zajtraMaMeniny = calendar[DateTime.Now.AddDays(1).Day, DateTime.Now.AddDays(1).Month];
    Console.WriteLine($"Dnes {DateTime.Now.ToShortDateString()} {(dnesMaMeniny != null ? $"má meniny {string.Join(", ", dnesMaMeniny.Where(s => !string.IsNullOrEmpty(s)))}" : "nemá nikto meniny")}");
    Console.WriteLine($"Zajtra {(zajtraMaMeniny != null ? $"má meniny {string.Join(", ", zajtraMaMeniny.Where(s => !string.IsNullOrEmpty(s)))}" : "nemá nikto meniny")}");

    Console.WriteLine("Menu\n1 - načítať kalendár\n2 - zobraziť štatistiku\n3 - vyhľadať mená\n4 - vyhľadať mená podľa dátumu\n5 - zobraziť kalendár mien v mesiaci\n6 | Escape - koniec\nVyša voľba: ");
    var volba = Console.ReadKey();
    Console.Clear();
    switch (volba.Key)
    {
        case ConsoleKey.D1:
        case ConsoleKey.NumPad1:
            Console.WriteLine("OTVORENIE\nZadajte cestu k súboru kalendára mien alebo stlačte Enter pre ukončenie.");
            string? path = null;
            while (path == null || path.Length == 0)
            {
                Console.Write("Zadajte cestu k CSV súboru: ");
                path = Console.ReadLine();
                if (path?.Length == 0) break;
                if (!path!.EndsWith(".csv".ToLower()))
                {
                    Console.WriteLine($"Zadaný súbor {path} nie je typu CSV!");
                    path = null;
                    continue;
                }
                try
                {
                    calendar.Load(new FileInfo(path!));
                    Console.WriteLine("Súbor kalendára bol načítaný.\nPre pokračovanie stlačte Enter.");
                    Console.ReadLine();
                }
                catch
                {
                    Console.WriteLine($"Zadaný súbor {path} neexistuje!");
                    path = null;
                }
            }
            break;
        case ConsoleKey.D2:
        case ConsoleKey.NumPad2:
            Console.WriteLine("ŠTATISTIKA");
            Console.WriteLine($"Celkový počet mien v kalendári: {calendar.NameCount}");
            Console.WriteLine($"Celkový počet dní obsahujúci mená v kalendári: {calendar.DayCount}");
            Console.WriteLine("Celkový počet mien v jednotlivých mesiacoch:");
            string[] mesiace = { "január", "február", "marec", "apríl", "máj", "jún", "júl", "august", "september", "október", "november", "december" };
            for (int i = 0; i < 12; i++)
                Console.WriteLine($"  {mesiace[i]}: {calendar.GetNamedays(i + 1).Count()}");
            Console.WriteLine("Počet mien podľa začiatočných písmen:");
            List<string> output = new();
            for (int i = 'A'; i <= 'Ž'; i++)
            {
                int pocet = calendar.GetNamedays($"^{Regex.Escape(((char)i).ToString())}").Count();
                if (pocet > 0)
                    output.Add($"  {(char)i}: {pocet}");
            }
            output.Sort(StringComparer.CurrentCulture);
            for (int i = 0; i < output.Count; i++)
            {
                Console.WriteLine(output[i]);
            }
            Console.WriteLine("Počet mien podľa dĺžky znakov:");
            for (int i = 1; i < 32; i++)
            {
                int pocet = calendar.GetNamedays($"^.{{{i}}}$").Count();
                if (pocet > 0)
                    Console.WriteLine($"  {i}: {pocet}");
            }
                
            Console.WriteLine("Pre ukončenie stlačte Enter.");
            Console.ReadLine();
            break;
        case ConsoleKey.D3:
        case ConsoleKey.NumPad3:
            Console.WriteLine("VYHĽADÁVANIE MIEN\nPre ukončenie stlačte Enter.");
            bool continueSearch = true;
            string? input = null;
            while (continueSearch)
            {
                Console.Write("Zadajte meno (regulárny výraz): ");
                input = Console.ReadLine();
                if (input?.Length == 0)
                {
                    continueSearch = false;
                }
                else
                {
                    var names = calendar.GetNamedays(input!);
                    if (names.Count() < 0)
                    {
                        Console.WriteLine("Neboli nájdené žiadne mená.");
                    }
                    else
                    {
                        int i = 1;
                        foreach (var name in names)
                        {
                            Console.WriteLine($"  {i}. {name.Name} ({name.DayMonth.Day}.{name.DayMonth.Month})");
                            i++;
                        }
                    }
                }
            }
            break;
        case ConsoleKey.D4:
        case ConsoleKey.NumPad4:
            Console.WriteLine("VYHĽADÁVANIE MIEN PODĽA DÁTUMU\nPre ukončenie stlačte Enter.");
            continueSearch = true;
            input = null;
            while (continueSearch)
            {
                Console.Write("Zadajte deň a mesiac: ");
                input = Console.ReadLine();
                if (input?.Length == 0)
                {
                    continueSearch = false;
                    continue;
                }
                int[]? date = null;
                try
                {
                    date = Array.ConvertAll(input!.Split('.'), int.Parse);
                }catch
                {
                    Console.WriteLine("Neplatný vstup");
                    continue;
                }
                var names = calendar.GetNamedays(date[1]);
                int i = 1;
                foreach (var name in names)
                {
                    if (name.DayMonth.Day == date[0])
                    {
                        Console.WriteLine($"  {i}. {name.Name}");
                        i++;
                    }
                }
            }
            break;
        case ConsoleKey.D5:
        case ConsoleKey.NumPad5:
            continueSearch = true;
            DateTime today = DateTime.Today;
            DateTime day = new DateTime(today.Year, today.Month, 1);
            DateTime curMonth = day;
            while (continueSearch)
            {
                Console.Clear();
                Console.WriteLine("KALENDÁR MENÍN");
                today = DateTime.Today;
                curMonth = day;
                Console.WriteLine($"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(day.Month)} {day.Year}:");
                for (int i = 0; i < DateTime.DaysInMonth(curMonth.Year, curMonth.Month); i++)
                {
                    if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    if (today == day)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    Console.WriteLine($"  {day.Day}.{day.Month} {day.DayOfWeek.ToString().Substring(0,2)} {string.Join(", ",calendar[day])}");
                    Console.ForegroundColor = ConsoleColor.White;
                    day = day.AddDays(1);
                }
                day = curMonth;
                var keyPressed = Console.ReadKey();
                switch( keyPressed.Key )
                {
                    case ConsoleKey.LeftArrow:
                        day = day.AddMonths(-1);
                        break;
                    case ConsoleKey.RightArrow:
                        day = day.AddMonths(1);
                        break;
                    case ConsoleKey.UpArrow:
                        day = day.AddYears(1);
                        break;
                    case ConsoleKey.DownArrow:
                        day = day.AddYears(-1);
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.Home:
                        today = DateTime.Today;
                        day = new DateTime(today.Year, today.Month, 1);
                        break;
                    case ConsoleKey.Enter:
                        continueSearch = false;
                        break;
                    default:
                        break;
                }
            }
            break;
        case ConsoleKey.D6:
        case ConsoleKey.NumPad6:
        case ConsoleKey.Escape:
            notEnd = false;
            break;
        default:
            Console.WriteLine("Invalid input.");
            Thread.Sleep(300);
            break;
    }
}