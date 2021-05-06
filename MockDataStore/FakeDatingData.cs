using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using ImageCallWrapper;
using FakeDatingLogic;
using FakeDatingLogic.Interfaces;
using FakeDatingLogic.Structures;

namespace MockDataStore
{
    public class FakeDatingData : IFakeDatingData
    {
        private readonly List<string> _domainLists = new()
        {
            "ourdomain.com",
            "fictionalnames.org",
            "spam.io",
            "likegmailbutnot.com",
            "justforyour.info",
            "hotweather.gov",
            "gennifersdatingagency.com",
            "nothingpoliticalhere.info",
            "californiasun.life"
        };

        private readonly List<KeyValuePair<Guid, Guid>> _matches = new();
        private readonly Dictionary<Guid, PersonProfile> _profiles = new();
        private readonly IResizeImage _resizeImage;
        private readonly IRotateImage _rotateImage;
        private readonly List<string> _usedNames = new(); //to keep names unique
        private NameLists _nameLists;
        private int _nameListSize;
        private readonly Random _random = new(DateTime.Now.Millisecond);

        public FakeDatingData()
        {
           var graphicsProvider = new SystemDrawingProvider();
           _resizeImage = graphicsProvider;
           _rotateImage = graphicsProvider;
        }
        public IEnumerable<PersonProfile> GetProfileData()
        {
            _nameLists = new NameLists();

            var profilesToAdd = ProfilesToAdd(GenderIdentityEnum.F).ToArray();
            foreach (var profile in profilesToAdd)
                _profiles.Add(profile.Id, profile);
            profilesToAdd = ProfilesToAdd(GenderIdentityEnum.M).ToArray();
            foreach (var profile in profilesToAdd)
                _profiles.Add(profile.Id, profile);
            return _profiles.Values;
        }

        public PersonProfile GetPersonProfileData(Guid id)
        {
            if (_profiles.ContainsKey(id))
                return _profiles.FirstOrDefault(a => a.Key == id).Value;
            return null;
        }

        public IEnumerable<KeyValuePair<Guid, Guid>> GetMatchData(IEnumerable<PersonProfile> profiles)
        {
            //Prevents multiple enumerations with keeping an interface as part of the signature.
            var arrayOfProfiles = profiles.ToArray();
            var arrayMaleProfileGuids = arrayOfProfiles.Where(a => a.GenderIdentity == GenderIdentityEnum.M)
                .Select(a => a.Id).ToArray();
            var arrayFemaleProfileGuids = arrayOfProfiles.Where(a => a.GenderIdentity == GenderIdentityEnum.F)
                .Select(a => a.Id).ToArray();
            var matches = new List<KeyValuePair<Guid, Guid>>();

            //Let's say each male is willing to meet 20-80% of females and that each female is willing to meet 5-35% of males.
            foreach (var profile in arrayMaleProfileGuids)
                matches.AddRange(CreateSwipesForPerson(profile, arrayFemaleProfileGuids, 20, 80));
            foreach (var profile in arrayFemaleProfileGuids)
                matches.AddRange(CreateSwipesForPerson(profile, arrayMaleProfileGuids, 5, 35));

            return matches;
        }

        public bool IsMatch(Guid profileSwiperId, Guid profileSwipeeId)
        {
            return (_matches.Contains(new KeyValuePair<Guid, Guid>(profileSwiperId, profileSwipeeId)));
        }

        public void AddOrUpdateProfile(PersonProfile profile)
        {
            if (_profiles.ContainsKey(profile.Id))
                _profiles.Remove(profile.Id);
            _profiles.Add(profile.Id, profile);
        }

        public void SetMatch(bool toggleValue, Guid profileSwiperId, Guid profileSwipeeId)
        {
            var keyValuePair = new KeyValuePair<Guid, Guid>(profileSwiperId, profileSwipeeId);

            if (IsMatch(profileSwiperId, profileSwipeeId))
                _matches.Remove(keyValuePair);

            if (toggleValue)
                 _matches.Add(keyValuePair);
        }

