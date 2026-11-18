using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ViaCidada.Ouvidoria.Api.Models;

[Table("TB_VC_NET_PROPOSTAS")]
public class PropostaMelhoria
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(120, MinimumLength = 5, ErrorMessage = "O título deve ter entre 5 e 120 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "A descrição deve ter entre 10 e 500 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    [StringLength(50, ErrorMessage = "A categoria deve ter no máximo 50 caracteres.")]
    public string Categoria { get; set; } = string.Empty;

    [StringLength(30, ErrorMessage = "O código da linha ou parada deve ter no máximo 30 caracteres.")]
    public string? CodigoLinhaOuParada { get; set; }

    [Required(ErrorMessage = "O status é obrigatório.")]
    [StringLength(30, ErrorMessage = "O status deve ter no máximo 30 caracteres.")]
    public string Status { get; set; } = "RECEBIDA";

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "A região urbana é obrigatória.")]
    [Range(1, int.MaxValue, ErrorMessage = "O ID da região urbana deve ser maior que zero.")]
    public int RegiaoUrbanaId { get; set; }

    [ForeignKey(nameof(RegiaoUrbanaId))]
    [JsonIgnore]
    public RegiaoUrbana? RegiaoUrbana { get; set; }
}
