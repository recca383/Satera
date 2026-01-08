using ExcelToSQLiteConverter.Data;
using Microsoft.EntityFrameworkCore;

public interface IAppDbContext
{
    DbSet<App_Category> App_Categories { get; set; }
}