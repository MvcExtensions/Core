#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Demo.Web
{
    using System.ComponentModel.DataAnnotations;
    using Resources;

    public interface IConventionalModel
    {
        int AverageAge { get; set; }
        string FirstName { get; set; }
        int Height { get; set; }
        string LastName { get; set; }
        string MiddleName { get; set; }
        int Weight { get; set; }
    }

    public class ConventionalFluentModel : IConventionalModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int AverageAge { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }
    }

    public class ConventionalFluentModelMetadataConfiguration : ModelMetadataConfiguration<ConventionalFluentModel>
    {
        public ConventionalFluentModelMetadataConfiguration()
        {
            Configure(x => x.FirstName).Required();
            Configure(x => x.LastName).Required().DisplayName(() => LocalizedTexts.LastNameLabel);
            Configure(x => x.MiddleName).Required();
            Configure(x => x.AverageAge).Range(0, 300);
            Configure(x => x.Weight).Range(0, 300);
            Configure(x => x.Height).DisplayName("My Height (from DisplayName)");
        }
    }

    public class ConventionalDataAnnotationsModel : IConventionalModel
    {
        [Required] 
        public string FirstName { get; set; }

        [Display(Name = "LastNameLabel")] [
        Required] 
        public string LastName { get; set; }

        [Required] 
        public string MiddleName { get; set; }

        public int AverageAge { get; set; }

        public int Weight { get; set; }

        [Display(Name = "My Height (from DisplayAttribute)")] 
        public int Height { get; set; }
    }
}
