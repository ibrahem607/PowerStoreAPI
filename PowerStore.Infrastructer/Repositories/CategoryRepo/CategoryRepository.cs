using PowerStore.Core.Contract.CategoryOfVehicleInterface;
using PowerStore.Core.Entities;
using PowerStore.Infrastructer.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Infrastructer.Repositories.CategoryRepo
{
    public class CategoryRepository:GenaricRepository<CategoryOfVehicle>
    {
        public CategoryRepository(ApplicationDbContext context):base(context)
        {

        }
    }
}

