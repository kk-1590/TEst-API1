namespace AdvanceAPI.SQL.Account
{
    public static class AccountSql
    {
        public const string LOGIN_ACCOUNT_STATUS_CHECK = @"select A.*,B.first_name,deisgnation,B.currentstatus from user_login A, emp_master B  where A.Employee_Code = @Employee_Code And A.Employee_Code=B.employee_code";

        public const string GET_MAIN_PASSWORD = "select * from main";

        public const string ADVANCE_ACCESS_CHECK = "select role,myroles,myagainstroles,applicationupload from advances.userroles where employee_code=@employee_code";

        public const string GET_ADDITIONAL_EMPLOYEE_CODE = "Select UserFrom from advances.user_roles_delegate where UserTo=@employee_code And `Status`='Active' And DateFrom is not NULL And DateFrom<=DATE(now()) And DATE(now())<=IFNULL(DateTill,now())";

        public const string ADD_TOKEN = " INSERT INTO login_tokens (UserId, Name, Role, Token, TokenExpiresAt, RefreshToken, RefreshTokenExpiresAt, TokenGeneratedOn, TokenGeneratedFrom,GeneratedAgainstRefreshToken) VALUES (@UserId, @Name, @Role, @Token, @TokenExpiresAt, @RefreshToken, @RefreshTokenExpiresAt, NOW(), @TokenGeneratedFrom,@GeneratedAgainstRefreshToken);";

        public const string VALIDATE_TOKEN = " SELECT TokenId FROM login_tokens WHERE UserId=@UserId AND Token=@Token AND RefreshToken=@RefreshToken AND RefreshTokenExpiresAt>NOW() AND RefreshTokenUseStatus=0;";

        public const string USE_TOKEN = "   UPDATE login_tokens SET RefreshTokenUseStatus=1,RefreshTokenUseOn=NOW(),RefreshTokenUseFrom=@RefreshTokenUseFrom WHERE TokenId=@TokenId;";

    }
}
