namespace AdvanceAPI.SQLConstants.Inclusive
{
    public static class InclusiveSql
    {
        public const string GET_ALLOWED_CAMPUS = "SELECT A.CampusCode,B.CampusName from gla_student_management.employee_campus_master A, gla_student_management.campus_master B WHERE A.CampusCode=B.CampusCode AND A.EmployeeCode=@EmployeeCode AND A.ApplicationType='FINANCE' AND B.IsActive=1 ORDER BY A.CampusCode;";
    }
}
