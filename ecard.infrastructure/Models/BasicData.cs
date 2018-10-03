using System.ComponentModel.DataAnnotations;

namespace Ecard.Models
{
    /// <summary>
    /// This object represents the properties and methods of a BasicData.
    /// </summary>
    public class BasicData
    {
        public BasicData()
        {
            State = 1;
            Code = string.Empty;
            Text = string.Empty;
        }
        [Key]
        public int BasicDataId { get; set; }

        public string Code { get; set; }

        public string Text { get; set; }

        public int Value { get; set; }
        public int State { get; set; }
    }
}