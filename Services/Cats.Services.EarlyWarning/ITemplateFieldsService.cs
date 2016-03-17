using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{
    public interface ITemplateFieldsService
    {
        bool AddTemplateField(TemplateField templateFields);
        bool DeleteTemplateField(TemplateField templateFields);
        bool DeleteById(int id);
        bool EditTemplateField(TemplateField templateFields);
        TemplateField FindById(int id);
        List<TemplateField> GetAllTemplateField();
        List<TemplateField> FindBy(Expression<Func<TemplateField, bool>> predicate);
    }
}
