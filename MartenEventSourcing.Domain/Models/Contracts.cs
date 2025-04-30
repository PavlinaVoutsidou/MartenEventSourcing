namespace MartenEventSourcing.Domain.Models
{
    public record CreatePolicyRequest(string Plates, string Insured);
    public record UpdatePlatesRequest(string Plates);
}
