using System.ComponentModel.DataAnnotations.Schema;

namespace encdec
{
    public class Employee
    {
        public int Id { get; set; }

        [NotMapped]
        public string DisplayId { get; set; }
        public string FullName { get; set; }
    }
}

