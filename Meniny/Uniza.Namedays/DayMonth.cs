using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniza.Namedays
{
    /// <summary>
    /// Record <c>DayMonth</c> štruktúra reprezentujúca deň a mesiac
    /// </summary>
    public record DayMonth
    {
        /// <summary>
        /// Property <c>Day</c> vlastnosť vracajúca deň
        /// </summary>
        public int Day { get; init; }
        /// <summary>
        /// Property <c>Month</c> vlastnosť vracajúca mesiac
        /// </summary>
        public int Month { get; init; }

        /// <summary>
        /// Method <c>DayMonth</c> konštruktor inicializujúci štruktúru
        /// </summary>
        public DayMonth() {}
        /// <summary>
        /// Method <c>DayMonth</c> koštruktor inicializujúci štruktúru podľa zadaných parametrov
        /// </summary>
        /// <param name="day"></param>
        /// <param name="month"></param>
        public DayMonth(int day, int month)
        {
            Day = day;
            Month = month;
        }

        /// <summary>
        /// Method <c>ToDateTime</c> metóda, ktorá vracia štruktúru DateTime nastavenú na aktuálny rok, pričom deň a mesiac nastaví podľa hodnôt vlastnosti DayMonth
        /// </summary>
        /// <returns>DateTime</returns>
        public DateTime ToDateTime()
        {
            return new DateTime(DateTime.Now.Year, Month, Day);
        }
    }
}
