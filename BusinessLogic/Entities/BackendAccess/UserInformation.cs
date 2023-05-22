using JetBrains.Annotations;

namespace BusinessLogic.Entities.BackendAccess;

public class UserInformation
{
    /// <summary>
    ///     Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    private UserInformation()
    {
        LmsUsername = "";
        IsLmsAdmin = false;
        LmsId = 0;
        LmsEmail = "";
    }

    public UserInformation(string lmsUsername, bool isLmsAdmin, int lmsId, string lmsEmail)
    {
        LmsUsername = lmsUsername;
        IsLmsAdmin = isLmsAdmin;
        LmsId = lmsId;
        LmsEmail = lmsEmail;
    }

    public string LmsUsername { get; set; }
    public bool IsLmsAdmin { get; set; }
    public int LmsId { get; set; }
    public string LmsEmail { get; set; }
}