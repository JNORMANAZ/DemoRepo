using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using FakeDatingLogic.Interfaces;
using MockDataStore;


namespace TestMockDataStore
{
    internal class Program
    {
        private static readonly string outputFolder = Path.GetTempPath() + @"\TestMockDataStore\";

        private static void Main(string[] args)
        {
            var minValueMilesAway = decimal.MaxValue;
            var maxValueMilesAway = 0M;
            IFakeDatingData fakeDatingData = new FakeDatingData();
            var profiles = fakeDatingData.GetProfileData().ToArray();
            var matches = fakeDatingData.GetMatchData(profiles).ToArray();
            if (Directory.Exists(outputFolder))
                Directory.Delete(outputFolder, true);
            Directory.CreateDirectory(outputFolder);

            var firstProfile = profiles.First();
            foreach (var profile in profiles)
            {
                var fileName = $"{outputFolder}\\" + profile.FullName.Replace(" ", "_") + ".png";
                var fileStream = File.Create(fileName);
                var profileDistanceFrom = profile.DistanceFrom(firstProfile);
                if (profileDistanceFrom > 0)
                {
                    if (profileDistanceFrom < minValueMilesAway)
                        minValueMilesAway = profileDistanceFrom;
                    if (profileDistanceFrom > maxValueMilesAway)
                        maxValueMilesAway = profileDistanceFrom;
                }

                profile.Image.Save(fileStream, ImageFormat.Png);
                fileStream.Dispose();
                var peopleChosenIds = matches.Where(a => a.Key == profile.Id).Select(a => a.Value).ToArray();
                var peopleChosenByIds = matches.Where(a => a.Value == profile.Id).Select(a => a.Key).ToArray();
                var mutualMatches = peopleChosenByIds.Intersect(peopleChosenIds).ToArray();
                //allows us to manually example stats to see if the work was done.
                WriteOutputLine(
                    $"{profile.Id} {profile.FullName} {profile.Email} {profile.GenderIdentity} {profileDistanceFrom} Number chosen: {peopleChosenIds.Length} Number Chosen By: {peopleChosenByIds.Length} Matches: {mutualMatches.Length}");
            }

            WriteOutputLine($"Minimum miles away from first profile: {minValueMilesAway}");
            WriteOutputLine($"Maximum miles away from first profile: {maxValueMilesAway}");
        }

        private static void WriteOutputLine(string s)
        {
            Console.WriteLine(s);
        }
    }
}