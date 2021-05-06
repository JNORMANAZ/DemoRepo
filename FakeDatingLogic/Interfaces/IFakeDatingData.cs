using System;
using System.Collections.Generic;
using FakeDatingLogic.Structures;

namespace FakeDatingLogic.Interfaces
{
    //Our substitute for a repository as it is very specific.
    public interface IFakeDatingData
    {
        public IEnumerable<PersonProfile> GetProfileData();
        public IEnumerable<KeyValuePair<Guid, Guid>> GetMatchData(IEnumerable<PersonProfile> profiles);
        public PersonProfile GetPersonProfileData(Guid id);
        public bool IsMatch(Guid profileSwiperId, Guid profileSwipeeId);
        public void AddOrUpdateProfile(PersonProfile profile);
        public void SetMatch(bool toggleValue, Guid profileSwiperId, Guid profileSwipeeId);
    }
}
