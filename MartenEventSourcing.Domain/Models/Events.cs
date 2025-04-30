namespace MartenEventSourcing.Domain.Models
{
    public class Events
    {
        public class PolicyCreated
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string? Plates { get; set; }
            public string? Insured { get; set; }
        }

        public class PolicyPlatesUpdated
        {
            public Guid Id { get; set; }
            public string? Plates{ get; set; }
        }

        public class PolicyRenewed
        {
            public Guid Id { get; set; }
            public DateTime? RenewedAtUtc { get; set; }
        }

        public class PolicyCanceled
        {
            public Guid Id { get; set; }
            public DateTime? CanceledAtUtc { get; set; }
        }
    }
}
