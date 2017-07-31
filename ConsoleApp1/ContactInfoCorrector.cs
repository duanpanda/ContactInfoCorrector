using System.Collections.Generic;

namespace ConsoleApp1
{
    class ContactInfoCorrector
    {
        private List<CorrectorRule> rules = new List<CorrectorRule>();

        public void AddRule(CorrectorRule r)
        {
            rules.Add(r);
        }

        public void ApplyRules(List<ContactRecord> contacts)
        {
            foreach (ContactRecord contact in contacts)
            {
                foreach (CorrectorRule rule in rules)
                {
                    rule.ApplyOn(contact);
                }
            }
        }
    }
}
