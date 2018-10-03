using System;
using System.ComponentModel.DataAnnotations;

namespace Ecard.Mvc.Models
{
    public class DataRange
    {
        [DataType(DataType.Date)]
        public DateTime? Start { get; set; }
        [DataType(DataType.Date)]
        public DateTime? End { get; set; }
        public DateTime GetEnd()
        {
            return (End ?? DateTime.Now.Date).Date.AddDays(1);
        }
    }
}