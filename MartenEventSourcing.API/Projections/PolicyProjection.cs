using Marten.Events.Aggregation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartenEventSourcing.Domain.Models
{
    public class PolicyProjection : SingleStreamProjection<Policy>
    {
        public void Apply(Events.PolicyCreated policyCreated, Policy policy)
        {
            policy.Id = policyCreated.Id;
            policy.Plates = policyCreated.Plates;
            policy.Insured = policyCreated.Insured;
        }

        public void Apply(Events.PolicyPlatesUpdated platesUpdated, Policy policy)
        {
            policy.Plates = platesUpdated.Plates;
        }

        public void Apply(Events.PolicyRenewed policyRenewed, Policy policy)
        {
            policy.RenewedAtUtc = policyRenewed.RenewedAtUtc;
        }
        public void Apply(Events.PolicyCanceled policyCanceled, Policy policy)
        {
            policy.CanceledAtUtc = policyCanceled.CanceledAtUtc;
        }
    }
}
