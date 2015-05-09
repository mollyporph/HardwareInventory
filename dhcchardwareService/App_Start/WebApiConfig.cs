using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using dhcchardwareService.DataObjects;
using dhcchardwareService.Models;
using dhcchardwareService.Utils;
using Microsoft.WindowsAzure.Mobile.Service.Security.Providers;
namespace dhcchardwareService
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            ConfigOptions options = new ConfigOptions();

            options.LoginProviders.Remove(typeof(AzureActiveDirectoryLoginProvider));
            options.LoginProviders.Add(typeof(CustomLoginProvider));
            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));
            config.SetIsHosted(true);

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            // config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            //
            Database.SetInitializer(new dhcchardwareInitializer());

            //Lets never touch this again..
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<LoanItem, LoanItemDTO>()
                    .ForMember(loanItemDTO => loanItemDTO.Item, map => map.MapFrom(loanItem => loanItem.Item));
                cfg.CreateMap<LoanItemDTO, LoanItem>()
                    .ForMember(loanItem => loanItem.Item, map => map.MapFrom(loanItemDTO => loanItemDTO.Item));

                cfg.CreateMap<HardwareItem, HardwareItemDTO>();
                cfg.CreateMap<HardwareItemDTO, HardwareItem>();
            });
        }
    }

    public class dhcchardwareInitializer : ClearDatabaseSchemaAlways<dhcchardwareContext>
    {
        protected override void Seed(dhcchardwareContext context)
        {
            var registrationRequest = new RegistrationRequest
            {
                username = "test",
                password = "TestingStuff"
            };
            byte[] salt = CustomLoginProviderUtils.generateSalt();
            Account newAccount = new Account
            {
                Id = Guid.NewGuid().ToString(),
                Username = registrationRequest.username,
                Salt = salt,
                SaltedAndHashedPassword = CustomLoginProviderUtils.hash(registrationRequest.password, salt)
            };
            context.Accounts.Add(newAccount);
            context.SaveChanges();

            var hwItemPersistedTPKabel = context.Set<HardwareItemDTO>().Add(new HardwareItemDTO
            {
                Id = Guid.NewGuid().ToString(),
                Name = "TP Kabel",
                ImageUrl = "Something"
            });
            var hwItemPersistedScreen = context.Set<HardwareItemDTO>().Add(new HardwareItemDTO
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Skärm",
                ImageUrl = "Something"
            });

            context.Set<LoanItemDTO>().Add(new LoanItemDTO
            {
                Id = Guid.NewGuid().ToString(),
                TeamName = "Dreamhack TV",
                LoanedBy = "Someone",
                IsReturned = false,
                LoanedAt = DateTime.Now,
                Item = hwItemPersistedTPKabel
            });
            context.Set<LoanItemDTO>().Add(new LoanItemDTO
            {
                Id = Guid.NewGuid().ToString(),
                TeamName = "Dreamhack TV",
                LoanedBy = "Someone",
                IsReturned = false,
                LoanedAt = DateTime.Now,
                Item = hwItemPersistedTPKabel
            });
            context.Set<LoanItemDTO>().Add(new LoanItemDTO
            {
                Id = Guid.NewGuid().ToString(),
                TeamName = "Dreamhack TV",
                LoanedBy = "Somethree",
                IsReturned = false,
                LoanedAt = DateTime.Now,
                Item = hwItemPersistedTPKabel
            });
            context.Set<LoanItemDTO>().Add(new LoanItemDTO
            {
                Id = Guid.NewGuid().ToString(),
                TeamName = "Dreamhack TV",
                LoanedBy = "MollyPorph",
                IsReturned = false,
                LoanedAt = DateTime.Now,
                Item = hwItemPersistedScreen
            });
            context.Set<LoanItemDTO>().Add(new LoanItemDTO
            {
                Id = Guid.NewGuid().ToString(),
                TeamName = "Media",
                LoanedBy = "Johan",
                IsReturned = false,
                LoanedAt = DateTime.Now,
                Item = hwItemPersistedTPKabel
            });
            context.Set<LoanItemDTO>().Add(new LoanItemDTO
            {
                Id = Guid.NewGuid().ToString(),
                TeamName = "Media",
                LoanedBy = "Grotto",
                IsReturned = false,
                LoanedAt = DateTime.Now,
                Item = hwItemPersistedScreen
            });
            context.Set<LoanItemDTO>().Add(new LoanItemDTO
            {
                Id = Guid.NewGuid().ToString(),
                TeamName = "Media",
                LoanedBy = "Grotto",
                IsReturned = false,
                LoanedAt = DateTime.Now,
                Item = hwItemPersistedScreen
            });

            //List<HardwareItem> HardwareItems = new List<HardwareItem>
            //{
            //    new HardwareItem { Id = Guid.NewGuid().ToString(), Name = "test", ImageUrl = "test" },
            //    new HardwareItem { Id = Guid.NewGuid().ToString(), Name = "Second item", ImageUrl = "test" },
            //};

            //foreach (HardwareItem HardwareItem in HardwareItems)
            //{
            //    context.Set<HardwareItem>().Add(HardwareItem);
            //}

            base.Seed(context);
        }
    }
}

