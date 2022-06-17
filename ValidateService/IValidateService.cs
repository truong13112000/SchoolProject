using SchoolProject.Dto;
using SchoolProject.Models;

namespace SchoolProject.ValidateService
{
    public interface IValidateService
    {
         int ValidateSigUp(SignUpDto input);
         bool ValidateLogin(LoginDto input);
         int ValidateCreateClass(ClassEntities classEntities);
         int ValidateUpdateClass(long id, ClassEntities classEntities);
         bool ValidateCreateDeparment(DepartmentEntities departmentEntities);
         bool ValidateUpdateDeparment(long id, DepartmentEntities departmentEntities);
         bool ValidateCreateSchool(SchoolEntities schoolEntities);
         bool ValidateUpdateSchool(long id, SchoolEntities schoolEntities);
         int ValidateUpdateUser(long id, UserEntities userEntities);
    }
}