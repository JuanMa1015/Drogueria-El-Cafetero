using Drogueria_Elcafetero.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Drogueria_Elcafetero.Permisos
{
    public class PermisosRolAttribute : ActionFilterAttribute
    {
        private readonly rol id_rol;

        public  PermisosRolAttribute(rol _id_rol)
        {
            id_rol = _id_rol;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var sessionEmployee = context.HttpContext.Session.GetString("Empleado");
            var sessionAdmin = context.HttpContext.Session.GetString("Administrador");

            if (sessionEmployee != null)
            {
                var employee = JsonConvert.DeserializeObject<Employees>(sessionEmployee);

                if (employee.id_rol != id_rol)
                {
                    context.Result = new RedirectToActionResult("SinPermiso","Home",null);
                }
            }
            else if (sessionAdmin != null)
            {
                var admin = JsonConvert.DeserializeObject<Employees>(sessionAdmin);
                if (admin.id_rol != id_rol)
                {
                    context.Result = new RedirectToActionResult("SinPermiso", "Home", null);
                }
            }
        }
    }
}
