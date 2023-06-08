using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Uniza.Namedays
{
    /// <summary>
    /// Record <c>Nameday</c> štruktúra reprezentujúca oslavu menín (meno a dátum jeho oslavy):
    /// </summary>
    public record Nameday
    {
        /// <summary>
        /// Property <c></c> vlastnosť vracajúca meno, ktoré oslavuje meniny
        /// </summary>
        public string Name {  get; init; }
        /// <summary>
        /// Property <c>DayMonth</c> vlastnosť vracajúca deň a mesiac menín
        /// </summary>
        public DayMonth DayMonth { get; init; }

        /// <summary>
        /// Method <c>Nameday</c> konštruktor ktorý inicializuje štruktúru
        /// </summary>
        public Nameday()
        {
            Name = "";
            DayMonth = new DayMonth();
        }

        /// <summary>
        /// Method <c>Nameday</c> konštruktor ktorý inicializuje štruktúru podľa zadaných parametrov
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dayMonth"></param>
        public Nameday(string name, DayMonth dayMonth)
        {
            Name = name;
            DayMonth = dayMonth;
        }
    }
}
