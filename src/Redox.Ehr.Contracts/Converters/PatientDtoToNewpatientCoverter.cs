using Microsoft.Extensions.Logging;
using Redox.Ehr.Contracts.Models.Dto;
using Redox.Ehr.Contracts.Models.Redox.Patientadmin.Newpatient;
using System.Collections.ObjectModel;

namespace Redox.Ehr.Contracts.Converters
{
    /// <summary>
    /// PatientDto to Newpatient converter
    /// </summary>
    public class PatientDtoToNewpatientCoverter : IConverter<PatientDto, Newpatient>
    {
        private readonly ILogger<PatientDtoToNewpatientCoverter> _logger;

        public PatientDtoToNewpatientCoverter(ILogger<PatientDtoToNewpatientCoverter> logger)
        {
            _logger = logger;
        }

        public Newpatient Convert(PatientDto patient)
        {
            _logger.LogDebug(nameof(Convert));

            var newPatient = new Newpatient();
            newPatient.Patient.Identifiers.Add(new Identifiers()
            {
                ID = patient.MRN,
                IDType = "MRN"
            });
            newPatient.Patient.Demographics = new Demographics
            {
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DOB = patient.BirthDate,
                Sex = patient.Sex,
                SSN = patient.SocialSecurityNumber
            };
            newPatient.Patient.Demographics.EmailAddresses = new Collection<object>
            {
                patient.Email
            };
            newPatient.Patient.Demographics.Address = new Address
            {
                StreetAddress = patient.Address.StreetAddress,
                City = patient.Address.City,
                State = patient.Address.State,
                ZIP = patient.Address.ZipCode
            };
            newPatient.Patient.Demographics.PhoneNumber = new PhoneNumber
            {
                Home = patient.HomePhone,
                Mobile = patient.MobilePhone
            };

            return newPatient;
        }
    }
}
