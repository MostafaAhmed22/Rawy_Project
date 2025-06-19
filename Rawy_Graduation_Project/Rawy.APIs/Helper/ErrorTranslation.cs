namespace Rawy.APIs.Helper
{
	public class ErrorTranslation
	{
		private static readonly Dictionary<string, string> ErrorTranslations = new()
		{
			{ "DuplicateUserName", "اسم المستخدم موجود بالفعل" },
	{ "DuplicateEmail", "البريد الإلكتروني مستخدم بالفعل" },
	{ "PasswordTooShort", "كلمة المرور قصيرة جداً" },
	{ "PasswordRequiresNonAlphanumeric", "يجب أن تحتوي كلمة المرور على رمز خاص" },
	{ "PasswordRequiresDigit", "يجب أن تحتوي كلمة المرور على رقم" },
	{ "PasswordRequiresUpper", "يجب أن تحتوي كلمة المرور على حرف كبير" },
	{ "PasswordRequiresLower", "يجب أن تحتوي كلمة المرور على حرف صغير" },
	{ "InvalidEmail", "البريد الإلكتروني غير صالح" },
	{ "InvalidUserName", "اسم المستخدم غير صالح" },
			{ "Passwords must have at least one non alphanumeric character.", "يجب أن تحتوي كلمة المرور على رمز غير حرفي." },
			{ "Passwords must have at least one digit ('0'-'9').", "يجب أن تحتوي كلمة المرور على رقم واحد على الأقل." },
			{ "Passwords must have at least one uppercase ('A'-'Z').", "يجب أن تحتوي كلمة المرور على حرف كبير واحد على الأقل." },
			{ "Passwords must have at least one lowercase ('a'-'z').", "يجب أن تحتوي كلمة المرور على حرف صغير واحد على الأقل." },
			{ "The Password and Confirmation Password do not match.", "كلمة المرور وتأكيد كلمة المرور غير متطابقين." },
			{ "Email 'x@x.com' is already taken.", "البريد الإلكتروني مستخدم بالفعل." },
			{ "Username 'x' is already taken.", "اسم المستخدم مستخدم بالفعل." },
            // Add more mappings as needed
        };

		public static string Translate(string error)
		{
			foreach (var pair in ErrorTranslations)
			{
				if (error.Contains(pair.Key))
					return pair.Value;
			}

			// If no match, return original (or a default Arabic fallback)
			return "حدث خطأ غير معروف.";
		}
	}
}
