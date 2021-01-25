using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMenagment.ViewModes
{
    public class EmployeeEditViewModel : EmploeeCreateViewModel
    {
        public int Id { get; set; }
        public string ExistingPhotoPath { get; set; }

    }
}
