namespace MartenEventSourcing.Domain.Models
{
    public class Policy
    {
        public Guid Id { get; set; }
        public string? Plates { get; set; }
        public string? Insured { get; set; }
        public DateTime? RenewedAtUtc { get; set; }
        public DateTime? CanceledAtUtc { get; set; }

        public void Apply(Events.PolicyCreated created)
        {
            Plates = created.Plates;
            Insured = created.Insured;
        }
        public void Apply(Events.PolicyRenewed renewed)
        {
            RenewedAtUtc = renewed.RenewedAtUtc;
        }
        public void Apply(Events.PolicyCanceled canceled)
        {
            CanceledAtUtc = canceled.CanceledAtUtc;
        }
    }
}
