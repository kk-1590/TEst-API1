namespace AdvanceAPI.SQLConstants;

public class GeneralSql
{
    public const string GET_CAMPUS_NAME="SELECT CampusName from gla_student_management.campus_master WHERE CampusCode=@CampusCode";
    
    public const string CECK_ALLOWED_COLUMN="Select * from userroles where employee_code=@EmpCode And @ColumnName is not null And ((CAST(@ColumnName as CHAR)='1') or (CAST(@ColumnName as CHAR) ='Yes'))";

    public const string GETEMPNAME =
        "SELECT first_name FROM salary_management.emp_master WHERE employee_code=@EmpCode\n";
}