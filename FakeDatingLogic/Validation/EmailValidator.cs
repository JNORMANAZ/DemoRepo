using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace FakeDatingLogic.Validation
{
    public static class EmailValidator
    {
        public static IEnumerable<ValidationResult> ValidateEmailAddress(string emailAddress)
        {
            var validationResults = new List<ValidationResult>();

            //adapted and then extended from https://stackoverflow.com/questions/5342375/regex-email-validation
            try
            {
                var m = new MailAddress(emailAddress);
                //since we know it's not null or empty at this point.  Catch the corner cases.
                if (emailAddress.Contains(".."))
                    throw new FormatException(
                        "Email address cannot contain two sequential dots.  If this is a GMail address, please simply remove any dots in the email address before the at sign.");
                if (emailAddress.StartsWith(" "))
                    throw new FormatException("Email address cannot start with a space");
                if (emailAddress.EndsWith(" "))
                    throw new FormatException("Email address cannot end with a space");
                if (emailAddress.Length < 7)
                    throw new FormatException("Email address length must be at least seven characters.");
                if (emailAddress.Length > 100)
                    throw new FormatException("Email address length may not be more than 100 characters.");
                var domain = m.Host;
            }
            catch (ArgumentNullException) //null
            {
                validationResults.Add(new ValidationResult("Email address must be filled in"));
            }
            catch (ArgumentException) //empty
            {
                validationResults.Add(new ValidationResult("Email address must be filled in"));
            }
            catch (FormatException)
            {
                validationResults.Add(new ValidationResult("Email address is not in a valid format."));
            }

            //I take "production-ready" seriously, but admit some things can be done in phases.  Just to show you I gave it some thought,
            // but validating TLDs. https://data.iana.org/TLD/tlds-alpha-by-domain.txt

            return validationResults;
        }
    }
}