        private IEnumerable<KeyValuePair<Guid, Guid>> CreateSwipesForPerson(Guid initiatorProfileId,
            IEnumerable<Guid> oppositeGenderProfilesIds, int minPercentage, int maxPercentage)
        {
            var oppositeGenderProfileIdsArray = oppositeGenderProfilesIds.ToArray();
            var numberOfSwipes = oppositeGenderProfileIdsArray.Length * _random.Next(minPercentage, maxPercentage) / 100;
            var profileIndexes =
                GenerateRandomNumberCollection(numberOfSwipes, 0, oppositeGenderProfileIdsArray.Length - 1);
            return profileIndexes.Select(index =>
                new KeyValuePair<Guid, Guid>(initiatorProfileId, oppositeGenderProfileIdsArray[index])).ToList();
        }

        public IEnumerable<int> GenerateRandomNumberCollection(int numberEntries, int minValue, int maxValue)
        {
            var entries = new List<int>(numberEntries);
            while (entries.Count < numberEntries)
            {
                var randomNumber = _random.Next(minValue, maxValue);
                if (!entries.Contains(randomNumber))
                    entries.Add(randomNumber);
            }

            return entries;
        }

        private IEnumerable<PersonProfile> ProfilesToAdd(GenderIdentityEnum identityEnum)
        {
            var originLatitude = 34.063699M;
            var originLongitude = -118.358781M;
            var subDir = identityEnum == GenderIdentityEnum.M ? "Male" : "Female";
            var files = Directory.GetFiles(  $@"{Directory.GetParent(Assembly.GetExecutingAssembly().Location)}\Photos\{subDir}\");
            var profilesToAdd = new List<PersonProfile>();
            foreach (var file in files)
                CreateProfileForPictureFile(identityEnum,  originLatitude, originLongitude, file, profilesToAdd);

            return profilesToAdd;
        }

        private void CreateProfileForPictureFile(GenderIdentityEnum identityEnum,
            decimal originLatitude,
            decimal originLongitude, string file, List<PersonProfile> profilesToAdd)
        {
            var domainNameIndex = (int) Math.Truncate(_random.NextDouble() * _domainLists.Count);
            var useNullGender = _random.NextDouble() > .95;

            //Declare these two items on here so we can write code more clearly with object initialized.
            var genderIdentity = useNullGender ? GenderIdentityEnum.Empty : identityEnum;
            var fullName = GetUniqueName(genderIdentity);
            var personProfile = new PersonProfile
            {
                GenderIdentity = genderIdentity,
                FullName = fullName,
                //Keep both Lat and long within reason .45 (+/- .2225 on both lat/long) seemed to provide reasonable distances between 0-30 miles
                LatitudeDecimalDegrees = originLatitude + (decimal) (_random.NextDouble() - .5) * .45M,
                LongitudeDecimalDegrees = originLongitude + (decimal) (_random.NextDouble() - .5) * .45M,
                Image = ResizeAndRotateImageFromFilename(file),
                //awful formula, but we are using this just to seed test data.
                Email = fullName.Substring(0, 1) + fullName.Split(" ")[1] +
                        "@" + _domainLists[domainNameIndex]
            };
            profilesToAdd.Add(personProfile);
        }

        private string GetUniqueName(GenderIdentityEnum identityEnum)
        {
            var canReturn = false;
            var fullName = string.Empty;
            while (!canReturn)
            {
                var useCommonLastName = _random.NextDouble() < .8;
                _nameListSize = 100; //All names should have a list of 100 elements.
                var lastNameIndex = (int) Math.Truncate(_random.NextDouble() * _nameListSize);
                var firstNameIndex = (int) Math.Truncate(_random.NextDouble() * _nameListSize);
                var lastName = useCommonLastName
                    ? _nameLists.CommonLastNames[lastNameIndex]
                    : _nameLists.UncommonLastNames[lastNameIndex];
                var firstName = identityEnum == GenderIdentityEnum.M
                    ? _nameLists.MaleNames[firstNameIndex]
                    : _nameLists.FemaleNames[firstNameIndex];
                fullName = firstName + " " + lastName;
                canReturn = !_usedNames.Contains(fullName);
                if (canReturn) _usedNames.Add(fullName);
            }

            return fullName;
        }

        private Image ResizeAndRotateImageFromFilename(string file)
        {
            var imageData = Image.FromFile(file);
            imageData = _resizeImage.Resize(imageData, 300);
            imageData = _rotateImage.Rotate(imageData);
            return imageData;
        }
    }
}