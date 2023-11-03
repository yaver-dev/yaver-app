using Yaver.Db;

namespace Admin.Service.Features.Tenants.Entities;

//   -i playland
//   -b "plylnd"
//   -n "Playland A.S."
//   -o "Hasan Hussein"
//   -e "info@falanca.com"
//   -p "+905321234567"
//   -a "Cilginca Mah., Eglence Sok. No:1, Allem Ahkam AVM. / İstanbul"
public class Tenant(
  string identifier,
  string brand,
  string name,
  string authorizedPerson,
  string email,
  string phoneNumber,
  string address,
  string logoUrl,
  string connectionString,
  Guid defaultBranch,
  string cardKey) : AuditableEntity {
  public required string Identifier { get; set; } = identifier;
  public required string Brand { get; set; } = brand;
  public required string Name { get; set; } = name;
  public required string AuthorizedPerson { get; set; } = authorizedPerson;
  public required string Email { get; set; } = email;
  public required string PhoneNumber { get; set; } = phoneNumber;
  public required string Address { get; set; } = address;
  public required string LogoUrl { get; set; } = logoUrl;
  public required string ConnectionString { get; set; } = connectionString;
  public Guid DefaultBranch { get; set; } = defaultBranch;
  public required string CardKey { get; set; } = cardKey;
}
