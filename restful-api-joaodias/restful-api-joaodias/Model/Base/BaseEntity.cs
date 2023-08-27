using System.ComponentModel.DataAnnotations.Schema;

namespace restful_api_joaodias.Model.Base
{
    public class BaseEntity
    {
        [Column("id")]
        public long Id { get; set; }
    }
}
