using CustomerProject.Business.Abstract;
using CustomerProject.Business.Concrete;
using CustomerProject.Data.Abstract;
using CustomerProject.Data.Concrete;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultSQLConnection");
builder.Services.AddDbContext<CustomerProjectDbContext>(db => db.UseSqlServer(connectionString));

builder.Services.AddScoped<ICustomerRepository, EFCoreCustomerRepository>();
builder.Services.AddScoped<IContactRepository, EFCoreContactRepository>();
builder.Services.AddScoped<IAddressRepository, EFCoreAddressRepository>();

builder.Services.AddScoped<IAddressService, AddressManager>();
builder.Services.AddScoped<IContactService, ContactManager>();
builder.Services.AddScoped<ICustomerService, CustomerManager>();

builder.Services.AddControllers().AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

string MyAllowOrigins = "_myAllowOrigins";

//Front-End uygulamalarýn Http req ile service ulaþabilmesi için eriþim izni veridik.Aþaðýdaki useCourse'da bunun parçasý.
builder.Services.AddCors(options =>
{
	options.AddPolicy(
		name: MyAllowOrigins,
		builder =>
		{
			//Bütün talepler karþýlanýr.
			builder.AllowAnyOrigin()
				   .AllowAnyHeader()
				   .AllowAnyMethod();
		}
	);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
