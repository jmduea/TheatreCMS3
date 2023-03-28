using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheatreCMS3.Areas.Prod.Models
{
    public class CalendarEvent
    {
        [Key][Required]
        public int EventID { get; set; }
        [Required]
        public string Title { get; set; }

        [DataType(DataType.Date)][Required]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)][Required]
        public DateTime EndDate { get; set; }

        [DataType(DataType.Time)]
        public DateTime? StartTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime? EndTime { get; set; }

        [Required]
        public bool AllDay { get; set; }

        [Required]
        public int TicketsAvailable { get; set; }

        [Required]
        public bool IsProduction { get; set; }

        [Required]
        public string Description { get; set; }
    }
}