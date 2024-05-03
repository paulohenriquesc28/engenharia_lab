using System.ComponentModel.DataAnnotations;

namespace TempMotoWeb.Models
{
    public class Medicao
    {
        public int Id { get; set; }
        public float Temperatura { get; set; }
        public float Ph { get; set; }

        [Display(Name = "Data")]
        [DisplayFormat(DataFormatString = "{0: HH:mm dd/MM/yyyy}")]
        public DateTime? Data_Medicao { get; set; } = DateTime.Now.AddHours(-3);
    }
}
