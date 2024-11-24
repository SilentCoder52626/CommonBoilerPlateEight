namespace CommonBoilerPlateEight.Domain.Helper;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool ValidatePassword(string password, string hashedPassword);
}
