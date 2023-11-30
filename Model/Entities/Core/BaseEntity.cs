using System.ComponentModel.DataAnnotations;

namespace Model.Entities.Core
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public bool IsSoftDel { get; set; } = false;

    }
}
