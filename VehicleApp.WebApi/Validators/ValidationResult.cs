namespace VehicleApp.WebApi.Validators;

public class ValidationResult
{
    public ValidationResult(bool isValid)
    {
        IsValid = isValid;
    }
    public ValidationResult(bool isValid, string errorMessage, Enums.ValidationType validationType)
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
        ValidationType = validationType;
    }

    public bool IsValid { get; set; }
    public string ErrorMessage { get; set; }
    public Enums.ValidationType ValidationType { get; set; }

}
