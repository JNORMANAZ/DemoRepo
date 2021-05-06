using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using FakeDatingLogic.Calculation;
using FakeDatingLogic.Validation;

namespace FakeDatingLogic.Structures
{
    public class PersonProfile
    {
        public PersonProfile()
        {
            Id = Guid.NewGuid();
            GenderIdentity = GenderIdentityEnum.Empty;
        }

        public Guid Id { get; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public decimal LatitudeDecimalDegrees { get; set; }

        public decimal LongitudeDecimalDegrees { get; set; }

        public Image Image { get; set; }

        //This is always not null, but the user should specifically initialize it.
        public GenderIdentityEnum GenderIdentity { get; set; }

        public IEnumerable<ValidationResult> Validate()
        {
            var validationResults = new List<ValidationResult>();
        
            var emailValidationResults = EmailValidator.ValidateEmailAddress(Email).ToArray(); //May have no items but will not be null
            if (emailValidationResults!.Any()) validationResults.AddRange(emailValidationResults);

            if (DecimalMath.Abs(LongitudeDecimalDegrees) > 180)
                validationResults.Add(new ValidationResult(
                    $"{nameof(LongitudeDecimalDegrees)} is out of range.  Max value is +/-180, value provided was {LongitudeDecimalDegrees}."));
            if (DecimalMath.Abs(LatitudeDecimalDegrees) > 90)
                validationResults.Add(new ValidationResult(
                    $"{nameof(LatitudeDecimalDegrees)} is out of range.  Max value is +/-90, value provided was {LatitudeDecimalDegrees}."));

            if (GenderIdentity == GenderIdentityEnum.Empty)
                validationResults.Add(new ValidationResult($"{nameof(GenderIdentity)} has not been initialized."));
            if (string.IsNullOrWhiteSpace(FullName))
                validationResults.Add(new ValidationResult("Full Name must be filled in."));
            else
            {
                if (FullName.Length < 3)
                    validationResults.Add(new ValidationResult("Full Name must be at least three characters."));
                if (FullName.Length > 100)
                    validationResults.Add(new ValidationResult("Full Name must be less than 100 characters."));
            }

            return validationResults;
        }

        //Since this is a calculation on an object, tempted to put it in extension
        // method.  But it's also core functionality of the system, so decided
        // it should go here instead.
        public decimal DistanceFrom(PersonProfile otherPerson)
        {
            return DistanceFrom(otherPerson.LatitudeDecimalDegrees, otherPerson.LongitudeDecimalDegrees);
        }

        public decimal DistanceFrom(decimal latitudeInDegrees, decimal longitudeInDegrees)
        {
            if (DecimalMath.Abs(latitudeInDegrees) > 90)
                throw new ArgumentOutOfRangeException($"{nameof(latitudeInDegrees)}",
                    "Maximum value of latitude is +/- 90");
            if (DecimalMath.Abs(longitudeInDegrees) > 180)
                throw new ArgumentOutOfRangeException($"{nameof(longitudeInDegrees)}",
                    "Maximum value of longitude is +/- 180");
            return DistanceMath.GetDistanceFromLatLonInMiles(LatitudeDecimalDegrees, LongitudeDecimalDegrees,
                latitudeInDegrees, longitudeInDegrees);
        }
    }
}