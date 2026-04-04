public interface ICompanyService
{
    Task<string> Create(CompanyDto dto);
}