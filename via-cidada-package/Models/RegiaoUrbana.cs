using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ViaCidada.Ouvidoria.Api.Models;

[Table("TB_VC_NET_REGIOES")]
public class RegiaoUrbana
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome da região é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "A zona da cidade é obrigatória.")]
    [StringLength(40, ErrorMessage = "A zona da cidade deve ter no máximo 40 caracteres.")]
    public string ZonaCidade { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "A subprefeitura deve ter no máximo 100 caracteres.")]
    public string? Subprefeitura { get; set; }

    [JsonIgnore]
    public ICollection<PropostaMelhoria> Propostas { get; set; } = new List<PropostaMelhoria>();
}
