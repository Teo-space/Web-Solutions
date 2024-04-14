public partial class Results
{
	public static Result<T> InputValidationError<T>(string Detail)
		=> Errors<T>("InputValidationError", Detail, Empty);


	public static Result<T> InputValidationErrors<T>(FluentValidation.Results.ValidationResult result)
	{
		var errors = result.Errors.Select(error => new FieldError
		{
			FieldName = error.PropertyName,
			ErrorMessages = new List<string> { $"Property : {error.PropertyName}, With error : {error.ErrorMessage}, InputValue: {error.AttemptedValue}" }
		}).ToArray();

		return Errors<T>("InputValidationErrors", $"One or more validation errors during execution", errors);
	}

}
