using System.ComponentModel;

namespace FakeDatingLogic.Structures
{
    //Specific M/F genders were a customer requirement and not a political statement.
    public enum GenderIdentityEnum
    {
        [Description("Needs Initialization")]
        Empty,
        [Description("Male")]
        M,
        [Description("Female")]
        F
    }
}