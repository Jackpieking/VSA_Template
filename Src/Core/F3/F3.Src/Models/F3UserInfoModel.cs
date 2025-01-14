namespace F3.Src.Models;

public sealed class F3UserInfoModel
{
    public long Id { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public bool EmailConfirmed { get; set; }

    public AdditionalUserInfoModel AdditionalUserInfo { get; set; }

    public sealed class AdditionalUserInfoModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Description { get; set; }
    }
}
