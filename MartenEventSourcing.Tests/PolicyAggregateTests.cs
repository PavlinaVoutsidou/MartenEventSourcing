namespace MartenEventSourcing.Tests
{
    using System;
    using System.Collections.Generic;
    using MartenEventSourcing.Domain.Models;
    using Xunit;

    public class PolicyAggregateTests
    {
        [Fact]
        public void ApplyPolicyCreated_SetsProperties()
        {
            var policy = new Policy();
            var createdEvent = new Events.PolicyCreated
            {
                Id = Guid.NewGuid(),
                Plates = "XYZ123",
                Insured = "John Doe"
            };

            policy.Apply(createdEvent);

            Assert.Equal(createdEvent.Id, policy.Id);
            Assert.Equal("XYZ123", policy.Plates);
            Assert.Equal("John Doe", policy.Insured);
            Assert.Null(policy.RenewedAtUtc);
            Assert.Null(policy.CanceledAtUtc);
        }

        [Fact]
        public void ApplyPolicyRenewed_SetsRenewedDate()
        {
            var policy = new Policy();
            var renewedEvent = new Events.PolicyRenewed
            {
                Id = Guid.NewGuid(),
                RenewedAtUtc = DateTime.UtcNow
            };

            policy.Apply(renewedEvent);

            Assert.Equal(renewedEvent.RenewedAtUtc, policy.RenewedAtUtc);
        }

        [Fact]
        public void ApplyPolicyCanceled_SetsCanceledDate()
        {
            var policy = new Policy();
            var canceledEvent = new Events.PolicyCanceled
            {
                Id = Guid.NewGuid(),
                CanceledAtUtc = DateTime.UtcNow
            };

            policy.Apply(canceledEvent);

            Assert.Equal(canceledEvent.CanceledAtUtc, policy.CanceledAtUtc);
        }

        [Fact]
        public void RehydratePolicy_FromEventStream()
        {
            var policyId = Guid.NewGuid();

            var events = new List<object>
        {
            new Events.PolicyCreated { Id = policyId, Plates = "ABC123", Insured = "Alice" },
            new Events.PolicyRenewed { Id = policyId, RenewedAtUtc = DateTime.UtcNow.AddDays(-1) },
            new Events.PolicyCanceled { Id = policyId, CanceledAtUtc = DateTime.UtcNow }
        };

            var policy = new Policy();

            foreach (var @event in events)
            {
                switch (@event)
                {
                    case Events.PolicyCreated pc:
                        policy.Apply(pc);
                        break;
                    case Events.PolicyRenewed pr:
                        policy.Apply(pr);
                        break;
                    case Events.PolicyCanceled pcan:
                        policy.Apply(pcan);
                        break;
                }
            }

            Assert.Equal(policyId, policy.Id);
            Assert.Equal("ABC123", policy.Plates);
            Assert.Equal("Alice", policy.Insured);
            Assert.NotNull(policy.RenewedAtUtc);
            Assert.NotNull(policy.CanceledAtUtc);
        }
    }

}