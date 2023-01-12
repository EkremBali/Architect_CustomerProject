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

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Customer}/{action=CustomerList}/{id?}");
});



app.Run();
